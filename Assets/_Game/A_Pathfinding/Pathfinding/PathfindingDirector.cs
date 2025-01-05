using A_Pathfinding.Nodes;
using GridSystem;
using UnityEngine;

namespace A_Pathfinding.Pathfinding
{
    public class PathfindingDirector
    {
        public PathfindingGrid grid;
        public Pathfinding pathfinding;
        public PathRequestManager pathRequestManager;
       public PathfindingDirector InitializePathfinding(GridConfig gridConfig/*, LayerMask unwalkableLayerMask*/)
       {
             grid = new PathfindingGrid.Builder()
                .SetGridWorldSize(gridConfig.gridWorldSize)
                .SetNodeRadius(gridConfig.CellHalfSize)
              //  .SetLayerMask(unwalkableLayerMask)
                .BuildAndStart();
            Pathfinding pathfinding = new Pathfinding(grid);
            PathRequestManager pathRequestManager = new PathRequestManager.Builder()
                .Build(pathfinding);
            return this;
       }
    }
}
