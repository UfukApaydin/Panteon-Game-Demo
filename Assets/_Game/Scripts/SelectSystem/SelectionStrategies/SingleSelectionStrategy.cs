using UnityEngine;
using UnityEngine.EventSystems;

namespace SelectionSystem
{
    public class SingleSelectionStrategy : ISelectionStrategy
    {
        public void Select(SelectionManager selectionManager)
        {
            selectionManager.ClearSelection();
      
            if(MouseTools.GetMousePosition(out RaycastHit2D hit))
            {
                if (hit.collider != null)
                {
                    //   DebugManager.instance.PrintLog($"{hit.transform.name} : selected");
                    if (hit.collider.TryGetComponent<ISelectable>(out var selectable))
                    {
                        selectionManager.AddToSelection(selectable);
                    }
                }
            }
         
        }
    }
}

