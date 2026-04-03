using Assets.Features.GameLogic.Scripts.Interfaces;
using System;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class MovesHandler : IMovesHandler
    {
        public event EventHandler<int> MovesCountUpdated;
        public int MovesCount 
        {
            get => _movesCount;
            set
            {
                if(_movesCount != value)
                {
                    _movesCount = value;
                    MovesCountUpdated?.Invoke(this, _movesCount);
                }
            }
        }        

        private int _movesCount;

        public void Dispose()
        {
            
        }
    }
}
