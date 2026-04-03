using System;

namespace Assets.Features.UI.UIGraphicElement.Scripts.Interfaces
{
    public interface IUIGraphicElementsSpawner<TView, TProtocol> : IDisposable
        where TView : IUIGraphicElement
        where TProtocol : struct
    {        
        public TView Spawn(TProtocol protocol);
        public void Despawn(TView view);
    }
}
