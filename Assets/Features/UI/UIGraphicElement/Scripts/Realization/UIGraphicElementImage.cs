using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGraphicElementBase : MonoBehaviour, IUIGraphicElement
{
    [SerializeField] protected RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform;
}

public class UIGraphicElementImage : UIGraphicElementBase, IUIGraphicElement<Image>
{
    [SerializeField] protected Image _graphics;
    public Image Graphic => _graphics;

    protected virtual void Init(UIGraphicElementProtocol protocol)
    {
        _graphics.sprite = protocol.Sprite;
        _rectTransform.localScale = Vector3.one;
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, protocol.Size.x);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, protocol.Size.y);
    }

    public class UIGraphicElementImagePool : UIGraphicElementPool<UIGraphicElementImage, UIGraphicElementProtocol>
    {
        public UIGraphicElementImagePool(UIGraphicElementImage prefab, int initCapacity = 0) : base(prefab, initCapacity)
        {
        }

        protected override void InitView(UIGraphicElementImage view, UIGraphicElementProtocol protocol)
        {
            view.Init(protocol);
        }
    }
}

public abstract class UIGraphicElementPool<TView, TProtocol> : IUIGraphicElementsSpawner<TView, TProtocol>
    where TView : UIGraphicElementBase
    where TProtocol : struct
{
    private const string DeactiveContainerName = "UIGraphicElementPoolDeactiveContainer";

    private TView _prefab;
    private Transform _deactiveContainer;
    private Stack<TView> _pool;

    public UIGraphicElementPool(TView prefab, int initCapacity = 0)
    {
        _prefab = prefab;
        _deactiveContainer = new GameObject(DeactiveContainerName).transform;
        _deactiveContainer.gameObject.SetActive(false);
        _pool = new Stack<TView>();
        for (int i = 0; i < initCapacity; i++)
        {
            _pool.Push(CreateNew());
        }
    }

    public void Dispose()
    {
        _prefab = null;

        foreach (var view in _pool)
        {
            UnityEngine.Object.Destroy(view.gameObject);
        }
        _pool.Clear();
        _pool = null;

        UnityEngine.Object.Destroy(_deactiveContainer.gameObject);
        _deactiveContainer = null;
    }

    public TView Spawn(TProtocol protocol)
    {
        TView instance;
        if (_pool.Count > 0)
        {
            instance = _pool.Pop();
        }
        else
        {
            instance = CreateNew();
        }

        instance.RectTransform.SetParent(null);
        InitView(instance, protocol);

        return instance;
    }

    public void Despawn(TView view)
    {
        if (view == null)
        {
            throw new System.Exception("You trying return null to a poll");
        }

        view.RectTransform.SetParent(_deactiveContainer);
        _pool.Push(view);
    }

    protected abstract void InitView(TView view, TProtocol protocol);

    private TView CreateNew()
    {
        return UnityEngine.Object.Instantiate(_prefab, _deactiveContainer);
    }

     
}
public class UIGraphicElementStorage : IUIGraphicElementStorage<UIGraphicElementImage>
{
    public IReadOnlyDictionary<int, UIGraphicElementImage> Items => _data;

    public event EventHandler<int> ItemAdded;
    public event EventHandler<int> ItemRemoved;

    private Dictionary<int, UIGraphicElementImage> _data = new Dictionary<int, UIGraphicElementImage>();

    public void Add(int key, UIGraphicElementImage view)
    {
        if(_data.TryAdd(key, view))
        {
            ItemAdded?.Invoke(this, key);
        }
    }

    public bool TryAdd(int key, UIGraphicElementImage view)
    {
        return _data.TryAdd(key, view);
    }

    public void Remove(int key)
    {
        if(_data.Remove(key))
        {
            ItemRemoved?.Invoke(this, key);
        }
    }

    public void Clear()
    {
        foreach (var item in _data.Keys)
        {
            ItemRemoved?.Invoke(this, item);
        }
        _data.Clear();
    }

    public void Dispose()
    {
        _data.Clear();
        _data = null;
    }

    
}

public readonly struct UIGraphicElementProtocol
{
    public readonly Sprite Sprite;
    public readonly Vector2 Size;

    public UIGraphicElementProtocol(Sprite sprite, Vector2 size)
    {
        Sprite = sprite;
        Size = size;
    }
}