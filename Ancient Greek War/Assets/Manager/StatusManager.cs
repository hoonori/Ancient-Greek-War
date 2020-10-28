using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public Text turnStatus;
    public int turn;

    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        turnStatus.text = turn.ToString();
    }

    void SetTurn(string input)
    {
        turn = System.Convert.ToInt32(input);
    }
}
