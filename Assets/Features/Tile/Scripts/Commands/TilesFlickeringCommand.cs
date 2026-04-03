using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Commands
{
    public class TilesFlickeringCommand : ICommand
    {
        private IEnumerable<(Vector2Int, TileType)> _tileToFlick;        
        private Color _targetColor;
        private readonly float _duration;

        public TilesFlickeringCommand(
            IEnumerable<(Vector2Int, TileType)> tileToFlick,
            Color targetColor,
            float duration)
        {
            _tileToFlick = tileToFlick;
            _targetColor = targetColor;
            _duration = duration;            
        }

        public void Dispose()
        {
            _tileToFlick = null;            
        }

        public void Cancel()
        {
            if(ServiceLocator.TryGet<TileFlickering>(out var flickering))
            {
                flickering.Dispose();
                ServiceLocator.Unregister<TileFlickering>();
            }
        }

        
        public async UniTask<CommandResult> Do()
        {
            var viewStorage = ServiceLocator.Get<IUIGraphicElementStorage<UIGraphicElementImage>>();
            var tileLayout = ServiceLocator.Get<ITileLayout>();
            var blinkTweens = new List<Tween>() { Capacity = _tileToFlick.Count() };
            foreach (var tile in _tileToFlick)
            {
                if (viewStorage.Items.TryGetValue(tileLayout.TilesLayout[tile.Item1.x][tile.Item1.y], out var view))
                {
                    var tween = view.Graphic.DOColor(_targetColor, _duration)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutQuad)
                        .OnKill(() =>
                        {
                            view.Graphic.color = Color.white;
                        })
                        .SetLink(view.gameObject);

                    blinkTweens.Add(tween);
                }
            }

            ServiceLocator.Register(new TileFlickering(
                ServiceLocator.Get<GameWindowPresentor>(),
                blinkTweens,
                () => 
                {
                    if (ServiceLocator.TryGet<TileFlickering>(out var flickering))
                    {
                        flickering.Dispose();
                        ServiceLocator.Unregister<TileFlickering>();
                    }
                }
                ));
            
            return new CommandResult() 
            { 
                Body = blinkTweens,
                Status = CommandStatus.Success 
            };
        }
    }
}
