using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int _enemySpawned;
    [SerializeField] private int _wave;
    private int _weightedTotal;
    private int _powerUpToSpawn;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _missilePowerUpPrefab;
    [SerializeField] GameObject _asteroidPrefab;
    [SerializeField] GameObject _BomberPrefab;
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject _penaltyPrefab;
    [SerializeField] GameObject _player;
    private UI_Manager _uiManager;
    private bool _stopSpawning = false;


    void Start()
    {
        _wave = 1;
        _enemySpawned = 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_player == null)
        {
            Debug.LogError("The player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(SpawnWave1Routine());
            StartCoroutine(SpawnPowerupRoutine());
        }
    }

    void ChooseAPowerup()
    {
        _weightedTotal = 0;

        int[] powerUpTable =
        {
            40, //ammo
            25, //speed
            16, //health
            11, //shield
            8, //tripleshot
        };
        int[] powerUpToAward =
        {
            3, //ammo
            1, //speed
            2, //shield
            4, //health
            0, //tripleshot
        };

        foreach(var item in powerUpTable)
        {
            _weightedTotal += item;
        }

        var randomNumber = Random.Range(0, _weightedTotal);
        var i = 0;

        foreach(var weight in powerUpTable)
        {
            if(randomNumber <= weight)
            {
                _powerUpToSpawn = powerUpToAward[i];
                return;
            }
            else
            {
                i++;
                randomNumber -= weight;
            }
        }
    }

   IEnumerator SpawnWave1Routine()
    {
        while (_enemySpawned < 10)

        {

            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Vector3 bomberSpawnPos = new Vector3(11, 4, 0);

            int getAsteroidRandomNumber = (Random.Range(1, 5));
            if (getAsteroidRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }

            int getBomberRandomNumber = (Random.Range(1, 5));
            if (getBomberRandomNumber == 1)
            {
                Instantiate(_BomberPrefab, bomberSpawnPos, Quaternion.identity);
            }

            if (getAsteroidRandomNumber != 1)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                _enemySpawned++;
                CalculateRandomEnemyMovement();
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(5.0f);
        }
        if (_enemySpawned == 10)
        {
            _wave++;
            _uiManager.UpdateWave(_wave);
            _enemySpawned = 0;
            StopCoroutine(SpawnWave1Routine());
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(SpawnWave2Routine());
        }
    }

    IEnumerator SpawnWave2Routine()
    {
        while (_enemySpawned < 20)

        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Vector3 bomberSpawnPos = new Vector3(11, 4, 0);

            int getRandomNumber = (Random.Range(0, 5));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }

            int getBomberRandomNumber = (Random.Range(0, 5));
            if (getBomberRandomNumber == 1)
            {
                Instantiate(_BomberPrefab, bomberSpawnPos, Quaternion.identity);
            }

            if (getRandomNumber != 1)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                _enemySpawned++;
                CalculateRandomEnemyMovement();
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(3.0f);
        }
        if (_enemySpawned == 20)
        {
            _wave++;
            _uiManager.UpdateWave(_wave);
            _enemySpawned = 0;
            StopCoroutine(SpawnWave2Routine());
            yield return new WaitForSeconds(1.9f);
            StartCoroutine(SpawnWave3Routine());
        }
    }

    IEnumerator SpawnWave3Routine()
    {
        while (_enemySpawned < 30)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Vector3 bomberSpawnPos = new Vector3(11, 4, 0);

            int getRandomNumber = (Random.Range(0, 10));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }

            int getBomberRandomNumber = (Random.Range(0, 5));
            if (getBomberRandomNumber == 1)
            {
                Instantiate(_BomberPrefab, bomberSpawnPos, Quaternion.identity);
            }

            if (getRandomNumber != 1)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                _enemySpawned++;
                CalculateRandomEnemyMovement();
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(1.0f);
        }
        if (_enemySpawned == 30)
        {
            _wave++;
            _uiManager.UpdateWave(_wave);
            _enemySpawned = 0;
            StopCoroutine(SpawnWave3Routine());
            _uiManager.GameOverSequence();
        }
    }

    public int CalculateRandomEnemyMovement()
   {
        return Random.Range(0, 5);
   }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
          //  int _RandomNumber = Random.Range(0, 5);

            SpawnMissilePowerUp();
            SpawnPenaltyPowerUp();
            ChooseAPowerup();
           /// Instantiate(powerups[_RandomNumber], posToSpawn, Quaternion.identity);
            Instantiate(powerups[_powerUpToSpawn], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    void SpawnMissilePowerUp()
    {
        int randomMissile = Random.Range(1, 10);

        if (randomMissile == 1)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9, 9), 7, 0);
            Instantiate(_missilePowerUpPrefab, spawnPos, Quaternion.identity);
        }
    }

    void SpawnPenaltyPowerUp()
    {
        int spawnRate;
        spawnRate = Random.Range(1, 5);
        float spawnX = Random.Range(-9, 9);
        float spawnY = Random.Range(-4, 0);
        Vector3 posToSpawn = new Vector3(spawnX, spawnY, 0);
        if (spawnRate == 1)
        {
            Instantiate(_penaltyPrefab, posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}


