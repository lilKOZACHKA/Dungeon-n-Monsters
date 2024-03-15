using System.Linq;
using Scripts.CellLogic;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Vector2Int[] _moves;
        [SerializeField] private Vector2Int[] _attackMoves;

        [Space]

        [SerializeField] private int _health;

        [SerializeField] private Cell _cell;

        private int _temp;

        [Header("Gizmos")]
        [SerializeField] private Color _moveColor = Color.white;
        [SerializeField] private Color _attackColor = Color.white;
        [SerializeField] private Color _universalColor = Color.white;

        [Space]

        [SerializeField] private float _spacing;

        [Header("Components")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;

        public Vector2Int[] Moves => _moves;
        public Vector2Int[] AttackMoves => _attackMoves;

        public GameObject GameObject => _gameObject;

        public Transform Transform => transform;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        
        public Unit(int initialHealth)
        {
            _health = initialHealth;
        }

        private void OnDrawGizmosSelected() 
        {
            if(_moves == null || _attackMoves == null) return;

            foreach (Vector2 move in _moves)
            {
                Vector2 position = move * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

                if(_attackMoves.Any(attackMove => attackMove == move))
                {
                    Gizmos.color = _universalColor;

                    Gizmos.DrawWireCube(position, _transform.localScale);

                    continue;
                }

                Gizmos.color = _moveColor;

                Gizmos.DrawCube(position, _transform.localScale);
            }

            Gizmos.color = _attackColor;

            foreach (Vector2 attackMove in _attackMoves)
            {
                Vector2 position = attackMove * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

                if(_moves.Any(move => move == attackMove)) continue;

                Gizmos.DrawCube(position, _transform.localScale);
            }
        }

        public void Initialize(Cell cell)
        {
            int setResult = cell.SetUnit(this);

            if (setResult == 0)
            {
                _cell.SetUnit(); 
            }

            _transform.position = cell.Transform.position;
            _cell = cell;
        }

        public void FindCell()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_transform.position, _transform.localScale, 0);

            if(colliders.Length == 0) return;

            Cell cell = null;
            colliders.First(collider => {cell = collider.GetComponent<Cell>(); return cell != null; });

            if(cell != null)
            {
                cell.SetUnit(this);

                _cell = cell;
            }
        }
    }
}