using SelectionSystem;
using UnityEngine;

namespace Game.Unit
{
    public class AttackState : StateBase
    {
       
        private UnitData _unitData;

        private bool isAttacking;
        private float nextAttackTime;
        private GameObject targetGO;
        private IAttackable attackable = null;
        public AttackState(StateManager stateManager,UnitData unitData) : base(stateManager)
        {
           _unitData = unitData;
        }

        public override void OnStateEnter(IAttackable target)
        {
            if (target == null)
            {
                _stateManager.ChangeState(_stateManager.idleState, null);
                return;
            }
            attackable = target;
            targetGO = target.Owner;
            isAttacking = true;
            nextAttackTime = 0f; // Attack immediately if in range
        }

        public override void OnStateExit()
        {
            isAttacking = false;
            targetGO = null;
        }

        public override void OnStateUpdate()
        {
            if (!isAttacking || targetGO == null)
                return;


            if (!targetGO || !targetGO.activeSelf)
            {
                _stateManager.ChangeState(_stateManager.idleState, null);
            }

            //Get closest point from bounding box
            float clampedX = Mathf.Clamp(unitBase.transform.position.x
                , targetGO.transform.position.x - attackable.Objectsize.x / 2
                , targetGO.transform.position.x + attackable.Objectsize.x / 2);
            float clampedY = Mathf.Clamp(unitBase.transform.position.y
                , targetGO.transform.position.y - attackable.Objectsize.y / 2
                , targetGO.transform.position.y + attackable.Objectsize.y / 2);


            // The nearest point on (or in) the bounding box
            Vector2 nearestPoint = new Vector2(clampedX, clampedY);

            // Distance from the agent to this nearest point
            float distance = Vector2.Distance(unitBase.transform.position, nearestPoint);

            if (distance >= _unitData.attackRange)
            {
                if (!unitBase.Agent.IsMoving)
                    unitBase.Agent.MoveToPosition(targetGO.transform.position);
            }
            else
            {
                // We are in range, so handle attack with cooldown
                if (Time.time >= nextAttackTime)
                {
                    // Attack
                    if (targetGO.TryGetComponent<IAttackable>(out var attackable))
                    {
                        unitBase.Agent.SetRotationToTarget(nearestPoint);
                        unitBase.AnimateAttack(() =>
                        {
                            attackable?.TakeDamage(_unitData.attackDamage);

                            Debug.Log($"{unitBase.name} attacked {targetGO.name} for {_unitData.attackDamage} damage!");
                        });
                 
                    }
                    // Set next time we can attack
                    nextAttackTime = Time.time + _unitData.attackSpeed;
                }
            }

        }
    }
}
