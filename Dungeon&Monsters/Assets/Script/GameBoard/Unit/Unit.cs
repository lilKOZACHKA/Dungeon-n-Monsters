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

        [Space]

        [SerializeField] private int _id;
        [SerializeField] private int _health;
        [SerializeField] private int _initiative;
        [SerializeField] public int _moveCount = 0;
        [SerializeField] private int _moveCountMax = 5;

        [SerializeField] private bool _isCombat = false;
        [SerializeField] private bool _isUnion = false;
        [SerializeField] private bool _isActive = false;

        [SerializeField] private Cell _cell;

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


        //public Cell Cell;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int Initiative
        {
            get { return _initiative; }
            set { _initiative = value; }
        }

        public bool IsCombat
        {
            get { return _isCombat; }
            set { _isCombat = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool IsUnion
        {
            get { return _isUnion; }
            set { _isUnion = value; }
        }

        public int MoveCount
        {
            get { return _moveCount; }
            set { _moveCount = value; }
        }

        public int MoveCountMax
        {
            get { return _moveCountMax; }
            set { _moveCountMax = value; }
        }

        // В классе Unit
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

        public Unit(int initialHealth, bool isCombat, bool isUnion, bool isActive, int initiative, int id, int moveCountMax, int moveCount)
        {
            _health = initialHealth;
            _initiative = initiative;
            _isCombat = isCombat;
            _isUnion = isUnion;
            _id = id;
            _isActive = isActive;
            _moveCount = moveCount;
            _moveCountMax = moveCountMax;
        }

        private void OnDrawGizmosSelected()
        {
            if (_moves == null || _attackMoves == null) return;

            foreach (Vector2 move in _moves)
            {
                Vector2 position = move * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

                if (_attackMoves.Any(attackMove => attackMove == move))
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

                if (_moves.Any(move => move == attackMove)) continue;

                Gizmos.DrawCube(position, _transform.localScale);
            }
        }

        public void Initialize(Cell cell)
        {
            if ((cell.SetUnit(this)) == 0)
            {
                _cell.SetUnit();
                _transform.position = cell.Transform.position;
                _cell = cell;
            }
        }

        public void FindCell()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_transform.position, _transform.localScale, 0);

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
            CombatCamera camera = GameObject.Find("CombatCamera").GetComponent<CombatCamera>();
            //          camera.SelectUnit(currentUnit);

            Debug.Log("Ход персонажа c инициативой - " + currentUnit.Initiative);
        }

        public Transform GetTransform()
        {
            return _transform;
        }

        public void BotTurn(Unit currentUnit, Transform transform, Vector2Int position, List<Unit> units)
        {
            CombatCamera camera = GameObject.Find("CombatCamera").GetComponent<CombatCamera>();
            Debug.Log("Ход бота c инициативой - " + currentUnit.Initiative);

            if (currentUnit.IsCombat)
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
        }







        // Метод для поиска союзного юнита (IsUnion == true) для текущего юнита
        private Unit FindUnionUnit(Unit currentUnit, List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                if (unit.IsUnion && unit != currentUnit) // Исключаем текущего юнита из поиска
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
                        targetCell.SetUnit();
                        _transform.position = targetCell.Transform.position;
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
