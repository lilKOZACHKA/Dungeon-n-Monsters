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

            Debug.Log("Ход персонажа c инициативой - " + currentUnit._initiative);
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
                Debug.LogError("Ошибка: У юнита нет ссылки на клетку.");
                return Vector2Int.zero;
            }
        }

        public void BotTurn(Unit currentUnit, Transform transform, Vector2Int position, List<Unit> units)
        {
            Debug.Log("Ход бота c инициативой - " + currentUnit._initiative);

            if (currentUnit._isCombat)
            {
                try
                {
                        Unit targetUnit = FindUnionUnit(currentUnit, units);
                    if (targetUnit == null)
                    {
                        Debug.Log("Союзный юнит для бота не найден.");
                        return;
                    }

                    Vector2Int targetPosition = targetUnit.GetCellPosition();
                    Vector2Int startPosition = currentUnit.GetCellPosition();

                    List<Vector2Int> path = CalculatePath(startPosition, targetPosition);

                
                        StartCoroutine(MoveAlongPathWithUnit(path, currentUnit, startPosition, () =>
                        {
                            Initiative initiative = FindObjectOfType<Initiative>();
                            initiative.EndRound();

                            // Обновляем позицию бота после завершения перемещения
                            currentUnit.Initialize(currentUnit.GetCellAtPosition(position));
                        }));
                    }
                catch
                {
                    Initiative initiative = FindObjectOfType<Initiative>();
                    initiative.EndRound();

                    Debug.Log("Поломался");
                }
            }
        }

        // Метод для поиска союзного юнита (IsUnion == true) для текущего юнита
        private Unit FindUnionUnit(Unit currentUnit, List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                if (unit._isUnion && unit != currentUnit) // Исключаем текущего юнита из поиска
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
                // Пропускаем клетку, если она совпадает с начальной позицией бота
                if (position == initialPosition)
                {
                    continue;
                }

                // Получить объект клетки для текущей позиции
                Cell targetCell = GetCellAtPosition(position);

                // Проверить, что клетка найдена
                if (targetCell != null)
                {
                    // Установить юнит на клетку
                    if (targetCell.SetUnit(unit) == 0)
                    {
                        // Успешно установлен новый юнит
                        // Можно выполнить дополнительные действия
                        if (_cell != null && _cell != targetCell)
                        {
                            // Если текущая клетка не null и отличается от целевой клетки
                            _cell.SetUnit(); // Удаляем текущий юнит из текущей клетки
                        }

                        transform.position = targetCell.Transform.position;
                        _cell = targetCell;
                    }
                    // Проверить, успешно ли установлен юнит на клетку
                    else
                    {
                        
                        // Если не удалось установить юнит на клетку, завершить ход
                        Debug.Log("Не удалось переместить бота на клетку.");
                        onComplete?.Invoke();
                        yield break; // Прерываем выполнение цикла и метода
                    }
                }
                else
                {
                    Debug.Log("Клетка не найдена для позиции: " + position);
                }

                // Дождаться завершения перемещения перед переходом к следующей клетке
                yield return new WaitForSeconds(1f);
            }

            // Вызываем onComplete после завершения всех перемещений по пути
            onComplete?.Invoke();
        }

        // Метод для поиска объекта клетки по ее позиции
        private Cell GetCellAtPosition(Vector2Int position)
        {
            // Находим все объекты клеток на сцене
            Cell[] cells = FindObjectsOfType<Cell>();

            // Проходим по каждой клетке и сравниваем ее позицию с искомой
            foreach (Cell cell in cells)
            {
                if (cell.Position == position)
                {
                    return cell; // Возвращаем клетку, если ее позиция совпадает с искомой
                }
            }

            return null; // Если клетка не найдена, возвращаем null
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
