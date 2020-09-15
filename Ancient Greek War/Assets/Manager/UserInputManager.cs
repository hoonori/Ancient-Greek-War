using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public GameObject gameManager;

    int currGridX, currGridY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getMouseInput(string input)
    {
        string[] inputs = input.Split(',');
        currGridX = System.Convert.ToInt32(inputs[0]);
        currGridY = System.Convert.ToInt32(inputs[1]);

        Debug.Log(string.Format("Mouse Input [X: {0} Y: {1}]", currGridX, currGridY));
    }

    public GameObject mouseManager;
    public GameObject buttonManager;

    public void turnOnMouseInput()
    {
        mouseManager.SendMessage("TurnOn");
    }

    public void turnOffMouseInput()
    {
        mouseManager.SendMessage("TurnOff");
    }

    public void turnOnButtonInput(string input)
    {
        buttonManager.SendMessage("TurnOn", input);
    }
    
    public void turnOffButtonInput(string input)
    {
        buttonManager.SendMessage("TurnOff", input);
    }
}
