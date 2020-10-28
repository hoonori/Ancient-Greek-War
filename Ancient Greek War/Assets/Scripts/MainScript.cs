using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(800, 600, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
