using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    GameObject tileManager;
    GameObject unitManager;
    GameObject buildingManager;
    GameObject userInputManager;

    float timer = 0;
    int sequence = 0;
    bool[] isSequenceDone = new bool[10];

    int currX, currY, currIndex;
    string currType;

    // Start is called before the first frame update
    void Start()
    {
        tileManager = GameObject.Find("TileManager");
        unitManager = GameObject.Find("UnitManager");
        buildingManager = GameObject.Find("BuildingManager");
        userInputManager = GameObject.Find("UserInputManager");

        // currX = 3;
        // currY = 3;
        // currIndex = 0;
        // currType = "Infantry";

        // unitManager.SendMessage("CreateUnit", currX.ToString() + "," + currY.ToString() + "," + currIndex.ToString() + "," + currType);
        // buildingManager.SendMessage("CreateBuilding", "5,5,Temple");
        // unitManager.SendMessage("MoveUnit", "3,3,0,1,1,0");
        // tileManager.SendMessage("ChangeTile", "0,0,Water");
        // unitManager.SendMessage("DestroyUnit", "1,1,0");
        // buildingManager.SendMessage("DestroyBuilding", "0,6");
    }

    // Update is called once per frame
    void Update()
    {

        if (sequence == 0 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
            // buildingManager.SendMessage("CreateBuilding", "5,5,Temple,6");
        }
        else if (sequence == 1 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 2 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 3 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
            // buildingManager.SendMessage("DestroyBuilding", "5,5");
        }
        else if (sequence == 4 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 5 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 6 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 7 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 8 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else if (sequence == 9 && !isSequenceDone[sequence]) {
            isSequenceDone[sequence] = true;
        }
        else {

        }

        if (timer >= 1) {
            sequence += 1;
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}