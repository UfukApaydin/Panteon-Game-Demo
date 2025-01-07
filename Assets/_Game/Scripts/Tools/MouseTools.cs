using UnityEngine.EventSystems;
using UnityEngine;

public static class MouseTools
{
    public static bool GetMousePosition(out RaycastHit2D hit)
    {
        hit = new RaycastHit2D();
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {

            return false;
        }

        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return true;
    }
    public static bool GetMousePosition(out RaycastHit2D hit, LayerMask layerMask)
    {
        hit = new RaycastHit2D();
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {

            return false;
        }

        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return true;
    }

}