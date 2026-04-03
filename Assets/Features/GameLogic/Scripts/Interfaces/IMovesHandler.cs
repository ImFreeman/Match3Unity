using System;

namespace Assets.Features.GameLogic.Scripts.Interfaces
{
    public interface IMovesHandler : IDisposable
    {
        public event EventHandler<int> MovesCountUpdated;
        public int MovesCount { get; set; }
    }
}
