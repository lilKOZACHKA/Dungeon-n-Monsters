using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class CombatCamera : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 2, -10);
        public float smoothTime = 0.25f;
        private Vector3 currentVelocity;

        public GameObject[] units;
        private GameObject selectedUnit;

        public void Start()
        {
            GameObject[] _units = GameObject.FindGameObjectsWithTag("Unit");

            units = _units;
        }

        private void LateUpdate()
        {
            if (selectedUnit != null)
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    selectedUnit.transform.position + offset,
                    ref currentVelocity,
                    smoothTime
                );
            }
        }

        public void SelectUnit(int unitIndex)
        {
            if (unitIndex >= 0 && unitIndex < units.Length)
            {
                selectedUnit = units[unitIndex];
            }
        }
    }
}