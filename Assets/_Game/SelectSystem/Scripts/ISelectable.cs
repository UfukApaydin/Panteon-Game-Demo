using UnityEngine;
using SelectionSystem.Marker;
using UnityEngine.Events;

namespace SelectionSystem
{
    public interface ISelectable
    {

        void Select(SelectionMarker selectionMarker);
        void Deselect();
        void Execute(Vector3 positon);
    }
}