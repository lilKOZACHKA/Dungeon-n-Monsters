using System.Collections.Generic;
using Scripts.CellLogic;
using Scripts.Factories;
using Scripts.UnitLogic;
using UnityEngine;

namespace Scripts.GameBoardLogic
{
    public class GameBoardGrid : MonoBehaviour
    {
        private string[] _map;

        [Header("Map Generation Parameters")]
        [SerializeField] private int _mapWidth = 100;
        [SerializeField] private int _mapHeight = 100;

        [Space]
        [SerializeField] private int _numRooms = 5;
        [SerializeField] private int _minRoomSize = 5;
        [SerializeField] private int _maxRoomSize = 10;
        
        [Space]
        // Если комнаты маленькие и много трапов то может юнити сломатся, тоже самое и с врагами
        [SerializeField] private int _numTraps = 3;
        [SerializeField] private int _minTrapDamage = 1;
        [SerializeField] private int _maxTrapDamage = 5;

        [Space]
        [SerializeField] private int _numEntities = 3; 

        [Space]
        [SerializeField] private float _spacing;
        [Space]
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Unit _enemyPrefab;
        [SerializeField] private Unit _heroPrefab;
        [Space]
        [SerializeField] private Transform _root;
        [Space]
        [SerializeField] private List<Cell> _cells;

        private GameBoardFactory _factory;

        [ContextMenu("Create")]
        public void Create()
        {
            Clear();
            GenerateMap();
            _factory = new GameBoardFactory();
            _factory.SetEnemyPrefab(_enemyPrefab);
            _factory.SetHeroPrefab(_heroPrefab);
            _cells = _factory.Create(_map, _cellPrefab, _spacing, _root);
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
            gameMap.PlaceEntities(_numEntities);

            _map = gameMap.ToStringArray();
        }
    }
}
