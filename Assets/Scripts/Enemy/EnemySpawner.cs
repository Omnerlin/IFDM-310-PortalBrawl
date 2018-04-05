using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public enum waveState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject[] enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves;
    public float waveCountdown;
    public waveState state = waveState.COUNTING;
    private float searchCountdown = 1f;

    public GameObject[] spawnPoints;

	// Use this for initialization
	void Start ()
    {
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found");
        }
        waveCountdown = timeBetweenWaves;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(state == waveState.WAITING)
        {
            if (!enemyIsAlive())
            {
                //Begin new round
                waveCompleted();
                return;
            }
            else
            {
                return;
            }
        }
        if(waveCountdown <= 0)
        {
            if(state != waveState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

    }

    void waveCompleted()
    {
        Debug.Log("Wave Complete");
        state = waveState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if(nextWave + 1 > waves.Length)
        {
            nextWave = 0;
            Debug.Log("All waves complete: Looping...");
        }
        else
        {
            nextWave++;
        }
    }

    bool enemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0)
        {
            searchCountdown = 1;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.waveName);
        state = waveState.SPAWNING;
        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy[Random.Range(0, _wave.enemy.Length)]);
            yield return new WaitForSeconds(1f/_wave.rate);
        }
        state = waveState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject _Enemy)
    {
        GameObject _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found");
        }
        Instantiate(_Enemy, _sp.transform.position, _sp.transform.rotation);
        Debug.Log("Spawning Enemy: " + _Enemy);
    }
}
