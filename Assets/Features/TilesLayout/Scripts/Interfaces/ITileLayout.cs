using System.Collections.Generic;

namespace Assets.Features.TilesLayout.Scripts.Interfaces
{
    public interface ITileLayout
    {
        public int[][] TilesLayout { get; set; }
        public IEnumerable<(int column, int row)> GetIndexes();
    }
}
