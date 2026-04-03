using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileShifter : ITileShifter
    {
        private ITileLayout _tileLayout;
        private IUIGraphicElementStorage<UIGraphicElementImage> _viewStorage;
        private GameWindowPresentor _windowPresenter;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public TileShifter(
            ITileLayout tileLayout,
            IUIGraphicElementStorage<UIGraphicElementImage> viewStorage,
            GameWindowPresentor windowPresenter)
        {
            _tileLayout = tileLayout;
            _viewStorage = viewStorage;
            _windowPresenter = windowPresenter;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;

            _tileLayout = null;
            _viewStorage = null;
            _windowPresenter = null;
        }

        public async UniTask Shift(int tileId, Vector2Int to)
        {
            if(_viewStorage.Items.TryGetValue(tileId, out var item))
            {
                var fromPosition = _windowPresenter.GetPositionFromLayoutPosition(new Vector2Int(to.x, -1));
                item.RectTransform.anchoredPosition = fromPosition;
                await MoveTile(item, _windowPresenter.GetPositionFromLayoutPosition(to), 1.0f);
            }
        }

        public async UniTask Shift(Vector2Int from, Vector2Int to)
        {
            if (_viewStorage.Items.TryGetValue(_tileLayout.TilesLayout[from.x][from.y], out var item))
            {
                await MoveTile(item, _windowPresenter.GetPositionFromLayoutPosition(to), 1.0f);
            }
        }

        private async UniTask MoveTile(UIGraphicElementImage tile, Vector2 position, float duration)
        {
            await tile.RectTransform
                    .DOAnchorPos(position, duration)
                    .ToUniTask(TweenCancelBehaviour.Kill, _cancellationTokenSource.Token);
        }
    }
}
