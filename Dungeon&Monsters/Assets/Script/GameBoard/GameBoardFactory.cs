using System;
using System.Collections.Generic;
using Scripts.CellLogic;
using UnityEngine;
using Zenject;

namespace Scripts.Factories
{
    public class GameBoardFactory : IFactory<string[], Cell, float, Transform, List<Cell>>
    {
        // Обьекты на карте
        private const char Wall = '#';
        private const char Door = 'D';
        private const char Interior = '0';
        private const char Trap = 'T';

        public List<Cell> Create(string[] map, Cell prefab, float spacing, Transform root)
        {
            string[] expandedMap = ExpandMap(map);
            List<Cell> cells = new List<Cell>();
            CreateCellsFromMap(expandedMap, cells, prefab, spacing, root);
            CreateOutlineCells(expandedMap, cells, prefab, spacing, root);
            return cells;
        }

        private void CreateCellsFromMap(string[] map, List<Cell> cells, Cell prefab, float spacing, Transform root)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (IsCellSymbol(map[y][x]))
                    {
                        Cell cell = InstantiateCellAt(prefab, x, y, spacing, root);
                        if (map[y][x] == Trap)
                        {
                            SetCellColor(cell, Color.red);
                        }
                        cells.Add(cell);
                    }
                }
            }
        }

        private void CreateOutlineCells(string[] map, List<Cell> cells, Cell prefab, float spacing, Transform root)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (ShouldPlaceOutline(map, x, y))
                    {
                        Cell outlineCell = InstantiateCellAt(prefab, x, y, spacing, root, isWalkable: false);
                        SetCellColor(outlineCell, Color.gray);
                        cells.Add(outlineCell);
                    }
                }
            }
        }

        private Cell InstantiateCellAt(Cell prefab, int x, int y, float spacing, Transform root, bool isWalkable = true)
        {
            Vector3 position = new Vector3(x * spacing, -y * spacing, 0);
            Cell cell = UnityEngine.Object.Instantiate(prefab, position + root.position, Quaternion.identity, root);
            cell.Initialize(new Vector2Int(x, y));
            cell.IsWalkable = isWalkable;
            return cell;
        }

        private void SetCellColor(Cell cell, Color color)
        {
            CellView cellView = cell.GetComponent<CellView>();
            if (cellView != null)
            {
                cellView.SetColor(color);
            }
        }

        private bool IsCellSymbol(char symbol)
        {
            return symbol == Wall || symbol == Door || symbol == Interior || symbol == Trap;
        }

        private bool ShouldPlaceOutline(string[] map, int x, int y)
        {
            if (x < 0 || y < 0 || x >= map[0].Length || y >= map.Length) return false;
            if (IsCellSymbol(map[y][x])) return false;

            return HasAdjacentCell(map, x, y);
        }

        private bool HasAdjacentCell(string[] map, int x, int y)
        {
            for (int offsetY = -1; offsetY <= 1; offsetY++)
            {
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    if (offsetX == 0 && offsetY == 0) continue;
                    int checkX = x + offsetX;
                    int checkY = y + offsetY;

                    if (checkX >= 0 && checkY >= 0 && checkX < map[0].Length && checkY < map.Length && IsCellSymbol(map[checkY][checkX]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string[] ExpandMap(string[] originalMap)
        {
            int originalHeight = originalMap.Length;
            int originalWidth = originalMap[0].Length;
            string[] expandedMap = new string[originalHeight + 2];

            string borderRow = new string(' ', originalWidth + 2);
            expandedMap[0] = borderRow;
            expandedMap[expandedMap.Length - 1] = borderRow;

            for (int y = 0; y < originalHeight; y++)
            {
                expandedMap[y + 1] = " " + originalMap[y] + " ";
            }

            return expandedMap;
        }
    }
}