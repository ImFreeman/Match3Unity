using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Interfaces
{
    public interface IMatchFinder : IDisposable
    {
        public UniTask<IEnumerable<Vector2Int>> FindMatch(Vector2Int origin, int[][] matrix);
    }
}