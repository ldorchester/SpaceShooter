using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _missilePowerUpPrefab;
    [SerializeField] GameObject _asteroidPrefab;
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject _enemyContainer;
    private bool _stopSpawning = false;
    
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);
            int getRandomNumber = (Random.Range(0, 4));
            if (getRandomNumber == 1)
            {
                Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            }
            if (getRandomNumber != 1)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(5.0f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 7, 0);

            int randomMissile = Random.Range(1, 10);
            if (randomMissile == 1)
            {
                Vector3 spawn = new Vector3(Random.Range(-9, 9), 7, 0);
                Instantiate(_missilePowerUpPrefab, spawn, Quaternion.identity);
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


