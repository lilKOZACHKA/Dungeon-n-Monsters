using Scripts.CellLogic;
using UnityEngine.EventSystems;

namespace Scripts.StateMachines
{
    public class AttackState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly Cell _cell;

        public AttackState(StateMachine stateMachine, Cell cell)
        {
            _stateMachine = stateMachine;
            _cell = cell;
        }
        public void Enter()
        {
            _cell.PointerClick += Attack;
        }

        public void Exit()
        {
            _cell.PointerClick -= Attack;
        }

        public void Update()
        {
        }

        private void Attack(PointerEventData eventData)
        {
            _cell.ClickMoving?. Invoke(_cell);
        }
    }
}