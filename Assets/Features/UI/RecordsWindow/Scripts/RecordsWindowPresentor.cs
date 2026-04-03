using Assets.Features.Core.Command.Realization;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Features.UI.RecordsWindow.Scripts
{
    public class RecordsWindowPresentor : IDisposable
    {
        private UIRecordsWindow _window;
        private IUIGraphicElementsSpawner<RecordDataView, RecordDataViewProtocol> _recordDataViewSpawner;        
        private IRecordsTrack _records;

        private ICommand _leaveSceneCommand;
        private readonly int _mainMenuSceneId;
        private IDictionary<DateTime, RecordDataView> _views;

        public RecordsWindowPresentor(
            IRecordsTrack records,
            IUIService uiService,
            IUIGraphicElementsSpawner<RecordDataView, RecordDataViewProtocol> recordDataViewSpawner,
            int mainMenuSceneId)
        {
            _window = uiService.Get<UIRecordsWindow>();
            _records = records;
            _recordDataViewSpawner = recordDataViewSpawner;
            _mainMenuSceneId = mainMenuSceneId;

            _leaveSceneCommand = new LeaveRecordsSceneCommand(_mainMenuSceneId);

            _window.Shown += OnShown;
            _window.Hidden += OnHidden;
            _window.MainMenuButtonClicked += OnMainMenuButtonClicked;

            _views = new Dictionary<DateTime, RecordDataView>();
        }

        public void Dispose()
        {
            _leaveSceneCommand.Dispose();
            _leaveSceneCommand = null;

            _window.MainMenuButtonClicked -= OnMainMenuButtonClicked;
            _window.Shown -= OnShown;
            _window.Hidden -= OnHidden;

            foreach (var view in _views.Values)
            {
                _recordDataViewSpawner.Despawn(view);
            }
            _views.Clear();
            _views = null;

            _window = null;
            _records = null;
        }

        public RecordDataView GetView(DateTime date)
        {
            if(_views.TryGetValue(date, out var view))
            {
                return view;
            }

            Debug.LogError("There is no view for rhis date");
            return null;
        }

        private void OnMainMenuButtonClicked(object sender, EventArgs e)
        {
            _leaveSceneCommand.Do();
        }

        private void OnShown(object sender, EventArgs e)
        {
            var recordsData = _records.GetAllRecods().OrderByDescending(r => r.Value);
            foreach (var record in recordsData)
            {
                var view = _recordDataViewSpawner.Spawn(new RecordDataViewProtocol(record.Value.ToString(), record.Date.ToString()));
                view.RectTransform.SetParent(_window.ContentTransform);
                _views.Add(record.Date, view);
            }
        }

        private void OnHidden(object sender, EventArgs e)
        {
            foreach (var view in _views.Values)
            {
                _recordDataViewSpawner.Despawn(view);
            }
            _views.Clear();
        }

        
    }
}
