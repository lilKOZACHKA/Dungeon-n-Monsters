using System.Collections.Generic;
using Scripts.CellLogic;
using Scripts.Factories;
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

        private void GenerateMap()
        {
            Map gameMap = new Map(_mapWidth, _mapHeight);
            gameMap.GenerateConnectedRooms(_numRooms, _minRoomSize, _maxRoomSize, 1);
            gameMap.PlaceDoors(1);

            _map = gameMap.ToStringArray();
        }
    }
}
