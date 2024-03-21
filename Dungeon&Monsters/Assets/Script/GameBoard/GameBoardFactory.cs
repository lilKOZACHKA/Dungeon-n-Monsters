using System;
using System.Collections.Generic;
using Scripts.CellLogic;
using UnityEngine;
using Zenject;

namespace Scripts.Factories
{
    public class GameBoardFactory : IFactory<string[], Cell, float, Transform, List<Cell>>
    {
        public List<Cell> Create(string[] map, Cell prefab, float spacing, Transform root)
        {
            List<Cell> cells = new();

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    Vector3 position = new(x * spacing, y * spacing);

                    if (map[y][x] == '0')
                    {

                        Cell cell = UnityEngine.Object.Instantiate(prefab, position + root.position, Quaternion.identity, root);
                        cell.Initialize(new Vector2Int(x, y));
                        cells.Add(cell);
                    }
                    else if (map[y][x] != '.')
                    {
                        Cell cell = UnityEngine.Object.Instantiate(prefab, position + root.position, Quaternion.identity, root);
                        cell.Initialize(new Vector2Int(x, y));
                        cells.Add(cell);
                    }
                    else
                    {

                        Cell existingCell = cells.Find(c => c.transform.position == position + root.position);
                        if (existingCell != null)
                        {
                            cells.Remove(existingCell);
                            UnityEngine.Object.Destroy(existingCell.gameObject);
                        }
                    }
                }
            }

            return cells;
        }
    }
}
