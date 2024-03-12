using Scripts.StateMachines;
using UnityEngine;

namespace Scripts.CellLogic
{
    public class CellView : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _enterColor = Color.white;
        [SerializeField] private Color _selectColor = Color.white;
        [SerializeField] private Color _selectEnterColor = Color.white;
        [SerializeField] private Color _movingColor = Color.white;
        [SerializeField] private Color _movingEnterColor = Color.white;
        [SerializeField] private Color _attackColor = Color.white;
        [SerializeField] private Color _attackEnterColor = Color.white;


        [Header("Components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Cell _cell;


        private void OnEnable() 
        {
            _cell.StateChanged += (state, oldState, sender) => SetColorByState(state, sender.PointerEnter);
            _cell.PointerChanged += (pointerEnter, sender) => SetColorByState(sender.CurrentState, pointerEnter);
        }
        private void OnDisable()
        {
            _cell.StateChanged -= (state, oldState, sender) => SetColorByState(state, sender.PointerEnter);
            _cell.PointerChanged -= (pointerEnter, sender) => SetColorByState(sender.CurrentState, pointerEnter);
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (_cell == null)
            {
                _cell = GetComponent<Cell>();
            }
        }

        private void SetColorByState(IState cellState, bool pointerEnter)
        {
            _spriteRenderer.color = (cellState, pointerEnter) switch
            {
                (DefaultState, true) => _enterColor,
                (DefaultState, _) => _defaultColor,
                (SelectState, true) => _selectEnterColor,
                (SelectState, _) => _selectColor,
                (MovingState, true) => _movingEnterColor,
                (MovingState, _) => _movingColor,
                (AttackState, true) => _attackEnterColor,
                (AttackState, _) => _attackColor,
                _ => _spriteRenderer.color,
            };
        }
    }
}
