using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public GameObject Player1Ready;
    public GameObject Player2Ready;
    public float Player1Timer;
    public float Player2Timer;
    public bool ifPlayer1Ready = false;
    public bool ifPlayer2Ready = false;
    // Start is called before the first frame update
    void Start()
    {
        Player1Timer = 0;
        Player2Timer = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ifPlayer1Ready==false)
        {
            CheckPlayer1Ready();
        }
        if (ifPlayer2Ready==false)
        {
            CheckPlayer2Ready();
        }
        
        CheckIfReady();
    }

    private void CheckIfReady()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            ifPlayer1Ready = true;
            Player1Ready.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            ifPlayer2Ready = true;
            Player2Ready.SetActive(true);
        }

        if (ifPlayer1Ready && ifPlayer2Ready)
        {
            SceneManager.LoadScene("CGJMain");
        }
    }
    private void CheckPlayer1Ready()
    {
        Player1Timer += Time.deltaTime;
        if (Player1Timer >= 0.8)
        {
            Player1Timer = 0;
        }
        if (Player1Timer < 0.4f)
        {
            Player1Ready.SetActive(true);
        }
        else if (Player1Timer > 0.4f)
        {
            Player1Ready.SetActive(false);
        }
    }
    private void CheckPlayer2Ready()
    {
        Player2Timer += Time.deltaTime;
        if (Player2Timer >= 1.1f)
        {
            Player2Timer = 0.3f;
        }
        if (Player2Timer < 0.5f || Player2Timer >= 0.9f)
        {
            Player2Ready.SetActive(true);
        }
        else if (Player2Timer > 0.5f && Player2Timer < 0.9f)
        {
            Player2Ready.SetActive(false);
        }
    }
}
