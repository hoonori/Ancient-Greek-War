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
    public GameObject abilityChangeMountain;
    public GameObject abilityFireBlast;

    public GameObject buildButton;
    public GameObject buildPolis;
    public GameObject buildTempleWater;
    public GameObject buildTempleFire;
    public GameObject buildTempleRock;
    public GameObject buildStandingStones;

    public GameObject skipButton;

    public GameObject menuButton;

    public GameObject backButton;

    // Start is called before the first frame update
    void Start()
    {
        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);
        abilityChangeMountain.SetActive(false);
        abilityFireBlast.SetActive(false);

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);

        backButton.SetActive(false);
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
            case "Skip":
                skipButton.GetComponent<Button>().interactable = true;
                break;
            case "Menu":
                menuButton.GetComponent<Button>().interactable = true;
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
            case "Skip":
                skipButton.GetComponent<Button>().interactable = false;
                break;
            case "Menu":
                menuButton.GetComponent<Button>().interactable = false;
                break;
        }
    }

    public void Appear()
    {
        abilityButton.SetActive(true);
        buildButton.SetActive(true);
        skipButton.SetActive(true);
        menuButton.SetActive(true);
    }

    public void Disappear()
    {
        abilityButton.SetActive(false);
        buildButton.SetActive(false);
        skipButton.SetActive(false);
        menuButton.SetActive(false);
    }

    public void AbilityButton()
    {
        Disappear();
        abilityButton.SetActive(true);
        TurnOff("Ability");

        abilityMoveUnit.SetActive(true);
        abilityChangeWater.SetActive(true);
        abilityChangeMountain.SetActive(true);
        abilityFireBlast.SetActive(true);

        backButton.SetActive(true);
    }

    public void AbilityMoveUnit()
    {
        userInterfaceManager.SendMessage("AbilityButton", "moveUnit");

        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);
        abilityChangeMountain.SetActive(false);
        abilityFireBlast.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Ability");
    }

    public void AbilityChangeWater()
    {
        userInterfaceManager.SendMessage("AbilityButton", "changeWater");

        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);
        abilityChangeMountain.SetActive(false);
        abilityFireBlast.SetActive(false);

        backButton.SetActive(false);
        
        Appear();
        TurnOn("Ability");
    }

    public void AbilityChangeMountain()
    {
        userInterfaceManager.SendMessage("AbilityButton", "changeMountain");

        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);
        abilityChangeMountain.SetActive(false);
        abilityFireBlast.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Ability");
    }

    public void AbilityFireBlast()
    {
        userInterfaceManager.SendMessage("AbilityButton", "fireBlast");

        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);
        abilityChangeMountain.SetActive(false);
        abilityFireBlast.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Ability");
    }

    public void BuildButton()
    {
        Disappear();
        buildButton.SetActive(true);
        TurnOff("Build");

        buildPolis.SetActive(true);
        buildTempleWater.SetActive(true);
        buildTempleFire.SetActive(true);
        buildTempleRock.SetActive(true);
        buildStandingStones.SetActive(true);

        backButton.SetActive(true);
    }

    public void BuildPolis()
    {
        userInterfaceManager.SendMessage("BuildButton", "Polis");

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Build");
    }

    public void BuildTempleWater()
    {
        userInterfaceManager.SendMessage("BuildButton", "templeWater");

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Build");
    }

    public void BuildTempleFire()
    {
        userInterfaceManager.SendMessage("BuildButton", "templeFire");

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Build");
    }

    public void BuildTempleRock()
    {
        userInterfaceManager.SendMessage("BuildButton", "templeRock");

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Build");
    }

    public void BuildStandingStones()
    {
        userInterfaceManager.SendMessage("BuildButton", "standingStones");

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);

        backButton.SetActive(false);

        Appear();
        TurnOn("Build");
    }

    public void BackButton()
    {
        abilityMoveUnit.SetActive(false);
        abilityChangeWater.SetActive(false);
        abilityChangeMountain.SetActive(false);
        abilityFireBlast.SetActive(false);
        TurnOn("Ability");

        buildPolis.SetActive(false);
        buildTempleWater.SetActive(false);
        buildTempleFire.SetActive(false);
        buildTempleRock.SetActive(false);
        buildStandingStones.SetActive(false);
        TurnOn("Build");

        backButton.SetActive(false);

        Appear();
    }

    public void MenuButton()
    {
        userInterfaceManager.SendMessage("MenuButton", "");
    }

    public void SkipButton()
    {
        userInterfaceManager.SendMessage("SkipButton", "");
    }
}
