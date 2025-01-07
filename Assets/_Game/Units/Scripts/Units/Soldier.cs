using SelectionSystem;
using UnityEngine;

namespace Game.Unit
{
    public class Soldier : UnitBase
    {
        public override void Attack(IAttackable target)
        { 
            if(this != target.Owner)
            {
                _stateManager?.ChangeState(_stateManager.attackState, target);

            }


        }

        public override void Command(Vector3 position)
        {
          
            _stateManager.moveState.SetDestination(position);
            _stateManager.ChangeState(_stateManager.moveState, null);
        }
    }
}