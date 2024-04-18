using System.Linq;
using System.Security.Cryptography;
using Scripts.CellLogic;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Vector2Int[] _moves;
        [SerializeField] private Vector2Int[] _attackMoves;

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        [SerializeField] private Cell _cell;

        [Header("Gizmos")]
        [SerializeField] private Color _moveColor = Color.white;
        [SerializeField] private Color _attackColor = Color.white;
        [SerializeField] private Color _universalColor = Color.white;

        [Space]

        [SerializeField] private int _health = 10;
        [SerializeField] private int _initiative;
        [SerializeField] private int _moveCount = 0;
        [SerializeField] private int _moveCountMax = 3;

        [SerializeField] private bool _isCombat = false;
        [SerializeField] private bool _isUnion = false;
        [SerializeField] private bool _isActive = true;

        [SerializeField] private float _spacing;

        public Vector2Int[] Moves => _moves;
        public Vector2Int[] AttackMoves => _attackMoves;


        private void OnDrawGizmosSelected()
        {
            if (_moves == null || _attackMoves == null) return;

            foreach (Vector2 move in _moves)
            {
                Vector2 position = move * transform.localScale * _spacing + (Vector2)transform.localPosition;

                if (_attackMoves.Any(attackMove => attackMove == move))
                {
                    Gizmos.color = _universalColor;
                    Gizmos.DrawWireCube(position, transform.localScale);
                    continue;
                }

                Gizmos.color = _moveColor;
                Gizmos.DrawCube(position, transform.localScale);
            }

            Gizmos.color = _attackColor;

            foreach (Vector2 attackMove in _attackMoves)
            {
                if (_moves.Any(move => move == attackMove)) continue;

                Vector2 position = attackMove * transform.localScale * _spacing + (Vector2)transform.localPosition;
                Gizmos.DrawCube(position, transform.localScale);
            }
        }

        public void Initialize(Cell cell)
        {
            if (cell.IsWalkable)
            {
                if (cell.SetUnit(this) == 0)
                {
                    if (_cell != null) { _cell.SetUnit(); }
                    transform.position = cell.Transform.position;
                    _cell = cell;
                }
            }
        }

        public void FindCell()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);

            if (colliders.Length == 0) return;

            Cell cell = null;
            colliders.First(collider => { cell = collider.GetComponent<Cell>(); return cell != null; });

            if (cell != null)
            {
                cell.SetUnit(this);
                _cell = cell;
            }
        }
    }
}
