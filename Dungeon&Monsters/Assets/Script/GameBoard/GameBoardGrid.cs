using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.CellLogic;
using Scripts.Factories;
using Scripts.UnitLogic;
using UnityEngine;

namespace Scripts.GameBoardLogic
{
    public class GameBoardGrid : MonoBehaviour
    {
        [Header("Map Generation Parameters")]
        [SerializeField] private int _mapWidth = 50;
        [SerializeField] private int _mapHeight = 100;
        [SerializeField] private int _numRooms = 10;
        [SerializeField] private int _minRoomSize = 10;
        [SerializeField] private int _maxRoomSize = 30;

        private string[] _map;

        [Space]
        [SerializeField] private float _spacing;
        [Space]
        [SerializeField] private Cell _prefab;
        [Space]
        [SerializeField] private Transform _root;
        [Space]
        [SerializeField] private List<Cell> _cells;

        private readonly GameBoardFactory _factory = new();

        public object SelectedUnit { get; internal set; }

        [ContextMenu("Create")]
        public void Create()
        {
            Clear();
            GenerateMap(); 
            _cells = _factory.Create(_map, _prefab, _spacing, _root);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            for (int i = 0; i < _cells.Count;)
            {
                DestroyImmediate(_cells[i].GameObject);
                _cells.RemoveAt(i);
            }
        }

        public Vector2Int GetUnitPosition(Unit unit)
        {
            // Ищем ячейку, содержащую данный юнит
            Cell cellWithUnit = _cells.FirstOrDefault(cell => cell.HaveUnit && cell.Unit == unit);

            // Если найдена ячейка с юнитом, возвращаем ее позицию
            if (cellWithUnit != null)
            {
                return cellWithUnit.Position;
            }
            else
            {
                Debug.LogWarning("Unit not found on any cell.");
                return Vector2Int.zero; // Возвращаем нулевую позицию или другое значение по умолчанию
            }
        }

        private void GenerateMap()
        {
            Map gameMap = new Map(_mapWidth, _mapHeight);
            gameMap.GenerateConnectedRooms(_numRooms, _minRoomSize, _maxRoomSize, 1);
            gameMap.PlaceDoors(1);

            _map = gameMap.ToStringArray();
        }

        public List<Cell> GetAllCells()
        {
            return _cells;
        }
        private Cell _selectedCell;

    // Метод для установки выделенной ячейки
    public void SetSelectedCell(Cell cell)
    {
        _selectedCell = cell;
    }

    // Метод для получения текущей выделенной ячейки
    public Cell GetSelectedCell()
    {
        return _selectedCell;
    }
    }
}
