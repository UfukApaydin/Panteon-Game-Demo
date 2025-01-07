using SelectionSystem.Marker;
using System;
using UnityEngine;

namespace SelectionSystem
{
    public interface ISelectable
    {
        GameObject Owner { get; }
        void Select(SelectionMarker selectionMarker);
        void Deselect();
        void Command(Vector3 positon);

    }

    public interface IAttackable : ISelectable
    {
        Action<int> OnHealthChange { get; set; }
        Vector2 Objectsize { get; }
        void TakeDamage(int damage);
    }
    public interface IAttacker : IAttackable
    {
        void Attack(IAttackable target);
    }
}