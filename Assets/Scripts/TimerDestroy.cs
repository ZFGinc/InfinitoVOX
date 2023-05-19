using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    public float Timer = 2f;

    void Update()
    {
        Timer-=Time.deltaTime;
        if(Timer <= 0) Destroy(gameObject);
    }
}
