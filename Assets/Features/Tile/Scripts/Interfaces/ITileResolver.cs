using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileResolver
    {
        public UniTask<IEnumerable<Vector2>> Resolve(Vector2 coords);
    }
}
