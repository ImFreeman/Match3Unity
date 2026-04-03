using DG.Tweening;
using System;
using System.Collections.Generic;

namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileFlickering : IDisposable
    {
        private IEnumerable<Tween> _tweens;
        private GameWindowPresentor _window;
        private Action _onClick;

        private bool _subs = true;

        public TileFlickering(GameWindowPresentor window, IEnumerable<Tween> tweens, Action onClick)
        {
            _onClick = onClick;
            _tweens = tweens;
            _window = window;
            _window.ClickOnTile += OnClickOnTile;
        }

        private void OnClickOnTile(object sender, UnityEngine.Vector2Int e)
        {
            _window.ClickOnTile -= OnClickOnTile;            
            foreach (var tween in _tweens)
            {
                tween.Kill();
            }
            _onClick();
        }

        public void Dispose()
        {
            if(_subs)
            {
                _window.ClickOnTile -= OnClickOnTile;
                foreach (var tween in _tweens)
                {
                    tween.Kill();
                }                
            }
            _tweens = null;
            _window = null;
        }
    }
}
