using SelectionSystem;
using UnityEngine;

namespace Game.Unit
{
    public class IdleState : StateBase
    {
        public IdleState(StateManager stateManager) : base(stateManager) { }

        public override void OnStateEnter(IAttackable target)
        {

            Debug.Log($"{unitBase.name} is now IDLE");
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {

        }
    }
}