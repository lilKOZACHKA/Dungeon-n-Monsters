using UnityEngine;

public class CreatePrefabInCanvas : MonoBehaviour
{
    public GameObject prefabToCreate; // Префаб, который вы хотите создать
    public Canvas canvas; // Ссылка на объект Canvas
    public Vector2 positionInCanvas; // Желаемые координаты внутри Canvas

    void Start()
    {
        // Проверяем, что префаб и объект Canvas установлены
        if (prefabToCreate != null && canvas != null)
        {
            // Создаем экземпляр префаба
            GameObject newObject = Instantiate(prefabToCreate);
            // Устанавливаем родителя для созданного объекта в Canvas
            newObject.transform.SetParent(canvas.transform, false);
            // Устанавливаем позицию созданного объекта в Canvas
            RectTransform rt = newObject.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = positionInCanvas;
            }
            
        }
        
    }
}
