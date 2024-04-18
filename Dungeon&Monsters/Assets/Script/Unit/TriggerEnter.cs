using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class TriggerEnter : MonoBehaviour
    {
        private BoxCollider2D boxCollider;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
    }
}