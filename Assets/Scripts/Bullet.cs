using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float SpeedMove = 4;
    public float Damage = 0;
    private float timer = 5;
    public bool isDestroyWhenTuch = true;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(new Vector3(0, 0, Time.deltaTime * SpeedMove));
        timer+=Time.deltaTime;
        if(timer <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().GetDamage(Damage);
        }
        else if(collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<Boss>().GetDamage(Damage);
        }

        if (isDestroyWhenTuch) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //Тут типа эфекты 
    }
}
