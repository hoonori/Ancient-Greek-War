using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject userInterfaceManager;

    public GameObject abilityButton;
    public GameObject abilityMoveUnit;
    public GameObject abilityChangeWater;

    public GameObject buildButton;
    public GameObject buildPolis;
    public GameObject buildTemple;

    public GameObject extraButton;

    // Start is called before the first frame update
    void Start()
    {
        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);

        buildPolis.SetActive(false);
        buildTemple.SetActive(false);
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

    public void Appear()
    {
        abilityButton.SetActive(true);
        buildButton.SetActive(true);
        extraButton.SetActive(true);
    }

    public void Disappear()
    {
        abilityButton.SetActive(false);
        buildButton.SetActive(false);
        extraButton.SetActive(false);
    }

    public void AbilityButton()
    {
        Disappear();

        abilityMoveUnit.SetActive(true);
        abilityChangeWater.SetActive(true);
    }

    public void AbilityMoveUnit()
    {
        userInterfaceManager.SendMessage("AbilityButton", "moveUnit");

        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);

        Appear();
    }

    public void AbilityChangeWater()
    {
        userInterfaceManager.SendMessage("AbilityButton", "changeWater");

        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);

        Appear();
    }

    public void BuildButton()
    {
        Disappear();

        buildPolis.SetActive(true);
        buildTemple.SetActive(true);
    }

    public void BuildPolis()
    {
        userInterfaceManager.SendMessage("BuildButton", "Polis");

        buildPolis.SetActive(false);
        buildTemple.SetActive(false);

        Appear();
    }

    public void BuildTemple()
    {
        userInterfaceManager.SendMessage("BuildButton", "Temple");

        buildPolis.SetActive(false);
        buildTemple.SetActive(false);

        Appear();
    }

    public void ExtraButton()
    {
        Disappear();

    }
}
