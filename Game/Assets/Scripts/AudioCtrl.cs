using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    public AudioSource transaction;
    public AudioSource cheater;
    public AudioSource count;
    public AudioSource pickup;
    public AudioSource putdown;
    public AudioSource win;
    public AudioSource lose;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTransaction()
    {
        transaction.Play();
    }

    public void PlayCheater()
    {
        cheater.Play();
    }

    public void Play321()
    {
        count.Play();
    }

    public void PlayPickUp()
    {
        pickup.Play();
    }

    public void PlayPutDown()
    {
        putdown.Play();
    }

    public void PlayWin()
    {
        win.Play();
    }

    public void PlayLose()
    {
        lose.Play();
    }
}
