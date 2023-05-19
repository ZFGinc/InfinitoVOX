using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] private bool _findCamera;
    [SerializeField] private Transform _targetLook;

    private void Update()
    {
        if (_targetLook == null)
        {
            if(_findCamera) _targetLook = Camera.main.transform;
            else return;
        }

        transform.LookAt(_targetLook);
    }
}
