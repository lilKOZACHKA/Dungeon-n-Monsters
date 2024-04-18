using System;
using System.Collections;
using System.Collections.Generic;
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

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        [SerializeField] private Cell _cell;

        [Header("Gizmos")]
        [SerializeField] private Color _moveColor = Color.white;
        [SerializeField] private Color _attackColor = Color.white;
        [SerializeField] private Color _universalColor = Color.white;

        [Space]

        [SerializeField] public int _health = 10;
        [SerializeField] public int _initiative;
        [SerializeField] public int _moveCount = 0;
        [SerializeField] public int _moveCountMax = 3;

        [SerializeField] public bool _isCombat = false;
        [SerializeField] public bool _isUnion = false;
        [SerializeField] public bool _isActive = true;

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

        public void DoTurn(Unit currentUnit)
        {

            Debug.Log("��� ��������� c ����������� - " + currentUnit._initiative);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public Vector2Int GetCellPosition()
        {
            if (_cell != null)
            {
                return _cell.Position;
            }
            else
            {
                Debug.LogError("������: � ����� ��� ������ �� ������.");
                return Vector2Int.zero;
            }
        }

        public void BotTurn(Unit currentUnit, Transform transform, Vector2Int position, List<Unit> units)
        {
            Debug.Log("��� ���� c ����������� - " + currentUnit._initiative);

            if (currentUnit._isCombat)
            {
                try
                {
                        Unit targetUnit = FindUnionUnit(currentUnit, units);
                    if (targetUnit == null)
                    {
                        Debug.Log("������� ���� ��� ���� �� ������.");
                        return;
                    }

                    Vector2Int targetPosition = targetUnit.GetCellPosition();
                    Vector2Int startPosition = currentUnit.GetCellPosition();

                    List<Vector2Int> path = CalculatePath(startPosition, targetPosition);

                
                        StartCoroutine(MoveAlongPathWithUnit(path, currentUnit, startPosition, () =>
                        {
                            Initiative initiative = FindObjectOfType<Initiative>();
                            initiative.EndRound();

                            // ��������� ������� ���� ����� ���������� �����������
                            currentUnit.Initialize(currentUnit.GetCellAtPosition(position));
                        }));
                    }
                catch
                {
                    Initiative initiative = FindObjectOfType<Initiative>();
                    initiative.EndRound();

                    Debug.Log("���������");
                }
            }
        }

        // ����� ��� ������ �������� ����� (IsUnion == true) ��� �������� �����
        private Unit FindUnionUnit(Unit currentUnit, List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                if (unit._isUnion && unit != currentUnit) // ��������� �������� ����� �� ������
                {
                    return unit;
                }
            }
            return null; 
        }




        private IEnumerator MoveAlongPathWithUnit(List<Vector2Int> path, Unit unit, Vector2Int initialPosition, Action onComplete)
        {
            foreach (Vector2Int position in path)
            {
                // ���������� ������, ���� ��� ��������� � ��������� �������� ����
                if (position == initialPosition)
                {
                    continue;
                }

                // �������� ������ ������ ��� ������� �������
                Cell targetCell = GetCellAtPosition(position);

                // ���������, ��� ������ �������
                if (targetCell != null)
                {
                    // ���������� ���� �� ������
                    if (targetCell.SetUnit(unit) == 0)
                    {
                        // ������� ���������� ����� ����
                        // ����� ��������� �������������� ��������
                        if (_cell != null && _cell != targetCell)
                        {
                            // ���� ������� ������ �� null � ���������� �� ������� ������
                            _cell.SetUnit(); // ������� ������� ���� �� ������� ������
                        }

                        transform.position = targetCell.Transform.position;
                        _cell = targetCell;
                    }
                    // ���������, ������� �� ���������� ���� �� ������
                    else
                    {
                        
                        // ���� �� ������� ���������� ���� �� ������, ��������� ���
                        Debug.Log("�� ������� ����������� ���� �� ������.");
                        onComplete?.Invoke();
                        yield break; // ��������� ���������� ����� � ������
                    }
                }
                else
                {
                    Debug.Log("������ �� ������� ��� �������: " + position);
                }

                // ��������� ���������� ����������� ����� ��������� � ��������� ������
                yield return new WaitForSeconds(1f);
            }

            // �������� onComplete ����� ���������� ���� ����������� �� ����
            onComplete?.Invoke();
        }

        // ����� ��� ������ ������� ������ �� �� �������
        private Cell GetCellAtPosition(Vector2Int position)
        {
            // ������� ��� ������� ������ �� �����
            Cell[] cells = FindObjectsOfType<Cell>();

            // �������� �� ������ ������ � ���������� �� ������� � �������
            foreach (Cell cell in cells)
            {
                if (cell.Position == position)
                {
                    return cell; // ���������� ������, ���� �� ������� ��������� � �������
                }
            }

            return null; // ���� ������ �� �������, ���������� null
        }

        public List<Vector2Int> CalculatePath(Vector2Int startPosition, Vector2Int targetPosition)
        {
            List<Vector2Int> path = new List<Vector2Int>();

            int dx = targetPosition.x - startPosition.x;
            int dy = targetPosition.y - startPosition.y;

            int steps = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));

            for (int i = 0; i <= steps; i++)
            {
                int x = Mathf.RoundToInt(Mathf.Lerp(startPosition.x, targetPosition.x, (float)i / steps));
                int y = Mathf.RoundToInt(Mathf.Lerp(startPosition.y, targetPosition.y, (float)i / steps));

                path.Add(new Vector2Int(x, y));
            }

            return path;
        }

    }
}
