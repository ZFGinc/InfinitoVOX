using UnityEngine;

public class PositionAdder : MonoBehaviour
{
    [SerializeField] private Vector3 _positionAdd;
    [SerializeField] private bool _isRandom = false;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "box")
            MoveObject(collision.gameObject.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "box")
            MoveObject(other.gameObject.transform);
    }

    private void MoveObject(Transform obj)
    {
        if (_isRandom)
        {
            Vector3 newPosAdder = new Vector3(Random.Range(-5, Random.Range(1, 6)), 0, Random.Range(-5, Random.Range(1, 6)));
            obj.position = new Vector3(newPosAdder.x + obj.position.x, obj.position.y, obj.position.z + newPosAdder.z);
        }
        else obj.position = new Vector3(_positionAdd.x + obj.position.x, obj.position.y, obj.position.z + _positionAdd.z);
    }
}
