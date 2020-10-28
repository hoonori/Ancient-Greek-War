using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitScript : MonoBehaviour
{
    public float actualX, actualY;
    public float targetX, targetY;

    public float actualHealth;
    public float targetHealth;
    public Image healthBar;

    // Start is called before the first frame update   
    void Start()
    {
        Vector3 currPosition = transform.position;

        actualX = currPosition[0];
        actualY = currPosition[1];
        targetX = actualX;
        targetY = actualY;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = actualHealth;
        
        if ((targetHealth - actualHealth) <= -0.0625f || (targetHealth - actualHealth) >= 0.0625f)
        {
            actualHealth += targetHealth > actualHealth ? 0.0625f : -0.0625f;
        }

        if ((targetX - actualX) <= -0.0625f || (targetX - actualX) >= 0.0625f)
        {
            actualX += targetX > actualX ? 0.0625f : -0.0625f;
        }
        if ((targetY - actualY) <= -0.0625f || (targetY - actualY) >= 0.0625f)
        {
            actualY += targetY > actualY ? 0.0625f : -0.0625f;
        }

        transform.position = new Vector3(actualX, actualY, -1);
    }
    
    public void Move(string input)
    {
        string[] inputs = input.Split(',');
        int gridX = System.Convert.ToInt32(inputs[0]);
        int gridY = System.Convert.ToInt32(inputs[1]);
        int gridIndex = System.Convert.ToInt32(inputs[2]);

        float xOffset = 0.0f;
        float yOffset = 0.0f;
        switch (gridIndex)
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
        }

        targetX = 2.5f * (float)gridX + xOffset * 0.625f;
        targetY = 2.5f * (float)gridY + yOffset * 0.625f;
    }

    public void SetUnitHealth(string input)
    {
        targetHealth = (float) System.Convert.ToSingle(input);
    }
}
