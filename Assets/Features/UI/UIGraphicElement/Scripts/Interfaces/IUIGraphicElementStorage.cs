using System;
using System.Collections.Generic;

namespace Assets.Features.UI.UIGraphicElement.Scripts.Interfaces
{
    public interface IUIGraphicElementStorage<TView> : IDisposable
        where TView : IUIGraphicElement
    {
        public event EventHandler<int> ItemAdded;
        public event EventHandler<int> ItemRemoved;
        public IReadOnlyDictionary<int, TView> Items { get; }
        public void Add(int key, TView view);
        public bool TryAdd(int key, TView view);
        public void Remove(int key);
        public void Clear();        
    }
}
