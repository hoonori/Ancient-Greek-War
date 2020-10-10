using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject gameManager;

    int currGridX, currGridY;

    string majorButton, minorButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AbilityButton(string input)
    {
        majorButton = "AbilityButton";
        minorButton = input;

        TurnOnMouseInput();
    }

    public void BuildButton(string input)
    {
        majorButton = "BuildButton";
        minorButton = input;

        TurnOnMouseInput();
    }

    public void ExtraButton(string input)
    {

    }

    public void GetMouseInput(string input)
    {
        string[] inputs = input.Split(',');
        currGridX = System.Convert.ToInt32(inputs[0]);
        currGridY = System.Convert.ToInt32(inputs[1]);

        switch (majorButton)
        {
            case "AbilityButton":
                break;
            case "BuildButton":
                gameManager.SendMessage("ButtonClicked", majorButton + "," + minorButton + "," + currGridX.ToString() + "," + currGridY.ToString());
                break;
        }
        
        TurnOffMouseInput();
    }

    public GameObject mouseManager;
    public GameObject buttonManager;
    public GameObject statusManager;

    public void TurnOnMouseInput()
    {
        mouseManager.SendMessage("TurnOn");
    }

    public void TurnOffMouseInput()
    {
        mouseManager.SendMessage("TurnOff");
    }

    public void TurnOnButtonInput(string input)
    {
        buttonManager.SendMessage("TurnOn", input);
    }
    
    public void TurnOffButtonInput(string input)
    {
        buttonManager.SendMessage("TurnOff", input);
    }

    public void SetTurn(string input)
    {
        statusManager.SendMessage("SetTurn", input);
    }
}
