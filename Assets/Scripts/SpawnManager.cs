using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int _enemySpawned;
    //[SerializeField] private int _maxEnemy;
    [SerializeField] private int _wave;
   // [SerializeField] private int _level;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _missilePowerUpPrefab;
    [SerializeField] GameObject _asteroidPrefab;
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject _player;
    private UI_Manager _uiManager;
    private bool _stopSpawning = false;
    public int _randomNumber;
    
    void Start()
    {
        _wave = 1;
        _enemySpawned = 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
       /* if (_wave == 1)
        {
            _maxEnemy = 10;
        }
        if (_wave == 2)
        {
            _maxEnemy = 20;
        }
        if (_wave == 3)
        {
            _maxEnemy = 30;
        }
       */
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(SpawnWave1Routine());
            StartCoroutine(SpawnPowerupRoutine());
        }
    }

  /*  IEnumerator SpawnEnemyRoutine()
     {
         while (_stopSpawning == false)
         {
            for (int i = 0; i < _maxEnemy; i++)
            {
                yield return new WaitForSeconds(_timeToSpawn);

                Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
                int getRandomNumber = (Random.Range(0, 5));
                if (getRandomNumber == 1)
                {
                    Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
                }
                if (getRandomNumber != 1)
                {
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    CalculateRandomEnemyMovement();
                    _enemySpawned++;
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
            }

            if (_enemySpawned >= _maxEnemy)
            {
                _enemySpawned = 0;
                _timeToSpawn -= 2f;
                _maxEnemy = _level;
                _wave++;
                _uiManager.UpdateWave(_wave);
            }
            yield return new WaitForSeconds(_timeToWait);
         }
     }
  */

   IEnumerator SpawnWave1Routine()
    {
        while (_enemySpawned < 10)

        {

            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);

            int getRandomNumber = (Random.Range(0, 5));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }
            if (getRandomNumber != 1)
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

            int getRandomNumber = (Random.Range(0, 5));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
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

            int getRandomNumber = (Random.Range(0, 5));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
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
        return Random.Range(0, 4);
   }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);

            int randomMissile = Random.Range(1, 10);

            if (randomMissile == 1)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-9, 9), 7, 0);
                Instantiate(_missilePowerUpPrefab, spawnPos, Quaternion.identity);
            }

            int randomPowerup = Random.Range(0, 5);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}


