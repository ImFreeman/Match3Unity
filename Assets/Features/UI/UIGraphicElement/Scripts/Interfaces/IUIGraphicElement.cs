using UnityEngine;
using UnityEngine.UI;

namespace Assets.Features.UI.UIGraphicElement.Scripts.Interfaces
{
    public interface IUIGraphicElement
    {
        public RectTransform RectTransform { get; }
    }
    public interface IUIGraphicElement<TGraphic> : IUIGraphicElement
        where TGraphic : Graphic
    {        
        public TGraphic Graphic { get; }
    }
}
