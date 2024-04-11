using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class CombatCamera : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 0, -5);
        public float smoothTime = 0.25f;
        private Vector3 currentVelocity;

        public GameObject selectedUnit;

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

        public void SelectUnit(int unit)
        {
            selectedUnit = unit;
        }
    }
}