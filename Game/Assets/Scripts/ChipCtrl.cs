using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCtrl : MonoBehaviour
{
    public Transform[] player1InsPoints;
    public Transform[] player2InsPoints;

    public GameObject[] chipPrefabs;

    public static List<GameObject> p1Chips;
    public static List<GameObject> p2Chips;
    public static List<int> p1Num;
    public static List<int> p2Num;

    // Start is called before the first frame update
    void Start()
    {
        p1Chips = new List<GameObject>();
        p2Chips = new List<GameObject>();
        p1Num = new List<int>();
        p2Num = new List<int>();
        LoadChips();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 把Record里的东西读过来，然后销毁Record    
    /// 我好困啊nmd
    private void LoadChips()
    {
        GameObject record = GameObject.Find("Record");
        if (record != null)
        {
            int[] tmp1 = record.GetComponent<Record>().player1ChipNum;
            int[] tmp2 = record.GetComponent<Record>().player2ChipNum;
            for (int i = 0; i < 6; i++)
            {
                if (tmp1[i] > 0)
                {
                    p1Num.Add(tmp1[i]);
                    GameObject obj = Instantiate(chipPrefabs[tmp1[i] - 1], player1InsPoints[i].position, transform.rotation);
                    p1Chips.Add(obj);
                }
                if (tmp2[i] > 0)
                {
                    p2Num.Add(tmp2[i]);
                    GameObject obj = Instantiate(chipPrefabs[tmp2[i] - 1], player2InsPoints[i].position, transform.rotation);
                    p2Chips.Add(obj);
                }
            }
        }
        Destroy(record);
    }
}
