using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class SaveData
{

    public InventoryData MyInventoryData { get; set; }
   public List<BagData> MyBags {get; set;} 

   public List<ItemData> MyItems { get; set;}

   
   public SaveData()
   {
    MyInventoryData = new InventoryData();
   }
    
} 

public class InventoryData
{
    public List<BagData> MyBags { get; set; }
    public List<ItemData> MyItems { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }

    // Метод для сохранения данных предметов из слотов
    public void SaveInventory(List<Slot> slots)
    {
        foreach (Slot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                // Создаем объект данных предмета и сохраняем его
                ItemData itemData = new ItemData();
                // Присваиваем необходимые данные, например, количество предметов
                itemData.MyItemCount = slot.MyCount;
                // Добавляем в список предметов
                MyItems.Add(itemData);
            }
        }
    }


}
[Serializable]
public class BagData
{
    public int MySlotCount {get; set;}
}

public class ItemData
{
    public int MyItemCount {get; set;}
}




