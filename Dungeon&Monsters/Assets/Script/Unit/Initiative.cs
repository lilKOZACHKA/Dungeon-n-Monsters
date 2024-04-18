using System.Collections.Generic; 
using System.Linq;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class Initiative : MonoBehaviour
    {
        public List<Unit> units;
        private List<Unit> _units = new List<Unit>();
        private Queue<Unit> turnQueue;
        private Unit currentUnit;

        public void findEnemyInArea(BoxCollider2D boxCollider, float detectionRange)
        {
           Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.transform.position, new Vector2(boxCollider.size.x, boxCollider.size.y) * detectionRange, 0);
            
            units.Clear();

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Unit"))
                {
                   // Debug.Log("������ ����: " + collider.gameObject.name);

                    Unit unit = collider.gameObject.GetComponent<Unit>();

                    if (unit != null && unit.gameObject.GetComponent<Unit>())
                    {
                        _units.Add(unit);
                    }
                }
            }
            GameObject gameManager = GameObject.Find("GameMananger");
            if (gameManager != null) 
            {
                foreach (Unit unit in _units)
                {
                    gameManager.gameObject.GetComponent<Initiative>().units.Add(unit);
                }
            }
        }

        public void intiativeRoll()
        {
            GameObject gameManager = GameObject.Find("GameMananger");
            _units = gameManager.GetComponent<Initiative>().units.ToList();

            foreach (Unit unit in _units)
            {
                unit._initiative = Random.Range(1, 20);
                unit._isActive = false;
                unit._isCombat = true;
            }

            _units = gameManager.GetComponent<Initiative>().units.OrderByDescending(unit => unit._initiative).ToList();

            gameManager.GetComponent<Initiative>().units.Clear();

            gameManager.GetComponent<Initiative>().units = _units.ToList();

            units = gameManager.GetComponent<Initiative>().units.ToList();

            turnQueue = new Queue<Unit>(units);

            StartNextTurn();
        }

        public void StartNextTurn()
        {
            if (turnQueue == null)
            {
                GameObject gameManager = GameObject.Find("GameMananger");
                turnQueue = gameManager.GetComponent<Initiative>().turnQueue;

            }
            if (turnQueue != null && turnQueue.Count > 0)
            {
                currentUnit = turnQueue.Dequeue();

                currentUnit._isActive = true;

                CombatCamera combatCamera = new CombatCamera();
                combatCamera.SetupCamera(currentUnit);

                if (currentUnit._isUnion)
                {
                    currentUnit.DoTurn(currentUnit);                
                }
                else
                {
                    Vector2Int position = currentUnit.GetCellPosition(); // �������� ������� �������� �����
                    currentUnit.BotTurn(currentUnit, currentUnit.GetTransform(), position, units);
                }
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
                unit._isActive = false;
                unit._moveCount = 0; // ������������ �������� 0 ���������� MoveCount
            }

            StartNextTurn();
        }

    }
}

