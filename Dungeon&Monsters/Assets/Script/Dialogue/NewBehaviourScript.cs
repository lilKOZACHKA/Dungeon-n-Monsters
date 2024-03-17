using UnityEngine;
using UnityEngine.UI;

public class EventWindow : MonoBehaviour
{
    public Text eventText;

    [SerializeField]
    private string newText; // Поле для нового текста события, которое можно будет изменять в редакторе Unity

    public Button choiceButton1;
    public Button choiceButton2;

    public Button choiceButton3;
    private void Start()
    {
        // Начальная настройка окна события
        UpdateEventText(newText); // Используем newText при инициализации
        choiceButton1.onClick.AddListener(Choice1);
        choiceButton2.onClick.AddListener(Choice2);
        choiceButton3.onClick.AddListener(Choice3);
    }

    // Метод для обновления текста события
    public void UpdateEventText(string newText)
    {
        eventText.text = newText;
    }

    // Методы для обработки выбора
    private void Choice1()
    {
        Debug.Log("Выбран вариант 1");
        UpdateEventText("Вы звали на помощь, но никто не пришёл... Вас любили");
    }

    private void Choice2()
    {
        Debug.Log("Выбран вариант 2");
        UpdateEventText("Вы дрались достойно, но их число превышало раза в 3... Вас любили ");
    }

    private void Choice3()
    {
        Debug.Log("Выбран вариант 3");
         UpdateEventText("Вы решили любить их в ответ... Такого даже волки не ожидали");
    }
}
