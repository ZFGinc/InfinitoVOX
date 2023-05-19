using UnityEngine;

public class DisableColliderTimer : MonoBehaviour
{
    public float Timer = .5f;
    public Collider Collider;

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0) Collider.enabled = false;
    }
}
