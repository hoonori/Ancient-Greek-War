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
        statusGold.text = "Gold : " + currGold.ToString();
        statusTurn.text = "Turn : " + currTurn.ToString();
    }

    void SetTurn(string input)
    {
        currTurn = System.Convert.ToInt32(input);
    }
}
