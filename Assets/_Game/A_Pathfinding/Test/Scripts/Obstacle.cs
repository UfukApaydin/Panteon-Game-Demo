using A_Pathfinding.Pathfinding;
using UnityEngine;

namespace A_Pathfinding.Test
{
    public class Obstacle : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            ServiceLocator.Get<PathfindingDirector>().grid.UpdateNodesWalkable(transform.position, 2, false);
        }


    }
}