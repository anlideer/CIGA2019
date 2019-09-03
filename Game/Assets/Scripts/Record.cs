using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public int[] player1ChipNum;
    public int[] player2ChipNum;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
