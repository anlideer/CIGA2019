using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseChip : MonoBehaviour
{
    public GameObject[] player1ChipObj;
    public GameObject[] player2ChipObj;
    public Text[] player1Texts;
    public Text[] player2Texts;
    public Image[] player1Chips;
    public Image[] player2Chips;
    public GameObject p1Ready;
    public GameObject p2Ready;
    public GameObject Record;
    public AudioSource readyAudio;

    public int maxChipNum = 6;
    public float cd = 0.2f;
    public int totalChipNum = 20;


    int[] player1ChipNum;
    int[] player2ChipNum;
    int[] currentChip;

    float timecnt;

    bool[] isReady;

    Sprite blank;
    Sprite[] chips;
    Sprite unChosenSprite;
    Sprite chosenSprite;
    


    // Start is called before the first frame update
    void Start()
    {
        timecnt = -1f;
        player1ChipNum = new int[6];
        player2ChipNum = new int[6];
        currentChip = new int[2];
        isReady = new bool[2];
        player1ChipNum[0] = player1ChipNum[1] = player2ChipNum[0] = player2ChipNum[1] = 5;
        player1ChipNum[2] = player2ChipNum[2] = 3;
        if (player1ChipObj.Length != 6 || player2ChipObj.Length != 6)
        {
            Debug.LogWarning("The number of chips should be 6...");
        }

        LoadSprites();
    }

    // Update is called once per frame
    void Update()
    {
        // 避免太快了，搞个cd
        if (timecnt + cd < Time.timeSinceLevelLoad)
        {
            timecnt = Time.timeSinceLevelLoad;
            AxisCtrl(0);
            AxisCtrl(1);
        }
        
        // ready
        if (Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            if (CalculateChips(0) == totalChipNum)
            {
                // p1 ready
                readyAudio.Play();
                isReady[0] = true;
                p1Ready.SetActive(true);
            }
        }
        if (Input.GetKeyUp(KeyCode.Joystick2Button0))
        {
            if (CalculateChips(1) == totalChipNum)
            {
                // p2 ready
                readyAudio.Play();
                isReady[1] = true;
                p2Ready.SetActive(true);
            }
        }

        // 跳转
        if (isReady[0] && isReady[1])
        {
            // 记录一下现在的情况
            Record.GetComponent<Record>().player1ChipNum = player1ChipNum;
            Record.GetComponent<Record>().player2ChipNum = player2ChipNum;
            // 跳转
            SceneManager.LoadScene("MainScene");
        }
    }

    private void AxisCtrl(int n)
    {
        if (isReady[n])
        {
            return;
        }
        float h, v;
        h = v = 0;
        if (n == 0)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        else if (n == 1)
        {
            h = Input.GetAxisRaw("Horizontal2");
            v = Input.GetAxisRaw("Vertical2");
        }
        
        // 调整目前要调整的chip块
        if (h == 0 && v == 0)
        {
            // pass
        }
        else if (Mathf.Abs(h) > Mathf.Abs(v))
        {
            if(h > 0)
            {
                MoveToNext(n, true);
            }
            else if (h < 0)
            {
                MoveToNext(n, false);
            }
        }
        // 调整具体的chip个数
        else
        {
            if (v > 0)
            {
                ChangeChipNum(n, true);
            }
            else if (v < 0)
            {
                ChangeChipNum(n, false);
            }
        }
    }

    // 去下一个
    private void MoveToNext(int n, bool direction)  // direction: true - +1, flase - -1
    {
        // 换回默认颜色
        if (n == 0)
        {
            player1ChipObj[currentChip[n]].GetComponent<Image>().sprite = unChosenSprite;
        }
        else if (n == 1)
        {
            player2ChipObj[currentChip[n]].GetComponent<Image>().sprite = unChosenSprite;
        }
        if (direction)
            currentChip[n] = (currentChip[n] + 1) % 6;
        else
            currentChip[n] = (currentChip[n] - 1 + 6) % 6;
        // 换来高亮颜色
        if (n == 0)
        {
            player1ChipObj[currentChip[n]].GetComponent<Image>().sprite = chosenSprite;
        }
        else if (n == 1)
        {
            //Debug.Log(currentChip[n]);
            player2ChipObj[currentChip[n]].GetComponent<Image>().sprite = chosenSprite;
        }
    }


    // 加载一下Resources里的一些Sprites资源
    private void LoadSprites()
    {
        blank = Resources.Load<Sprite>("blank") as Sprite;
        unChosenSprite = Resources.Load<Sprite>("UnChosen") as Sprite;
        chosenSprite = Resources.Load<Sprite>("Chosen") as Sprite;

        chips = new Sprite[9];
        for (int i = 0; i < 9; i++)
        {
            chips[i] = Resources.Load<Sprite>(i.ToString()) as Sprite;
        }

    }


    // 调整具体的Chip个数
    private void ChangeChipNum(int n, bool direction)   // direction: true - + 1, flase - -1
    {
        if (n == 0)
        {
            if ((direction && player1ChipNum[currentChip[n]] >= maxChipNum) || (direction == false && player1ChipNum[currentChip[n]] <= 0))
            {
                return;
            }
            if(direction)
                player1ChipNum[currentChip[n]] += 1;
            else
                player1ChipNum[currentChip[n]] -= 1;

            // 调整视觉表现
            int tmp = player1ChipNum[currentChip[n]];
            player1Texts[currentChip[n]].text = tmp.ToString();
            if (tmp == 0)
            {
                player1Chips[currentChip[n]].sprite = blank;
            }
            else
            {
                player1Chips[currentChip[n]].sprite = chips[tmp];
            }
        }
        else if (n == 1)
        {
            if ((direction && player2ChipNum[currentChip[n]] >= maxChipNum) || (direction == false && player2ChipNum[currentChip[n]] <= 0))
            {
                return;
            }
            if (direction)
                player2ChipNum[currentChip[n]] += 1;
            else
                player2ChipNum[currentChip[n]] -= 1;

            // 调整视觉表现
            int tmp = player2ChipNum[currentChip[n]];
            player2Texts[currentChip[n]].text = tmp.ToString();
            if (tmp == 0)
            {
                player2Chips[currentChip[n]].sprite = blank;
            }
            else
            {
                player2Chips[currentChip[n]].sprite = chips[tmp];
            }
        }
    }

    // 计算筹码总和
    private int CalculateChips(int n)
    {
        int sum = 0;
        if (n == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                sum += player1ChipNum[i];
            }
        }
        else if (n == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                sum += player2ChipNum[i];
            }
        }
        return sum;
    }
}
