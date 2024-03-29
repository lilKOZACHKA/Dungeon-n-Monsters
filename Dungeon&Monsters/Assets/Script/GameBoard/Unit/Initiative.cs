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

        private void Start()
        {
            foreach (Unit unit in units)
            {
                unit.Initiative = Random.Range(1, 20);
                unit.IsCombat = true;
                unit.gameObject.SetActive(false);
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

                currentUnit.gameObject.SetActive(true);

                currentUnit.DoTurn(currentUnit);

                return;
            }
            else
            {
                turnQueue = new Queue<Unit>(units);
                StartNextTurn();
            }
        }

        public void EndRound()
        {
            foreach (Unit unit in units)
            {
                unit.gameObject.SetActive(false);
            }
            StartNextTurn();
        }
    }
}

