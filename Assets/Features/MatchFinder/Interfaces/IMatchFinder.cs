using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchFinder : IDisposable
{
    public UniTask<IEnumerable<Vector2>> ResolveMatch(Vector2 origin, Vector2[] match);
}
