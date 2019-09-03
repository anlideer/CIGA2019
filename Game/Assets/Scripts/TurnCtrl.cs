using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO: 加结果判断
public class TurnCtrl : MonoBehaviour
{
    public Text roundInfo;
    public Text p1Text;
    public Text p2Text;
    public GameObject panel;
    public Text resText;
    public GameObject numText;
    public GameObject player1;
    public GameObject player2;

    int round = 0;
    bool isWaiting = false;
    bool isTrading = false;
    float timecnt;
    bool[] isCheat;
    float roundTimecnt;
    bool isOver = false;

    // Start is called before the first frame update
    void Start()
    {
        round = 0;
        isWaiting = false;
        isTrading = false;
        isCheat = new bool[2];
        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        { 
            if (roundTimecnt + 5f < Time.time)
            {
                DetectReady();
            }

            if (isTrading)
            {
                Cheat();

                roundInfo.text = "Round " + (round + 1).ToString() + ": Trading";
                float tmpf = timecnt + 3f - Time.time + 1f;
                if (tmpf > 3f)
                    tmpf = 3f;
                numText.GetComponent<Text>().text = ((int)tmpf).ToString();
                //Debug.Log(timecnt + 3f - Time.time);
                if (timecnt + 3f < Time.time)
                {
                    isTrading = false;
                    PlayerCtrl.ready[0] = PlayerCtrl.ready[1] = false;
                    roundTimecnt = Time.time;

                    // 结算回合结果
                    // 这里先简单的让交易成功的东西瞬移到远一点的地方去...我真的tmd好困
                    if (isCheat[0] == false)
                    {
                        if (PlayerCtrl.carrying[0] != null)
                        {
                            GetComponent<AudioCtrl>().PlayTransaction();
                            GameObject obj = PlayerCtrl.carrying[0];
                            Vector3 v = obj.transform.position;
                            v.x += 6f;
                            obj.transform.position = v;
                            obj.GetComponent<BoxCollider2D>().enabled = true;
                            obj.transform.SetParent(null);
                            int index = FindIndex(ChipCtrl.p1Chips, obj);
                            if (index == -1)
                            {
                                Debug.LogWarning("Error: Not find");
                            }
                            else
                            {
                                ChipCtrl.p1Chips.RemoveAt(index);
                                int c = ChipCtrl.p1Num[index];
                                ChipCtrl.p2Chips.Add(obj);
                                ChipCtrl.p2Num.Add(c);
                                ChipCtrl.p1Num.RemoveAt(index);
                            }
                            PlayerCtrl.carrying[0] = null;
                            PlayerCtrl.anims[0].SetBool("Carrying", false);

                        }
                    }
                    else
                    {
                        GetComponent<AudioCtrl>().PlayCheater();
                    }
                    if (isCheat[1] == false)
                    {
                        if (PlayerCtrl.carrying[1] != null)
                        {
                            GetComponent<AudioCtrl>().PlayTransaction();
                            GameObject obj = PlayerCtrl.carrying[1];
                            Vector3 v = obj.transform.position;
                            v.x -= 6f;
                            obj.transform.position = v;
                            obj.GetComponent<BoxCollider2D>().enabled = true;
                            obj.transform.SetParent(null);
                            int index = FindIndex(ChipCtrl.p2Chips, obj);
                            if (index == -1)
                            {
                                Debug.LogWarning("Error: Not find");
                            }
                            else
                            {
                                ChipCtrl.p2Chips.RemoveAt(index);
                                int c = ChipCtrl.p2Num[index];
                                ChipCtrl.p1Chips.Add(obj);
                                ChipCtrl.p1Num.Add(c);
                                ChipCtrl.p2Num.RemoveAt(index);
                            }
                            PlayerCtrl.carrying[1] = null;
                            PlayerCtrl.anims[1].SetBool("Carrying", false);
                        }
                    }
                    else
                    {
                        GetComponent<AudioCtrl>().PlayCheater();
                    }

                    // 判断游戏是否结束
                    bool p1win, p2win;
                    p1win = p2win = false;
                    // 更新Text
                    int p1 = CalculateSum(0);
                    p1Text.text = "Total: " + p1.ToString();
                    if (p1 == PlayerCtrl.goals[0])
                    {
                        p1win = true;
                    }
                    int p2 = CalculateSum(1);
                    p2Text.text = "Total: " + p2.ToString();
                    if (p2 == PlayerCtrl.goals[1])
                    {
                        p2win = true;
                    }
                    // 双赢
                    if (p1win && p2win)
                    {
                        GetComponent<AudioCtrl>().PlayWin();
                        resText.text = "Double Win!";
                        panel.SetActive(true);
                        //Debug.Log("Both win");
                        isOver = true;
                    }
                    // p1 赢
                    else if (p1win)
                    {
                        GetComponent<AudioCtrl>().PlayWin();
                        resText.text = "Player1 wins";
                        //resText.text = "Player2 wins";
                        panel.SetActive(true);
                        //Debug.Log("Player1 wins");
                        isOver = true;
                    }
                    // p2 赢
                    else if (p2win)
                    {
                        GetComponent<AudioCtrl>().PlayWin();
                        resText.text = "Player2 wins";
                        //resText.text = "Player1 wins";
                        panel.SetActive(true);
                        //Debug.Log("Player2 wins");
                        isOver = true;
                    }

                    // 双输
                    if (round >= 9)
                    {
                        GetComponent<AudioCtrl>().PlayLose();
                        resText.text = "Both of you lose.";
                        panel.SetActive(true);
                        //Debug.Log("Both lose");
                        isOver = true;
                    }



                    // 其他处理
                    isCheat[0] = isCheat[1] = false;
                    isWaiting = false;
                    numText.SetActive(false);
                    round++;


                }
            }
        }
        // 任意人按A就回到封面
        else
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                SceneManager.LoadScene("CGJCover");
            }
        }
    }

    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            isCheat[0] = true;
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
        {
            isCheat[1] = true;
        }
    }

    // 检测是否站在台子上（以及谁先的问题
    private void DetectReady()
    {
        if (!isWaiting)
        {
            // p1 first
            if (round % 2 == 0)
            {
                roundInfo.text = "Round " + (round+1).ToString() + ": Player l First";
                RaycastHit2D hit = Physics2D.Raycast(player1.transform.position, Vector2.zero, 100, 1 << LayerMask.NameToLayer("Ready"));
                if (hit && PlayerCtrl.carrying[0] != null)
                {
                    PlayerCtrl.ready[0] = true;
                    isWaiting = true;
                }
            }
            // p2 first
            else
            {
                roundInfo.text = "Round " + (round+1).ToString() + ": Player 2 First";
                RaycastHit2D hit = Physics2D.Raycast(player2.transform.position, Vector2.zero, 100, 1 << LayerMask.NameToLayer("Ready"));
                if (hit && PlayerCtrl.carrying[1] != null)
                {
                    PlayerCtrl.ready[1] = true;
                    isWaiting = true;
                }
            }
        }
        else if (isWaiting && !isTrading)
        {
            // p1 first
            if (round % 2 == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(player2.transform.position, Vector2.zero, 100, 1 << LayerMask.NameToLayer("Ready"));
                if (hit && PlayerCtrl.carrying[1] != null)
                {
                    PlayerCtrl.ready[1] = true;
                    GetComponent<AudioCtrl>().Play321();
                    isTrading = true;
                    timecnt = Time.time;
                    numText.SetActive(true);
                }
            }
            // p2 first
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(player1.transform.position, Vector2.zero, 100, 1 << LayerMask.NameToLayer("Ready"));
                if (hit && PlayerCtrl.carrying[0] != null)
                {
                    PlayerCtrl.ready[0] =  true;
                    GetComponent<AudioCtrl>().Play321();
                    isTrading = true;
                    timecnt = Time.time;
                    numText.SetActive(true);
                }
            }
        }
    }

    // 算了手写find吧
    private int FindIndex(List<GameObject> list, GameObject obj)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                return i;
            }
        }
        return -1;
    }

    // 计算总值
    private int CalculateSum(int n)
    {
        int sum = 0;
        if (n == 0)
        {
            foreach(int i in ChipCtrl.p1Num)
            {
                sum += i;
            }
        }
        else if (n == 1)
        {
            foreach (int i in ChipCtrl.p2Num)
            {
                sum += i;
            }
        }
        return sum;
    }
}
