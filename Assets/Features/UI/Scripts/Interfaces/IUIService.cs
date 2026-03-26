using Assets.Features.UI.Scripts.Realization;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public interface IUIService : IDisposable
{
    public UniTask Init(string windowPoolName);
    public T Show<T>() where T : UIWindow;
    public T Get<T>() where T : UIWindow;
    public void Hide<T>() where T : UIWindow;
}
