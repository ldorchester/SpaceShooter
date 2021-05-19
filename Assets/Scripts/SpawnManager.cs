using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int _enemySpawned;
    [SerializeField] private int _bossFightersSpawned;
    [SerializeField] private int _wave;
    private int _weightedTotal;
    private int _powerUpToSpawn;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _bossFightersPrefab;
    [SerializeField] GameObject _bottomEnemyPrefab;
    [SerializeField] GameObject _missilePowerUpPrefab;
    [SerializeField] GameObject _asteroidPrefab;
    [SerializeField] GameObject _BomberPrefab;
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject _penaltyPrefab;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _bossPrefab;
    private UI_Manager _uiManager;
    private bool _stopSpawning;
    [SerializeField] private bool _bossCreated;
    [SerializeField] private bool _bossAlive;


    void Start()
    {
        _wave = 1;
        _bossCreated = false;
        _bossAlive = false;
        _enemySpawned = 0;
        _bossFightersSpawned = 0;
        _stopSpawning = true;
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_player == null)
        {
            Debug.LogError("The player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        _stopSpawning = false;
        StartCoroutine(SpawnWave1Routine());
        StartCoroutine(SpawnPowerupRoutine());
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
        while (_enemySpawned < 3 && _stopSpawning == false)

        {

            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Vector3 bomberSpawnPos = new Vector3(11, 4, 0);
            Vector3 bottomEnemySpawnPos = new Vector3(-9, 8, 0);

            int getbottomEnemyRandomNumber = (Random.Range(1, 12));
            if (getbottomEnemyRandomNumber == 1)
            {
                Instantiate(_bottomEnemyPrefab, bottomEnemySpawnPos, Quaternion.identity);
                _enemySpawned++;
            }

            int getAsteroidRandomNumber = (Random.Range(1, 10));
            if (getAsteroidRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }

            int getBomberRandomNumber = (Random.Range(1, 8));
            if (getBomberRandomNumber == 1)
            {
                Instantiate(_BomberPrefab, bomberSpawnPos, Quaternion.identity);
                _enemySpawned++;
            }

            if (getAsteroidRandomNumber != 1)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                CalculateRandomEnemyMovement();
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemySpawned++;
            }
            yield return new WaitForSeconds(7.0f);
        }
        if (_enemySpawned >= 3)
        {
            StopWave();
            _wave++;
            _enemySpawned = 0;
           // StopCoroutine(SpawnWave1Routine());
           // StopCoroutine(SpawnPowerupRoutine());
            _uiManager.UpdateWave(_wave);
            yield return new WaitForSeconds(4.0f);
            StartCoroutine(SpawnWave2Routine());
        }
    }

    IEnumerator SpawnWave2Routine()
    {
        _stopSpawning = false;
        StartCoroutine(SpawnPowerupRoutine());
        while (_enemySpawned <= 3 && _stopSpawning == false)

        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Vector3 bomberSpawnPos = new Vector3(11, 4, 0);
            Vector3 bottomEnemySpawnPos = new Vector3(-9, 8, 0);

            int getbottomEnemyRandomNumber = (Random.Range(1, 12));
            if (getbottomEnemyRandomNumber == 1)
            {
                Instantiate(_bottomEnemyPrefab, bottomEnemySpawnPos, Quaternion.identity);
            }

            int getRandomNumber = (Random.Range(1, 10));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }

            int getBomberRandomNumber = (Random.Range(1, 8));
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
            yield return new WaitForSeconds(4.0f);
        }
        if (_enemySpawned >= 3)
        {
            _wave++;
            _uiManager.UpdateWave(_wave);
            _enemySpawned = 0;
            StopWave();
            StopCoroutine(SpawnWave2Routine());
            yield return new WaitForSeconds(2f);
            StartCoroutine(SpawnWave3Routine());
        }
    }

    IEnumerator SpawnWave3Routine()
    {
        _stopSpawning = false;
        while (_enemySpawned < 3 && _stopSpawning == false)
        {
            StartCoroutine(SpawnPowerupRoutine());
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Vector3 bomberSpawnPos = new Vector3(11, 4, 0);
            Vector3 bottomEnemySpawnPos = new Vector3(-9, 8, 0);

            int getbottomEnemyRandomNumber = (Random.Range(1, 12));
            if (getbottomEnemyRandomNumber == 1)
            {
                Instantiate(_bottomEnemyPrefab, bottomEnemySpawnPos, Quaternion.identity);
            }

            int getRandomNumber = (Random.Range(1, 10));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }

            int getBomberRandomNumber = (Random.Range(1, 8));
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
            yield return new WaitForSeconds(4.0f);
        }
        if (_enemySpawned >= 3)
        {
            _uiManager.UpdateBossWave();
            _enemySpawned = 0;
            StopWave();
            StopCoroutine(SpawnWave3Routine());
            yield return new WaitForSeconds(3f);
            StartCoroutine(SpawnWaveBossStage());
        }
    }

 /*  private void SpawnWaveBossStage()
    {
        Vector3 bossFightersPosToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
        Vector3 bossPosToSpawn = new Vector3(0, 10, 0);
        if (_bossCreated == false)
        {
            Instantiate(_bossPrefab, bossPosToSpawn, Quaternion.identity);
            _bossAlive = true;
            _bossCreated = true;
        }
        /* for (int i = 0; i >= 9; i++)
         {
             GameObject newEnemy = Instantiate(_bossFightersPrefab, bossFightersPosToSpawn, Quaternion.identity);
             // _enemySpawned++;
             newEnemy.transform.parent = _enemyContainer.transform;
         }
        
        while (_bossFightersSpawned <= 9)
         {
             Instantiate(_bossFightersPrefab, bossFightersPosToSpawn, Quaternion.identity);
             _bossFightersSpawned++;
            break;
         }
    }
 */
  

    IEnumerator SpawnWaveBossStage()
    {
        StartCoroutine(SpawnPowerupRoutine());
        Vector3 bossPosToSpawn = new Vector3(0, 10, 0);
        if (_bossCreated == false)
        {
            Instantiate(_bossPrefab, bossPosToSpawn, Quaternion.identity);
            _bossAlive = true;
            _bossCreated = true;
        }
        while (_bossAlive == true && _bossFightersSpawned < 5)
        {
            Vector3 bossFightersPosToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            Instantiate(_bossFightersPrefab, bossFightersPosToSpawn, Quaternion.identity);
            _bossFightersSpawned++;
            yield return new WaitForSeconds(2f);
        }
        if (_bossAlive == false)
        {
            StopCoroutine(SpawnWaveBossStage());
            StopCoroutine(SpawnPowerupRoutine());
            StopWave();
            _uiManager.GameOverSequence();
        }
       
    }


    void StopWave()
    {
        _stopSpawning = true;

        var powerup = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (var PowerUp in powerup)
        {
            Destroy(PowerUp);
        }
        var enemyshootup = GameObject.FindGameObjectsWithTag("EnemyShootUp");
        foreach (var EnemyShootUp in enemyshootup)
        {
            Destroy(EnemyShootUp);
        }
        var bomber = GameObject.FindGameObjectsWithTag("Bomber");
        foreach (var Bomber in bomber)
        {
            Destroy(Bomber);
        }
        var enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var Enemy in enemy)
        {
            Destroy(Enemy);
        }
    }

    public int CalculateRandomEnemyMovement()
   {
        return Random.Range(0, 5);
   }

    IEnumerator SpawnPowerupRoutine()
    {
        _stopSpawning = false;
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
        _stopSpawning = false;
        int randomMissile = Random.Range(1, 10);

        if (randomMissile == 1 && _stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9, 9), 7, 0);
            Instantiate(_missilePowerUpPrefab, spawnPos, Quaternion.identity);
        }
    }

    void SpawnPenaltyPowerUp()
    {
        _stopSpawning = false;
        int spawnRate;
        spawnRate = Random.Range(1, 5);
        float spawnX = Random.Range(-9, 9);
        float spawnY = Random.Range(-4, 0);
        Vector3 posToSpawn = new Vector3(spawnX, spawnY, 0);
        if (spawnRate == 1 && _stopSpawning == false)
        {
            Instantiate(_penaltyPrefab, posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}


