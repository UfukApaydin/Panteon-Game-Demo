using A_Pathfinding.Nodes;
using UnityEngine;

namespace A_Pathfinding.Pathfinding
{
    public class PathfindingDirector
    {
        public PathfindingGrid grid;
        public Pathfinding pathfinding;
        public PathRequestManager pathRequestManager;
       public PathfindingDirector InitializePathfinding(Vector2 worldGridSize, float nodeRadius, LayerMask unwalkableLayerMask)
       {
             grid = new PathfindingGrid.Builder()
                .SetGridWorldSize(worldGridSize)
                .SetNodeRadius(nodeRadius)
                .SetLayerMask(unwalkableLayerMask)
                .BuildAndStart();
            Pathfinding pathfinding = new Pathfinding(grid);
            PathRequestManager pathRequestManager = new PathRequestManager.Builder()
                .Build(pathfinding);
            return this;
       }
    }
}
