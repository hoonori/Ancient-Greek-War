using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject abilityButton;
    public GameObject buildButton;
    public GameObject extraButton;

    // Start is called before the first frame update
    void Start()
    {
        abilityButton = GameObject.Find("AbilityButton");
        buildButton = GameObject.Find("BuildButton");
        extraButton = GameObject.Find("ExtraButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TurnOn(string input)
    {
        switch (input)
        {
            case "Ability":
                abilityButton.GetComponent<Button>().interactable = true;
                break;
            case "Build":
                buildButton.GetComponent<Button>().interactable = true;
                break;
            case "Extra":
                extraButton.GetComponent<Button>().interactable = true;
                break;
        }
    }

    public void TurnOff(string input)
    {
        switch (input)
        {
            case "Ability":
                abilityButton.GetComponent<Button>().interactable = false;
                break;
            case "Build":
                buildButton.GetComponent<Button>().interactable = false;
                break;
            case "Extra":
                extraButton.GetComponent<Button>().interactable = false;
                break;
        }
    }
}
