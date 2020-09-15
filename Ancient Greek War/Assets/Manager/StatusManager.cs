using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public Text statusGold;
    public int currGold;
    public Text statusTurn;
    public int currTurn;

    // Start is called before the first frame update
    void Start()
    {
        currGold = 0;
        currTurn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currGold += 1;

        if (currGold >= 1000)
        {
            currTurn += 1;
            currGold -= 1000;
        }

        statusGold.text = "Gold : " + currGold.ToString();
        statusTurn.text = "Turn : " + currTurn.ToString();
    }
}
