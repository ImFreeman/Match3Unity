using System;

namespace Assets.Features.GameLogic.Scripts.Interfaces
{
    public interface IScoreHandler : IDisposable
    {
        public event EventHandler<int> ScoreUpdated;
        public int Score { get; set; }
    }
}