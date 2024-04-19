using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum ArmorType{helmet, chestplate, Sword, Shield, Boots, Twohand}
[CreateAssetMenu (fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
[SerializeField]
    private ArmorType armorType;
    [SerializeField]
    private int strength; // сила
[SerializeField]
    private int Dexterity; // ловкость
[SerializeField]
    private int Constitution; // телосложение
[SerializeField]
    private int Intelligence; // интелект
[SerializeField]
    private int Wisdom; // мудрость
     [SerializeField]
    private int Charisma; // харизма

   internal ArmorType MyArmorType { get { return armorType;}}

}
