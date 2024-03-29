using System.Collections.Generic;
using Scripts.CellLogic;
using Scripts.Factories;
using UnityEngine;

namespace Scripts.GameBoardLogic
{
    public class GameBoardGrid : MonoBehaviour
    {
        [Header("Map Generation Parameters")]
        [SerializeField] private int _mapWidth = 100;
        [SerializeField] private int _mapHeight = 100;
        [SerializeField] private int _numRooms = 5;
        [SerializeField] private int _minRoomSize = 5;
        [SerializeField] private int _maxRoomSize = 10;
        [SerializeField] private int _numTraps = 10;

        [SerializeField] private int _minTrapDamage = 1;
        [SerializeField] private int _maxTrapDamage = 5;

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
            gameMap.PlaceTraps(_numTraps);

            _map = gameMap.ToStringArray();
        }

        
    }
}
