using Assets.Features.Core.ServiceLocatorScript;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Features.Core.Command.Realization
{
    public abstract class BaseClearSceneCommand : ICommand
    {
        public virtual void Cancel()
        {}

        public virtual void Dispose()
        {}

        public abstract UniTask<CommandResult> Do();

        protected void DisposeService<T>() where T : class, IDisposable
        {
            if (ServiceLocator.TryGet<T>(out T service))
            {       
                service.Dispose();
                ServiceLocator.Unregister<T>();
            }
        }
    }
}
