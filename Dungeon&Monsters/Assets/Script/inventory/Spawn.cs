using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void SpawnDroppedItem()
    {
        // Создаем новый вектор с дробными координатами X и Y, используя текущую позицию игрока
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, 0f);
        
        // Прибавляем дробные числа к координатам X и Y позиции игрока
        playerPos += new Vector3(1.2f, 0.5f, 0f); // Например, добавляем 0.5 к каждой координате
        
        // Создаем объект item в новой позиции игрока
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
