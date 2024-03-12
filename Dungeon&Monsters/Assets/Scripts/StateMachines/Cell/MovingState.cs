using Scripts.CellLogic;
using UnityEngine.EventSystems;

namespace Scripts.StateMachines
{

    public class MovingState : IState
    {

        private readonly StateMachine _stateMachine;
        private readonly Cell _cell;

        public MovingState(StateMachine stateMachine, Cell cell)
        {
            _stateMachine = stateMachine;
            _cell = cell;
        }

        public void Enter()
        {
            _cell.PointerClick += Move;
        }

        public void Exit()
        {
            _cell.PointerClick -= Move;
        }

        public void Update()
        {
        }

        private void Move(PointerEventData eventData)
        {
            _cell.ClickMoving?. Invoke (_cell);
        }
    }
}