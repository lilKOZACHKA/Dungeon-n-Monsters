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

        private List<(int startX, int startY, int length, int width)> _roomCoordinates;

        [Header("Map Generation Parameters")]
        [SerializeField] private int _mapWidth = 250;
        [SerializeField] private int _mapHeight = 250;

        [Space]
        [SerializeField] private int _numRooms = 10;
        [SerializeField] private int _minRoomSize = 10;
        [SerializeField] private int _maxRoomSize = 30;
        
        [Space]
        [SerializeField] private int _numTraps = 10;
        [SerializeField] private int _minTrapDamage = 1;
        [SerializeField] private int _maxTrapDamage = 5;

        [Space]
        [SerializeField] private int _numEntities = 10;

        [Space]
        [SerializeField] private int _numChests = 20;

        [Space]
        [SerializeField] private float _spacing;
        [Space]
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Unit _enemyPrefab;
        [SerializeField] private Unit _heroPrefab;
        [SerializeField] private Chest _chestPrefab; 
        [SerializeField] private Trap _trapPrefab; 

        [Space]
        [SerializeField] private Transform _root;
        [Space]
        [SerializeField] private List<Cell> _cells;

        private GameBoardFactory _factory;

        [ContextMenu("Create")]

        private void Awake()
        {
            Create();
        }
        public void Create()
        {
            Clear();
            GenerateMap();
            _factory = new GameBoardFactory();
            _factory.SetEnemyPrefab(_enemyPrefab);
            _factory.SetHeroPrefab(_heroPrefab);
            _factory.SetChestPrefab(_chestPrefab);
            _factory.SetTrapPrefab(_trapPrefab);
            _factory.SetRoomsCoordinate(_roomCoordinates);
            _cells = _factory.Create(_map, _cellPrefab, _spacing, _root);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            List<GameObject> roomColliders = new List<GameObject>();

            foreach (Transform child in _root)
            {
                if (child.name == "RoomCollider")
                {
                    roomColliders.Add(child.gameObject);
                }
            }

            foreach (GameObject collider in roomColliders)
            {
                DestroyImmediate(collider);
            }

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
            gameMap.PlaceChests(_numChests); 
            gameMap.PlaceTraps(_numTraps);
            gameMap.PlaceEntities(_numEntities);

            _roomCoordinates = gameMap.GetAllRoomCoordinates();

            _map = gameMap.ToStringArray();
        } 
    }
}
