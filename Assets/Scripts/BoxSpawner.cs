using System.Collections;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    private const float _timer = 16f;
    private const float _radius = 3f;

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject[] _allBoxes;
    [SerializeField] private LookingToBoostBox _lookerBox;

    private GameObject _currentBoost = null;

    public static BoxSpawner Instance { get; private set; }

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timer);
            while (_currentBoost != null || BoostApplication.IsTempBoosting) yield return new WaitForSeconds(1f);

            float x = Random.Range(-_radius, _radius);
            float z = Random.Range(-_radius, _radius);

            int _id = Random.Range(0, _allBoxes.Length);

            var _obj = Instantiate(_allBoxes[_id],
                        new Vector3(_player.position.x + x, 1.2f, _player.position.z + z),
                        Quaternion.identity);
            _currentBoost = _obj;

            _lookerBox.LookBox(_currentBoost.transform);
        }
    }

    private Vector2 GetPosition() => new Vector2(Random.Range(-_radius, _radius), Random.Range(-_radius, _radius));
    private GameObject GetRandomBox() => _allBoxes[Random.Range(0, _allBoxes.Length)];

    public void SpawnBox()
    {
        Vector2 position = GetPosition();
        GameObject box = GetRandomBox();

        var _obj = Instantiate(box,
                    new Vector3(_player.position.x + position.x, 1.2f, _player.position.z + position.y),
                    Quaternion.identity);

        _currentBoost = _obj;

        _lookerBox.LookBox(_currentBoost.transform);
    }

    private void OnDestroy()
    {
        StopCoroutine(Spawner());
    }
}
