using SelectionSystem;

namespace Game.Unit
{
    public class StateManager
    {
        public UnitBase unit;
        public StateBase currentState;
        public IdleState idleState;
        public MoveState moveState;
        public AttackState attackState;


        public StateManager(UnitBase unit, UnitData unitData)
        {
            this.unit = unit;

            idleState = new(this);
            moveState = new(this);
            attackState = new(this, unitData);

            currentState = idleState;
            currentState.OnStateEnter(null);
        }

        public void ChangeState(StateBase newState, IAttackable target)
        {
            currentState.OnStateExit();
            currentState = newState;
            currentState.OnStateEnter(target);
        }
    }
}