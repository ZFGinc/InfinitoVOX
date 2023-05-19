using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] Enemys;
    public GameObject[] Bosses;
    public GameObject Player;

    [SerializeField] private AltarController altarController;

    private const float _targetTimeForSpawn = 10f;
    private const int _baseSpawnCount = 2;
    private const int _maxCountWaves = 7;
    private const int _maxTryBossFight = 15;

    private uint _countWaves = 0;
    private uint _tryBossFight = 0;
    private bool _isBossSpawn = false;
    private List<GameObject> _spawnList;

    private void Start()
    {
        _spawnList = new List<GameObject>();
        StartCoroutine(SpawnerEnemy());
    }

    private void ClearNullSpawnList()
    {
        int i = 0;
        while(i < _spawnList.Count)
        {
            if (_spawnList[i] == null)
                _spawnList.RemoveAt(i);
            else i++;
        }
    }

    public void EndBossFight()
    {
        _countWaves = 0;
        _tryBossFight = 0;
        _isBossSpawn = false;
    }

    private void KillAllEnemy()
    {
        foreach (GameObject e in _spawnList)
            Destroy(e);
        _spawnList.Clear();
    }

    private IEnumerator SpawnerEnemy()
    {
        while (true)
        {
            ClearNullSpawnList();

            while (_isBossSpawn)
            {
                yield return new WaitForSeconds(1f);
                ClearNullSpawnList();

                if (_spawnList.Count == 0 && _isBossSpawn) 
                { 
                    KillAllEnemy();
                    altarController.SpawnRandomAltar();
                } 
                else
                {
                    _tryBossFight++;

                    if(_tryBossFight >= _maxTryBossFight)
                    {
                        KillAllEnemy();
                    }
                }
            }

            while (!_isBossSpawn)
            {
                yield return new WaitForSeconds(2f);

                int _newSpawnCount = _baseSpawnCount + (int)(PlayerStats.Level / 8);
                for (int i = 0; i < _newSpawnCount; i++)
                {

                    float dx = 0;
                    while (dx == 0) dx = Random.Range(-15f, 15f);

                    float dz = Mathf.Pow(225 - Mathf.Pow(dx, 2), 0.5f);

                    int idEnemy = 0;

                    if (PlayerStats.Level > Enemys.Length)
                        idEnemy = Random.Range(0, Enemys.Length);
                    else
                        idEnemy = Random.Range(0, (int)PlayerStats.Level);

                    if (idEnemy > Enemys.Length) idEnemy = Enemys.Length - 1;

                    GameObject enemy = Instantiate(Enemys[idEnemy], new Vector3(Player.transform.position.x + dx, Player.transform.position.y, Player.transform.position.z + dz), Quaternion.identity);
                    _spawnList.Add(enemy);
                }
                _countWaves++;
                if (_countWaves >= _maxCountWaves)
                {
                    _isBossSpawn = true;
                    break;
                }
                yield return new WaitForSeconds(_targetTimeForSpawn);
            }
        }
    }
}
