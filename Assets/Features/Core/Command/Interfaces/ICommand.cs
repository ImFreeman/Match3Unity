using Cysharp.Threading.Tasks;
using System;

public enum CommandStatus
{
    Success,
    Canceled,
    Failed
}
public class CommandResult
{
    public object Body;
    public CommandStatus Status;
}

public interface ICommand : IDisposable
{
    public UniTask<CommandResult> Do();
    public void Cancel();
}