using UnityEngine;

public class MarkerEventAnimation : MonoBehaviour
{
    private EnemyMain target;

    private void Start()
    {
        if(transform.parent.GetComponent<Enemy>())
            target = transform.parent.GetComponent<Enemy>();
        else
            target = transform.parent.GetComponent<Boss>();
    }
    
    public void DestroyObject() => target.DestroyEnemy();
    public void DisableSounds() => target.DisableSounds();
    public void EndImmortal() => target.EndImmortal();
}
