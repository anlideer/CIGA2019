using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverCtrl : MonoBehaviour
{
    public GameObject BGM;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("BGM") == null)
        {
            Instantiate(BGM);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
