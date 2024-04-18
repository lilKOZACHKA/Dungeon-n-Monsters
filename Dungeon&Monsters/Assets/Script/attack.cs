using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.GameBoardLogic;
using Scripts.StateMachines;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.CellLogic
{
    public class ShowAttackCells : MonoBehaviour
    {
        [SerializeField] private GameBoardGrid _gameBoardGrid; 
        [SerializeField] private Button _button; 

        private bool _isAttackShown = false; 

        private void Start()
        {
            // Подписываемся на событие нажатия на кнопку
            _button.onClick.AddListener(new UnityEngine.Events.UnityAction(ShowOrHideAttack));
        }

        private void ShowOrHideAttack()
        {
            // Если атака уже отображена, то скрываем ее
            if (_isAttackShown)
            {
                HideAttack();
            }
            // Иначе отображаем атаку
            else
            {
                ShowAttack();
            }

            // Инвертируем флаг состояния отображения атаки
            _isAttackShown = !_isAttackShown;
        }

        private void ShowAttack()
        {
            if (_gameBoardGrid != null)
            {
                // Получаем все ячейки на игровом поле
                List<Cell> cells = _gameBoardGrid.GetAllCells();

                // Показываем клетки атаки для каждой ячейки с юнитом
                foreach (var cell in cells.Where(cell => cell.HaveUnit && cell.Unit != null))
                {
                    // Получаем доступ к атакующим ходам юнита
                    Vector2Int[] attackMoves = cell.Unit.AttackMoves;

                    // Показываем клетки атаки
                    ShowAttack(cell.Position, attackMoves);
                }
            }
            else
            {
                Debug.LogWarning("Game board grid reference is missing.");
            }
        }

        private void HideAttack()
        {
            // Получаем все ячейки на игровом поле
            List<Cell> cells = _gameBoardGrid.GetAllCells();

            // Снимаем атаку для каждой ячейки, которая ее показывала
            foreach (var cell in cells)
            {
                if (cell.CurrentState is AttackState)
                {
                    cell.SetDefault();
                }
            }
        }

        private void ShowAttack(Vector2Int unitPosition, Vector2Int[] attackMoves)
        {
            foreach (var move in attackMoves)
            {
                Cell attackCell = FindCell(unitPosition, move, true);

                if (attackCell != null)
                {
                    attackCell.SetAttack();
                }
            }
        }

        private Cell FindCell(Vector2Int unitPosition, Vector2Int move, bool HaveUnit)
        {
            return _gameBoardGrid.GetAllCells().FirstOrDefault(cell => cell.Position == move + unitPosition && cell.HaveUnit == HaveUnit);
        }
    }
}
