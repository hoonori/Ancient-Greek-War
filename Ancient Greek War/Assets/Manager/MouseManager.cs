using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject userInputManager;

    private bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int[] currGrid = ConvertCoordianteToGrid(position.x, position.y);

            userInputManager.SendMessage("getMouseInput", currGrid[0].ToString() + "," + currGrid[1].ToString());
        }        
    }

    public int[] ConvertCoordianteToGrid(float x, float y)
    {
        int gridX = (int)(x / 2.5f);
        int gridY = (int)(y / 2.5f);

        int[] grid = new int[2];
        grid[0] = gridX;
        grid[1] = gridY;

        return grid;
    }

    public void TurnOn()
    {
        isActive = true;
    }

    public void TurnOff()
    {
        isActive = false;
    }
}
