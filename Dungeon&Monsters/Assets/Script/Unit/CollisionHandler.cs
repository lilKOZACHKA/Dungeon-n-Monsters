using UnityEngine;

namespace Scripts.UnitLogic
{
    public class CollisionHandler : Initiative
    {
        private float detectionRange = 1f;

        private BoxCollider2D boxCollider;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<BoxCollider2D>() != null)
            {
                if (other.GetComponent<BoxCollider2D>().CompareTag("Area"))
                {
                    boxCollider = other.GetComponent<BoxCollider2D>();

                    findEnemyInArea(boxCollider, detectionRange);

                    intiativeRoll();
                }
            }
        }
    }
}

