using System.Collections.Generic;
using Scripts.CellLogic;
using UnityEngine;


namespace Scripts.Factories
{

    public class GameBoardFactory : IFactory<Cell, Vector2Int, float, Transform, List<Cell>>
    {
        public List<Cell> Create(Cell prefab, Vector2Int size, float spacing, Transform root)
        {
            List<Cell> cells = new();

            for (int x = 0; x < size.x; x++)
            {

                for (int y = 0; y < size.y; y++)
                {
                    Vector3 position = new(x * spacing, y * spacing);

                    Cell cell = Object.Instantiate(prefab, position + root.position, Quaternion.identity, root);

                    cell.Initialize(new Vector2Int(x, y));

                    cells.Add(cell);
                }
            }

            return cells;
        }
    }

    public interface IFactory<T1, T2, T3, T4, T5>
    {
    }
}