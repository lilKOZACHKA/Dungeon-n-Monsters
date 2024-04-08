using System;
using System.Collections.Generic;
using Scripts.CellLogic;
using Scripts.UnitLogic;
using UnityEngine;
using Zenject;

namespace Scripts.Factories
{
    public class GameBoardFactory : IFactory<string[], Cell, float, Transform, List<Cell>>
    {
        private Unit _enemyPrefab;
        private Unit _heroPrefab;

        private const char Door = 'D';
        private const char Interior = '0';
        private const char Trap = 'T';
        private const char Enemy = 'E';  
        private const char Hero = 'H';   

        public void SetEnemyPrefab(Unit enemyPrefab)
        {
            _enemyPrefab = enemyPrefab;
        }

        public void SetHeroPrefab(Unit heroPrefab)
        {
            _heroPrefab = heroPrefab;
        }

        public List<Cell> Create(string[] map, Cell prefab, float spacing, Transform root)
        {
            string[] expandedMap = ExpandMap(map);
            List<Cell> cells = new List<Cell>();
            CreateCellsFromMap(expandedMap, cells, prefab, spacing, root);
            CreateOutlineCells(expandedMap, cells, prefab, spacing, root);
            return cells;
        }

        private void CreateCellsFromMap(string[] map, List<Cell> cells, Cell cellPrefab, float spacing, Transform root)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    char symbol = map[y][x];
                    if (IsCellSymbol(symbol))
                    {
                        Cell cell = InstantiateCellAt(cellPrefab, x, y, spacing, root);
                        if (symbol == Trap)
                        {
                            SetCellColor(cell, Color.red);
                        }
                        else if (symbol == Enemy)
                        {
                            InstantiateUnitAt(_enemyPrefab, cell);
                        }
                        else if (symbol == Hero)
                        {   
                            InstantiateUnitAt(_heroPrefab, cell);
                        }
                        cells.Add(cell);
                    }
                }
            }
        }

        private Unit InstantiateUnitAt(Unit unitPrefab, Cell cell)
        {
            Unit unit = UnityEngine.Object.Instantiate(unitPrefab, cell.Transform.position, Quaternion.identity, cell.Transform);
            unit.Initialize(cell); 
            return unit;
        }

        private bool IsCellSymbol(char symbol)
        {
            return symbol == Door || symbol == Interior || symbol == Trap || symbol == Enemy || symbol == Hero;
        }

        private void CreateOutlineCells(string[] map, List<Cell> cells, Cell cellPrefab, float spacing, Transform root)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (ShouldPlaceOutline(map, x, y))
                    {
                        Cell outlineCell = InstantiateCellAt(cellPrefab, x, y, spacing, root, isWalkable: false);
                        SetCellColor(outlineCell, Color.gray);
                        cells.Add(outlineCell);
                    }
                }
            }
        }

        private Cell InstantiateCellAt(Cell cellPrefab, int x, int y, float spacing, Transform root, bool isWalkable = true)
        {
            Vector3 position = new Vector3(x * spacing, -y * spacing, 0);
            Cell cell = UnityEngine.Object.Instantiate(cellPrefab, position + root.position, Quaternion.identity, root);
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