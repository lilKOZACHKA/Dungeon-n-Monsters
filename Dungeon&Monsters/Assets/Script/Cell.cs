using System;
using UnityEngine;

namespace Scripts.CellLogic
{

public class Cell : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Vector2Int _position;

        [Header("Components")]
        [SerializeField] private GameObject _gameObject;

        public GameObject GameObject => _gameObject;

        public void Initialize(Vector2Int position)
        {
            _position = position;

            _gameObject.name = $"X: {position.x}, Y: {position.y}";
        }

        internal void Add(Cell cell)
        {
            throw new NotImplementedException();
        }

        internal void AddComponent(Cell cell)
        {
            throw new NotImplementedException();
        }
    }
}
