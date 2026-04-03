using Assets.Features.GameLogic.Scripts.Interfaces;
using System;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class ScoreHandler : IScoreHandler
    {
        public event EventHandler<int> ScoreUpdated;
        public int Score
        {
            get => _score;
            set
            {
                if (_score != value)
                {
                    _score = value;
                    ScoreUpdated?.Invoke(this, _score);
                }
            }
        }

        private int _score;

        public void Dispose()
        {
            
        }
    }
}