
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class charButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ArmorType armoryType;

    private Armor armor;
[SerializeField]
    private Image icon;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;
                if(tmp.MyArmorType == armoryType)
                {
                    EquipArmor(tmp);
                }
            }
        }
    }

    public void EquipArmor(Armor armor)
    {
        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        this.armor = armor;
        icon.color = Color.white;
        Debug.Log("EQUIP  " + armor);
        
    }
}
