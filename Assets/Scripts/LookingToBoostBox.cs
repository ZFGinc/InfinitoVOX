using UnityEngine;

public class LookingToBoostBox : MonoBehaviour
{
    [SerializeField] private Transform _box;
    [SerializeField] private GameObject _arrow;

    private void Update()
    {
        if(_box == null)
        {
            _arrow.SetActive(false);
            return;
        }

        transform.LookAt(_box);

    }

    public void LookBox(Transform box)
    {
        _box = box;
        _arrow.SetActive(true);
    }
}
