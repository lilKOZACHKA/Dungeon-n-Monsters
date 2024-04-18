using UnityEngine;

namespace Scripts.UnitLogic
{
    public class CombatCamera : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 0, -3);
        public float smoothTime = 0.25f;
        private Vector3 currentVelocity;

        private Unit selectedUnit;

        public void Start()
        {
            Unit unit = GameObject.Find("Hero(Clone)").GetComponent<Unit>();

            selectedUnit = unit;
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

        public void SetupCamera(Unit unit)
        {
            GameObject combatCamera = GameObject.Find("CombatCamera");
            combatCamera.GetComponent<CombatCamera>().selectedUnit = unit;
        }
    }
}