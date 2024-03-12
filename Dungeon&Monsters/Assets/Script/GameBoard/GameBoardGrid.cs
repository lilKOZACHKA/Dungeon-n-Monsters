using System.Collections.Generic;
using Scripts.CellLogic;
using Scripts.Factories;
using UnityEngine;

namespace Scripts.GameBoardLogic
{
    public class GameBoardGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private float _spacing;

        [Space]

        [SerializeField] private Cell _prefab;

        [Space]

        [SerializeField] private Transform _root;

        [Space]

        [SerializeField] private List<Cell> _cells;

        private readonly GameBoardFactory _factory = new();

        private void OnValidate() 
        {
            _size =new((int)Mathf.Clamp(_size.x, 0, Mathf.Infinity), (int)Mathf.Clamp(_size.y, 0, Mathf.Infinity));
            _spacing = Mathf.Clamp(_spacing, 0, Mathf.Infinity);

            if(_root == null)
            {
                _root = transform;
            }
        }
        [ContextMenu("Create")]
        public void Create()
        {
            Clear();

            _cells = _factory.Create(_prefab, _size, _spacing, _root);
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
    }
}