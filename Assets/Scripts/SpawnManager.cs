﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] public float enemySpawnTime = 0.5f;
    [SerializeField] public float powerUpSpawnTime;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUpPrefabs;
    [SerializeField] private GameObject _powerUpContainer;
    private bool _stopSpawning = false;

    void Start()
    {
        powerUpSpawnTime = Random.Range(8f, 30f);
      
       // Debug.Log(Random.Range(0, _powerUpPrefabs.Length));

    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy(enemySpawnTime));
        StartCoroutine(SpawnPowerUp(powerUpSpawnTime));
    }
  
    IEnumerator SpawnEnemy(float timeBetweenSpawn)
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {

            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(timeBetweenSpawn);

        }
    }

    IEnumerator SpawnPowerUp(float timeBetweenSpawn)
    {
        yield return new WaitForSeconds(15f);
        while (_stopSpawning == false)
        {
            for (int i=0; i <= _powerUpPrefabs.Length-1; i++)
            {
                GameObject newEnemy = Instantiate(_powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Length)], new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
                newEnemy.transform.parent = _powerUpContainer.transform;
                yield return new WaitForSeconds(timeBetweenSpawn);
            }
            
            

        }

    }

    public void OnPlayerDeath()
    {

        _stopSpawning = true;

    }
}







