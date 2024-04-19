using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator anim;
    public int levelToLoad;

    public async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("fade");

            await Task.Delay(2500);

            SceneManager.LoadScene(levelToLoad);
        }
    }

    [SerializeField]
    public GameObject myPrefab;

    // void Start()
    // {
    //     // Получаем сохраненные данные из SaveData
    //     SaveData saveData = LoadSaveData();

    //     // Создаем экземпляр префаба
    //     GameObject newObject = Instantiate(myPrefab);
        
    //     // Если у префаба есть скрипт InventoryData
    //     InventoryData inventoryData = newObject.GetComponent<InventoryData>();
    //     if (inventoryData != null)
    //     {
    //         // Загружаем сохраненные данные инвентаря
    //         inventoryData.MyBags = saveData.MyInventoryData.MyBags;
    //         inventoryData.MyItems = saveData.MyInventoryData.MyItems;
            
    //         // Дополнительные операции с созданным объектом, если необходимо
    //     }
    

}
