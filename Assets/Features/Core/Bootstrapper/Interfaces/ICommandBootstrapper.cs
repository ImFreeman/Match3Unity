using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public interface ICommandBootstrapper : IDisposable
{
    public void AddCommand(ICommand command);
    public UniTask ExecuteAsync(CancellationToken cancellationToken);
    public void Cancel();
}
