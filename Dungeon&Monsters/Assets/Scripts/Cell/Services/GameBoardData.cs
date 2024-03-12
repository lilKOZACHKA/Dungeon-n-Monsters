using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.StateMachines;
using Scripts.UnitLogic;
using UnityEngine;

namespace Scripts.CellLogic
{
    public class GameBoardData : MonoBehaviour
    {
        private Cell[] _cells;

        private List<Cell> _movingCells = new();
        private Unit _unit; 

        private Cell _selectCell;
        private bool _haveSelectedCell;

        private void Awake()
        {
            Cell[] cells = FindObjectsOfType<Cell>();

            if(cells.Length == 0) throw new NullReferenceException("cells not found.");

            _cells = cells;

            foreach (Cell cell in _cells)
            {
                cell.StateChanged += TrySetSelected;
                cell.StateChanged += TryRenderMoves;

                cell.ClickMoving += Move;
            }
        }

        private void OnDisable()
        {
            foreach(Cell cell in _cells)
            {
                cell.StateChanged -= TrySetSelected;
                cell.StateChanged -= TryRenderMoves;

                cell.ClickMoving -= Move;
            }
        }

        private void TrySetSelected(IState state, IState oldState, Cell cell)
        {
            if(state is SelectState && oldState is DefaultState)
            {
                if(_haveSelectedCell)
                {
                    _selectCell.Unselect();
                    ClearMoving();
                }

                _selectCell = cell;

                _haveSelectedCell = true;
            }
            if(state is DefaultState && oldState is SelectState)
            {
                _selectCell.Unselect();
                ClearMoving();

                _selectCell = null;
                _haveSelectedCell = false;
            }
        }
        private void TryRenderMoves(IState state, IState oldState, Cell cell)
        {
            if(state is SelectState && oldState is DefaultState && cell.HaveUnit)
            {
                _movingCells = SetMovingCells(cell.Position, cell.Unit.Moves, cell.Unit.AttackMoves);

                _unit = cell.Unit;
            }
        }

        private List<Cell> SetMovingCells(Vector2Int unitPosition, Vector2Int[] moves, Vector2Int[] attackMoves)
        {
            List<Cell> cells = new();

            foreach (Vector2Int move in moves)
            {
                Cell movingCell = FindCell(unitPosition, move, false);

                if(movingCell != null)
                {
                    movingCell.SetMoving();

                    cells.Add(movingCell);
                }
            }

            foreach (Vector2Int move in attackMoves)
            {
                Cell attackCell = FindCell(unitPosition, move, true);

                if(attackCell != null)
                {
                    var movingCell = cells.FirstOrDefault(cell => attackCell == cell);

                    if(movingCell != null)
                    {
                        movingCell.SetAttack();
                    }
                    else
                    {
                        attackCell.SetAttack();

                        cells.Add(attackCell);
                    }
                }
            }
            return cells;
        }

        private void Move(Cell cell)
        {
            _unit.Initialize(cell);

            _selectCell.Unselect();
        }

        private void ClearMoving()
        {
            _movingCells.ForEach(cell => cell.SetDefault());
            _movingCells.Clear();
        }

        private Cell FindCell(Vector2Int unitPosition, Vector2Int move, bool HaveUnit)
        {
            return _cells.FirstOrDefault(cell => cell.Position == move + unitPosition && cell.HaveUnit == HaveUnit);
        }
    }
}