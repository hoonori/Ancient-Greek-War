using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject InfantryPrefab;
    public GameObject MusketeerPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Input should be "x,y,index,type" where x and y being grid and type being unit type.
    public void CreateUnit(string input)
    {
        //Debug.Log("CreateUnit : " + input);
        string[] inputs = input.Split(',');
        int gridX = System.Convert.ToInt32(inputs[0]);
        int gridY = System.Convert.ToInt32(inputs[1]);
        int gridIndex = System.Convert.ToInt32(inputs[2]);
        string unitType = inputs[3];
        int property = System.Convert.ToInt32(inputs[4]);

        int xOffset = 0;
        int yOffset = 0;
        switch(gridIndex)
        {
            case 0:
                xOffset = 1;
                yOffset = 1;
                break;
            case 1:
                xOffset = 3;
                yOffset = 1;
                break;
            case 2:
                xOffset = 1;
                yOffset = 3;
                break;
            case 3:
                xOffset = 3;
                yOffset = 3;
                break;
            case 4:
                xOffset = 0;
                yOffset = 3;
                break;
            case 5:
                xOffset = 0;
                yOffset = 1;
                break;
            case 6:
                xOffset = 1;
                yOffset = 0;
                break;
            case 7:
                xOffset = 3;
                yOffset = 0;
                break;
        }

        float targetX = 2.5f * (float)gridX + 0.625f * xOffset;
        float targetY = 2.5f * (float)gridY + 0.625f * yOffset;

        GameObject newObject;

        switch(unitType)
        {
            case "Infantry":
                {
                    newObject = Instantiate(InfantryPrefab, new Vector3(targetX, targetY, -1), Quaternion.identity);
                    break;
                }
            case "Musketeer":
                {
                    newObject = Instantiate(MusketeerPrefab, new Vector3(targetX, targetY, -1), Quaternion.identity);
                    break;
                }
            default:
                {
                    newObject = Instantiate(InfantryPrefab, new Vector3(targetX, targetY, -1), Quaternion.identity);
                    break;
                }
        }

        switch(property)
        {
            case 1:
                {
                    newObject.GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
            case 2:
                {
                    newObject.GetComponent<Renderer>().material.color = Color.blue;
                    break;
                }
            case 3:
                {
                    newObject.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                }
            case 4:
                {
                    newObject.GetComponent<Renderer>().material.color = Color.green;
                    break;
                }
            case 5:
                {
                    newObject.GetComponent<Renderer>().material.color = Color.white;
                    break;
                }
            case 6:
                {
                    newObject.GetComponent<Renderer>().material.color = Color.black;
                    break;
                }
        }
    }

    // Input should be two "x,y,index" where grid to find unit.
    GameObject FindUnit(int x, int y, int index)
    {
        float xOffset = 0.0f;
        float yOffset = 0.0f;
        switch (index)
        {
            case 0:
                xOffset = 1.0f;
                yOffset = 1.0f;
                break;
            case 1:
                xOffset = 3.0f;
                yOffset = 1.0f;
                break;
            case 2:
                xOffset = 1.0f;
                yOffset = 3.0f;
                break;
            case 3:
                xOffset = 3.0f;
                yOffset = 3.0f;
                break;
            case 4:
                xOffset = 0.0f;
                yOffset = 3.0f;
                break;
            case 5:
                xOffset = 0.0f;
                yOffset = 1.0f;
                break;
            case 6:
                xOffset = 1.0f;
                yOffset = 0.0f;
                break;
            case 7:
                xOffset = 3.0f;
                yOffset = 0.0f;
                break;
        }

        float targetX = 2.5f * (float)x + xOffset * 0.625f;
        float targetY = 2.5f * (float)y + yOffset * 0.625f;

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        for (int i = 0; i < units.Length; i++)
        {
            GameObject currUnit = units[i];

            Vector3 currPosition = currUnit.transform.position;

            if ((targetX - currPosition[0]) > -0.0625f && (targetX - currPosition[0]) < 0.0625f
                && (targetY - currPosition[1]) > -0.0625f && (targetY - currPosition[1]) < 0.0625f)
                return currUnit;
        }

        return null;
    }

    // Input should be two "x,y,index" where curr grid and next grid.
    public void MoveUnit(string input)
    {
        //Debug.Log("MoveUnit : " + input);
        string[] inputs = input.Split(',');
        int currGridX = System.Convert.ToInt32(inputs[0]);
        int currGridY = System.Convert.ToInt32(inputs[1]);
        int currGridIndex = System.Convert.ToInt32(inputs[2]);
        string nextGridX = (inputs[3]);
        string nextGridY = (inputs[4]);
        string nextGridIndex = (inputs[5]);

        GameObject currObject = FindUnit(currGridX, currGridY, currGridIndex);
        if (currObject == null)
        {
            Debug.Log("MoveUnit: " + "Cannot find object");
            return;
        }

        currObject.SendMessage("Move", nextGridX + "," + nextGridY + "," + nextGridIndex);
    }

    // Input should be two "x,y,index" where curr grid and next grid.
    public void SetUnitHealth(string input)
    {
        //Debug.Log("SetUnitHealth : " + input);
        string[] inputs = input.Split(',');
        int currGridX = System.Convert.ToInt32(inputs[0]);
        int currGridY = System.Convert.ToInt32(inputs[1]);
        int currGridIndex = System.Convert.ToInt32(inputs[2]);
        string currHealth = (inputs[3]);

        GameObject currObject = FindUnit(currGridX, currGridY, currGridIndex);
        if (currObject == null)
        {
            Debug.Log("SetUnitHealth: " + "Cannot find object");
            return;
        }

        currObject.SendMessage("SetHealth", currHealth);
    }

    // Input should be "x,y,index" where curr grid and next grid.
    public void DestroyUnit(string input)
    {
        //Debug.Log("DestroyUnit : " + input);
        string[] inputs = input.Split(',');
        int currGridX = System.Convert.ToInt32(inputs[0]);
        int currGridY = System.Convert.ToInt32(inputs[1]);
        int currGridIndex = System.Convert.ToInt32(inputs[2]);

        GameObject currObject = FindUnit(currGridX, currGridY, currGridIndex);
        if (currObject == null)
        {
            Debug.Log("DestroyUnit: " + "Cannot find object");
            return;
        }
        
        Destroy(currObject, 0.0f);
    }
}
