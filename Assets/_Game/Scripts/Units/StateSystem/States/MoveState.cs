using SelectionSystem;
using UnityEngine;

namespace Game.Unit
{
    public class MoveState : StateBase
    {
        private Vector3 destination;
        private bool arrived = false;

        public MoveState(StateManager stateManager) : base(stateManager) { }

        public void SetDestination(Vector3 dest)
        {
            destination = dest;
        }

        public override void OnStateEnter(IAttackable target)
        {
            arrived = false;
            unitBase.Agent.MoveToPosition(destination);
            Debug.Log($"{unitBase.name} moving to {destination}");
        }

        public override void OnStateExit()
        {
            // Nothing special on exit
        }

        public override void OnStateUpdate()
        {
            // If Agent has arrived, switch to Idle
            // (or Attack if there's some queued command)
            if (!unitBase.Agent.IsMoving)
            {
                if (!arrived)
                {
                    arrived = true;
                    Debug.Log($"{unitBase.name} has arrived at the destination.");
                    _stateManager.ChangeState(_stateManager.idleState, null);
                }
            }
        }
    }
}
