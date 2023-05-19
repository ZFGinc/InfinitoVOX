using UnityEngine;

public class AltarController : MonoBehaviour
{
    [SerializeField] private Altar[] _altars;
    [SerializeField] private CameraMove _camera;
    [SerializeField] private EnemySpawn _spawner;
    [SerializeField] private LookingToBoostBox _looker;

    private bool _isActive = false;
    private int altarIndex = 0;

    public void SpawnRandomAltar()
    {
        if (_isActive) return;

        _camera.SetTarget(_altars[altarIndex].transform);
        _looker.LookBox(_altars[altarIndex].transform);

        _altars[altarIndex].StartAnimation();

        _isActive = true;
    }

    public GameObject GetRandomBoss() => _spawner.Bosses[Random.Range(0, _spawner.Bosses.Length)];

    public void SetDefaultCamera()
    {
        _camera.SetDefaultCamera();
    }

    public void DisableArrow()
    {
        _looker.LookBox(null);
    }

    public void EndBossFight()
    {
        _spawner.EndBossFight();

        _camera.SetTarget(_altars[altarIndex].transform);

        _altars[altarIndex].StartAnimation();

        altarIndex++;
        if (altarIndex >= _altars.Length) altarIndex = 0;

        _isActive = false;
    }
}
