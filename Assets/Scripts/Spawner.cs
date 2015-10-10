﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemy;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    Wave currentWave;
    int currentWaveNumber;
    void Start() {
        NextWave();
    }
    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }
    void OnEnemyDeath() {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0) {
            NextWave();
        }
    }

    void NextWave() {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            print("wave:" + currentWaveNumber);
            currentWave = waves[currentWaveNumber - 1];
       
        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }
    [System.Serializable]
    public class Wave {
        
        public int enemyCount;
        public float timeBetweenSpawns;

    }
}
