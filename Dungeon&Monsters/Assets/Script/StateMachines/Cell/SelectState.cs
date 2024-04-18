using Scripts.CellLogic;
using UnityEngine.EventSystems;

namespace Scripts.StateMachines
{
    public class SelectState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly Cell _cell;

        public SelectState(StateMachine stateMachine, Cell cell)
        {
            _stateMachine = stateMachine;
            _cell = cell;
        }

        public void Enter()
        {
            _cell.PointerClick += Unselect;
        }

        public void Exit()
        {
            _cell.PointerClick -= Unselect;
        }

        public void Update()
        {
        }

        private void Unselect(PointerEventData eventData)
        {
            _stateMachine.ChangeState( _cell.DefaultState);
        }
    }
}
