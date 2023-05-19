using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform target;

    private const float _speed = 3f;
    private float TempSpeed;

    void Update()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        TempSpeed = dist * _speed;

        transform.position = Vector3.MoveTowards(transform.position, target.position, TempSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target) => this.target = target;

    public void SetDefaultCamera() => SetTarget(Player.Init.transform);
    
}
