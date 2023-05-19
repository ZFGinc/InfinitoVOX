using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "box")
        {
            Destroy(collision.gameObject);
            BoxSpawner.Instance.SpawnBox();
        }
    }
}
