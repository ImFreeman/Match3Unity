using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Features.UI.RecordsWindow.Scripts
{
    public class RecordDataView : UIGraphicElementBase, IUIGraphicElement<Image>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _scoreText;

        public Image Graphic => _image;

        private void Init(RecordDataViewProtocol protocol)
        {
            _dateText.text = protocol.Date;
            _scoreText.text = protocol.Score;
        }

        public class RecordDataViewePool : UIGraphicElementPool<RecordDataView, RecordDataViewProtocol>
        {
            public RecordDataViewePool(RecordDataView prefab, int initCapacity = 0) : base(prefab, initCapacity)
            {
            }

            protected override void InitView(RecordDataView view, RecordDataViewProtocol protocol)
            {
                view.Init(protocol);
            }
        }
    }

    public readonly struct RecordDataViewProtocol
    {
        public readonly string Date;
        public readonly string Score;

        public RecordDataViewProtocol(string score, string date) : this()
        {
            Score = score;
            Date = date;
        }
    }
}