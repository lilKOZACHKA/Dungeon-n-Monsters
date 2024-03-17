using System;
using UnityEngine;
using UnityEngine.UI;

public class EventWindow : MonoBehaviour
{
    public Text eventText;

    [SerializeField] private string[] newText;

    [SerializeField] private string[] choiceTextsArray1;
    [SerializeField] private string[] choiceTextsArray2;
    [SerializeField] private string[] choiceTextsArray3;

    private string[][] choiceTexts;


    public Button choiceButton1;
    public Button choiceButton2;
    public Button choiceButton3;

    private int index;

    private void Start()
    {
        choiceTexts = new string[][] { choiceTextsArray1, choiceTextsArray2, choiceTextsArray3 };

        index = UnityEngine.Random.Range(0, newText.Length);
        UpdateEventText(newText[index]); 

        UpdateChoiceButtonTexts(index);

        choiceButton1.onClick.AddListener(() => Choice(0));
        choiceButton2.onClick.AddListener(() => Choice(1));
        choiceButton3.onClick.AddListener(() => Choice(2));
    }

    public void UpdateEventText(string newText)
    {
        eventText.text = newText;
    }

    private void UpdateChoiceButtonTexts(int index)
    {
        choiceButton1.GetComponentInChildren<Text>().text = choiceTexts[index][0];
        choiceButton2.GetComponentInChildren<Text>().text = choiceTexts[index][1];
        choiceButton3.GetComponentInChildren<Text>().text = choiceTexts[index][2];
    }

    private void Choice(int choiceIndex)
    {
        string chosenText = choiceTexts[index][choiceIndex];
        Debug.Log("Выбран вариант " + (choiceIndex + 1) + ": " + chosenText);
        UpdateEventText(chosenText);
    }

}
