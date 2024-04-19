using System.Linq;
using Scripts.CellLogic;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class Trap : Unit
    {
        public int Damage { get; private set; }

        public void Initialize(int damage)
        {
            Damage = damage;
        }

        // код для трапа
    }
}