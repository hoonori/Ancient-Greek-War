using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tileManager;
    public GameObject unitManager;
    public GameObject buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Input should be "x,y,type" where x and y being grid and type being terrain type.
    void ChangeTile(string input)
    {
        tileManager.SendMessage("ChangeTile", input);
    }

    void CreateUnit(string input)
    {
        unitManager.SendMessage("CreateUnit", input);
    }

    void MoveUnit(string input)
    {
        unitManager.SendMessage("MoveUnit", input);
    }

    void SetUnitHealth(string input)
    {
        unitManager.SendMessage("SetUnitHealth", input);
    }

    void DestroyUnit(string input)
    {
        unitManager.SendMessage("DestroyUnit", input);
    }

    void CreateBuilding(string input)
    {
        buildingManager.SendMessage("CreateBuilding", input);
    }

    void DestroyBuilding(string input)
    {
        buildingManager.SendMessage("DestroyBuilding", input);
    }
}
