using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject[] players;
    public float speed = 10f;

    public static GameObject[] carrying;
    public static Animator[] anims;
    public static int[] goals;
    MovingDirection[] directions;
    public static bool[] ready;


    public enum MovingDirection
    {
        Up = -1,
        Down = 1,
        Left = -2,
        Right = 2,
        Center = 0
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadGoal();
        ready = new bool[2];
        carrying = new GameObject[2];
        directions = new MovingDirection[2];
        directions[0] = directions[1] = MovingDirection.Center;
        anims = new Animator[2];
        anims[0] = players[0].GetComponent<Animator>();
        anims[1] = players[1].GetComponent<Animator>();

        
        if (players.Length != 2)
        {
            Debug.LogError("players must be 2.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready[0])
            PlayerMove(0);
        if (!ready[1])
            PlayerMove(1);
        CarryItem();
       
    }

    private void PlayerMove(int n)
    {
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
        Vector3 tmpv = players[n].transform.position;
        Vector3 movV = new Vector3(h, v, 0);
        tmpv += movV * speed * Time.deltaTime;
        players[n].transform.position = tmpv;
        
        
        // 动画
        directions[n] = GetDirection(new Vector2(h, v));
        anims[n].SetInteger("Direction", (int)directions[n]);
        anims[n].SetFloat("DirectionFloat", (float)directions[n]);
        
        if (Mathf.Abs(h) + Mathf.Abs(v) < 0.2f)
        {
            anims[n].SetBool("Walking", false);
        }
        else
        {
            anims[n].SetBool("Walking", true);
        }
        
    }


    protected MovingDirection GetDirection(Vector2 inputV2)
    {
        if (Mathf.Approximately(inputV2.x, 0))
        {
            if (inputV2.y > Mathf.Epsilon)
            {
                return MovingDirection.Up;
            }

            if (inputV2.y < -Mathf.Epsilon)
            {
                return MovingDirection.Down;
            }
        }

        if (Mathf.Approximately(inputV2.y, 0))
        {
            if (inputV2.x > Mathf.Epsilon)
            {
                return MovingDirection.Right;
            }

            if (inputV2.x < -Mathf.Epsilon)
            {
                return MovingDirection.Left;
            }
        }

        return MovingDirection.Center;
    }

    private void CarryItem()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && !ready[0])
        {
            //Debug.Log("Carry");
            if (carrying[0] == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(players[0].transform.position, Vector2.zero, 100, 1 << LayerMask.NameToLayer("Item"));
                if (hit)
                {
                    GetComponent<AudioCtrl>().PlayPickUp();
                    // 瞬移，请
                    Vector3 v = players[0].transform.position;
                    v.y += 0.7f;
                    hit.collider.gameObject.transform.position = v;
                    hit.collider.gameObject.transform.SetParent(players[0].transform);
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    carrying[0] = hit.collider.gameObject;
                    anims[0].SetBool("Carrying", true);
                }
            }
            else
            {
                if (!ready[0])
                {
                    GetComponent<AudioCtrl>().PlayPutDown();
                    carrying[0].transform.SetParent(null);
                    carrying[0].GetComponent<BoxCollider2D>().enabled = true;
                    carrying[0] = null;
                    anims[0].SetBool("Carrying", false);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Joystick2Button0) && !ready[1])
        {
            //Debug.Log("Carry2");
            if (carrying[1] == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(players[1].transform.position, Vector2.zero, 100, 1 << LayerMask.NameToLayer("Item"));
                if (hit)
                {
                    GetComponent<AudioCtrl>().PlayPickUp();
                    // 瞬移，请
                    Vector3 v = players[1].transform.position;
                    v.y += 0.7f;
                    hit.collider.gameObject.transform.position = v;
                    hit.collider.gameObject.transform.SetParent(players[1].transform);
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    carrying[1] = hit.collider.gameObject;
                    anims[1].SetBool("Carrying", true);
                }
            }
            else
            {
                if (!ready[1])
                {
                    GetComponent<AudioCtrl>().PlayPutDown();
                    carrying[1].transform.SetParent(null);
                    carrying[1].GetComponent<BoxCollider2D>().enabled = true;
                    carrying[1] = null;
                    anims[1].SetBool("Carrying", false);
                }
            }
        }
    }

    // 加载goal
    private void LoadGoal()
    {
        goals = new int[2];
        GameObject record = GameObject.Find("GoalRecord");
        if (record != null)
        {
            goals[0] = record.GetComponent<GoalRecord>().goals[0];
            goals[1] = record.GetComponent<GoalRecord>().goals[1];
            Destroy(record);
        }
    }
}
