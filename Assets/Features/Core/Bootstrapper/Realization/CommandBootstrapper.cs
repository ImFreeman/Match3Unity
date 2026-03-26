using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Core.Bootstrapper.Realization
{
    public class CommandBootstrapper : ICommandBootstrapper
    {
        public event Action<ICommand, CommandResult> CommandExecuted;

        private readonly Queue<ICommand> _commandQueue = new Queue<ICommand>();
        private ICommand _currentCommand;
        private CancellationTokenSource _cts;
        private bool _isExecuting;
        private bool _disposed;
        private readonly object _lock = new object();        
        
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Cancel();
            lock (_lock)
            {
                while (_commandQueue.Count > 0)
                {
                    _commandQueue.Dequeue().Dispose();
                }
            }
            _cts?.Dispose();
        }

        public void AddCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            lock (_lock)
            {
                _commandQueue.Enqueue(command);
            }
        }
        
        public async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            lock (_lock)
            {
                if (_isExecuting)
                    throw new InvalidOperationException("Execution already in progress");
                _isExecuting = true;
            }

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                while (true)
                {
                    ICommand command = null;
                    lock (_lock)
                    {
                        if (_commandQueue.Count == 0)
                            break;
                        command = _commandQueue.Dequeue();
                    }

                    if (_cts.Token.IsCancellationRequested)
                    {
                        command?.Dispose();
                        break;
                    }

                    _currentCommand = command;
                    CommandResult result = null;

                    try
                    {
                        result = await command.Do();
                        OnCommandExecuted(command, result);
                    }
                    catch (OperationCanceledException)
                    {
                        result = new CommandResult { Status = CommandStatus.Failed, Body = "Command cancelled" };
                        OnCommandExecuted(command, result);
                        break;
                    }
                    catch (Exception ex)
                    {
                        result = new CommandResult { Status = CommandStatus.Failed, Body = ex };
                        OnCommandExecuted(command, result);
                        break;
                    }
                    finally
                    {
                        command.Dispose();
                        _currentCommand = null;
                    }

                    if (result != null && result.Status == CommandStatus.Failed)
                    {
                        break;
                    }
                }
            }
            finally
            {
                lock (_lock)
                {
                    _isExecuting = false;
                }
                _cts?.Dispose();
                _cts = null;
            }
        }

        public void Cancel()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }

            _currentCommand?.Cancel();
        }

        private void OnCommandExecuted(ICommand command, CommandResult result)
        {
            CommandExecuted?.Invoke(command, result);
        }        
    }
}
