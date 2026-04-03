using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Features.UI.GameWindow.Scripts
{
    public class TilesField : MonoBehaviour, IPointerDownHandler
    {
        public event EventHandler<Vector2> PointerDown;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _background;
        [SerializeField] private Color _firstColor;
        [SerializeField] private Color _secondColor;

        private void Start()
        {
            GenerateBackgroundTexture(new Vector2Int(8, 8), 100);
        }

        public void GenerateBackgroundTexture(Vector2Int numOfTiles, int cellSize)
        {
            int textureWidth = numOfTiles.x * cellSize;
            int textureHeight = numOfTiles.y * cellSize;

            var chessboardTexture = new Texture2D(textureWidth, textureHeight);
            chessboardTexture.wrapMode = TextureWrapMode.Clamp;
            chessboardTexture.filterMode = FilterMode.Point;

            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    int cellX = x / cellSize;
                    int cellY = y / cellSize;
                    Color cellColor = ((cellX + cellY) % 2 == 0) ? _firstColor : _secondColor;
                    chessboardTexture.SetPixel(x, y, cellColor);
                }
            }

            chessboardTexture.Apply();

            Sprite sprite = Sprite.Create(chessboardTexture,
                                          new Rect(0, 0, textureWidth, textureHeight),
                                          new Vector2(0.5f, 0.5f));
            _background.sprite = sprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var rez = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var point
                );
            PointerDown?.Invoke(this, new Vector2(point.x, Mathf.Abs(point.y)));
        }
    }
}
