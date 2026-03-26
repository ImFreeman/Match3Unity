using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileSpawner<TModel> : IDisposable
        where TModel : ITileModel
    {
        public void SetMaxSize(int count);
        public void Despawn(TModel model);
    }

    public interface ITileSpawner<TModel, TEnumType> : ITileSpawner<TModel>
        where TModel : ITileModel<TEnumType>
        where TEnumType : Enum
    {
        public TModel Spawn(TEnumType type, ITileResolver resolver);
    }

    public interface ITileSpawner<TModel, TEnumType, TProtocol> : ITileSpawner<TModel>
        where TModel : ITileModel
        where TEnumType : Enum
        where TProtocol : struct
    {
        public TModel Spawn(TProtocol protocol);
    }
}
