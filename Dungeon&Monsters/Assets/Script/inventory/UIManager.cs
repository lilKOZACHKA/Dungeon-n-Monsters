using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button[]  actionButtons;

    private KeyCode action1, action2, action3;


    void Start() {
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    void Update() { 
        if (Input.GetKeyDown(action1)){ actionButtonsOnClick(0);}
        if (Input.GetKeyDown(action2)){ actionButtonsOnClick(1);}
        if (Input.GetKeyDown(action3)){ actionButtonsOnClick(2);}
    }

public void actionButtonsOnClick (int btnIndex)
{actionButtons[btnIndex].onClick.Invoke(); }
}
