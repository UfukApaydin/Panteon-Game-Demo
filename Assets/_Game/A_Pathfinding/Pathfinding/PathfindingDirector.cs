using GridSystem;

namespace AStarPathfinding
{
    public class PathfindingDirector
    {
        public PathfindingGrid grid;
        public Pathfinding pathfinding;
        public PathRequestManager pathRequestManager;
        public PathfindingDirector InitializePathfinding(GridConfig gridConfig)
        {
            grid = new PathfindingGrid.Builder()
               .SetGridWorldSize(gridConfig.gridWorldSize)
               .SetNodeRadius(gridConfig.CellHalfSize)
               .BuildAndStart();
            Pathfinding pathfinding = new Pathfinding(grid);
            PathRequestManager pathRequestManager = new PathRequestManager.Builder()
                .Build(pathfinding, grid);
            return this;
        }
    }
}
