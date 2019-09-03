using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    public GameObject goalRecord;

    public enum TextContent
    {
        First=1,
        Second=2,
        Third=3,
        Fourth=4
    }

    public int Player1Goal;
    public int Player2Goal;
    public int Coin;//掷硬币决定谁大谁小
    public Text m_Text;
    public bool EyesOpen=false;
    protected TextContent LastText = TextContent.First;
    
    void Start()
    {
        LastText = TextContent.First;
        ResetGoal();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        ChangeText();
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("ChipScene");
        }
    }

    private void ChangeText()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            
            
            if (LastText==TextContent.Fourth)
            {
                EyesOpen = true;
                SceneManager.LoadScene("ChipScene");
            }
            if (LastText == TextContent.Third)
            {
                m_Text.text = "Player 1, your goal is " + Player1Goal + ".";
                //m_Text.text = "Player 2's goal is " + Player2Goal + ".";
                LastText = TextContent.Fourth;

            }

        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            if (LastText == TextContent.First)
            {
                m_Text.text = "Player 2, your goal is " + Player2Goal + ".";
                //m_Text.text = "Player 1's goal is " + Player1Goal + ".";
                LastText = TextContent.Second;
            }
            else if (LastText == TextContent.Second)
            {
                m_Text.text = "Player 2, close your eyes.";
                LastText = TextContent.Third;
            }
        }
    }
    private void ResetGoal()
    {
        Coin = Random.Range(0, 2);
        switch (Coin)
        {
            case 0:
                Player1Goal = Random.Range(11, 16);
                Player2Goal = Random.Range(26, 31);
                break;
            case 1:
                Player2Goal = Random.Range(11, 16);
                Player1Goal = Random.Range(26, 31);
                break;
        }
        //Debug.Log("Player1Goal" + Player1Goal);
        //Debug.Log("Player2Goal" + Player2Goal);
        // 存
        goalRecord.GetComponent<GoalRecord>().goals = new int[2];
        goalRecord.GetComponent<GoalRecord>().goals[0] = Player1Goal;
        goalRecord.GetComponent<GoalRecord>().goals[1] = Player2Goal;
    }
}
