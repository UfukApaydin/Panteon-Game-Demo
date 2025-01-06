using SelectionSystem;

namespace Game.Unit
{
    public abstract class StateBase
    {
     
        protected StateManager _stateManager;
        protected UnitBase unitBase => _stateManager.unit;
        public StateBase(StateManager stateManager)
        {
            _stateManager = stateManager; 
        }
        public abstract void OnStateEnter(IAttackable target);
        public abstract void OnStateExit();
        public abstract void OnStateUpdate();
    }
}