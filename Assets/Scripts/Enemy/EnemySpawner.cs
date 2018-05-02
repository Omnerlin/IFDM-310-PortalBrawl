using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {

    public enum waveState { SPAWNING, WAITING, COUNTING, DONE };

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject[] enemy;
        public int count;
        public float rate;
    }

    public GameObject portalPrefab;
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves;
    public float waveCountdown;
    public waveState state = waveState.COUNTING;
    private float searchCountdown = 1f;

    // New variables added by Alex B. for win condition vs. next level condition.
    public bool loop = false;
    public bool lastLevel = false;

    public GameObject[] spawnPoints;

	// Use this for initialization
	void Start ()
    {
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found");
        }
        waveCountdown = timeBetweenWaves;
        Debug.Log(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    // Update is called once per frame
    void Update ()
    {
        if(state == waveState.DONE)
        {
            return;
        }
        if(state == waveState.WAITING)
        {
            if (!enemyIsAlive())
            {
                // Test to see if there are any waves left if we killed all the enemies.
                if (nextWave + 1 >= waves.Length && !loop)
                {
                    if (!lastLevel)
                    {
                        PlayerManager.instance.playerPortal.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("hey there, I'm done spawning now");
                        PlayerManager.instance.GameWin();
                        state = waveState.DONE;
                    }
                }
                else
                {
                    //Begin new round
                    waveCompleted();
                }
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
        // Changing this a bit to spawn portals instead, which spawn the enemy that was going to be spawned
        // by this, if that makes sense.
        GameObject _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found");
        }

        Instantiate(portalPrefab, _sp.transform.position, _sp.transform.rotation);
        portalPrefab.GetComponent<PortalSpawn>().monsterToSpawn = _Enemy;
        //Instantiate(_Enemy, _sp.transform.position, _sp.transform.rotation);
        Debug.Log("Spawning Enemy: " + _Enemy);
    }
}
