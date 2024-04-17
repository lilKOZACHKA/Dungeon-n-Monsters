using System.Collections.Generic;
using Scripts.UnitLogic; 
using System.Linq;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class Initiative : MonoBehaviour
    {
        public List<Unit> units;
        private Queue<Unit> turnQueue;
        private Unit currentUnit;
        private Vector2Int position;

        private void Start()
        {
            foreach (Unit unit in units)
            {
                unit.Initiative = Random.Range(1, 20);
                unit.IsActive = false;
                unit.IsCombat = true;
            }

            units = units.OrderByDescending(unit => unit.Initiative).ToList();

            turnQueue = new Queue<Unit>(units);

            StartNextTurn();
        }

        private void StartNextTurn()
        {
            if (turnQueue.Count > 0)
            {
                currentUnit = turnQueue.Dequeue();

                currentUnit.IsActive = true;

                if (currentUnit.IsUnion)
                {
                    currentUnit.DoTurn(currentUnit);
                }
                else
                {
                    Vector2Int position = currentUnit.GetCellPosition(); // ѕолучаем позицию текущего юнита
                    currentUnit.BotTurn(currentUnit, currentUnit.GetTransform(), position, units);
                }
            }
            else
            {
                // ѕосле завершени€ очереди, мы можем начать новый цикл, возвраща€ все единицы в очередь
                turnQueue = new Queue<Unit>(units);
                StartNextTurn();
            }
        }



        public void EndRound()
        {
            foreach (Unit unit in units)
            {
                unit.IsActive = false;
                unit.MoveCount = 0; // ѕрисваивание значени€ 0 переменной MoveCount
            }
            StartNextTurn();
        }

    }
}

