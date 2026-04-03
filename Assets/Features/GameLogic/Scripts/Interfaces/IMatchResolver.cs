using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Interfaces
{
    public interface IMatchResolver : IDisposable
    {
        public UniTask<IEnumerable<Vector2Int>> ResolveMatch(IEnumerable<Vector2Int> match);
    }
}