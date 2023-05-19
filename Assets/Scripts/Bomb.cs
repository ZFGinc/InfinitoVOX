using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float Damage;
    private float timer = 0;
    private float timerexp = 0;
    public GameObject explosionObj;
    Rigidbody rb;
    bool isExplosion = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 10, ForceMode.Impulse);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(isExplosion) timerexp += Time.deltaTime;
        if (timer > 3f) Explosion();
        if(timer > 8f || timerexp > 2) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ground")
            Explosion();
        
    }

    private async void Explosion()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        explosionObj.GetComponent<Bullet>().Damage = Damage;
        explosionObj.SetActive(true);
        isExplosion = true;
    }
}
