using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRange = 9.0f;
    private int enamyCount;
    private int waveNumber = 1;
    private int numOfMins = 0;

    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject[] powerupPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject MainUIHandler;


    void Update()
    {
        ISThereEnemysLeft();
    }

    private void ISThereEnemysLeft()
    {
        enamyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (enamyCount == 0 && player != null)
        {
            MainUIHandler.GetComponent<MainUIHandler>().IncrementRound();
            SpawnEnemyWave(waveNumber);
            waveNumber++;
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosY = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosY);

        return spawnPos;
    }

    private void SpawnEnemyWave(int enemyToSpaw)
    {
        int enemyNUm;
        if (enemyToSpaw % 5 == 0)
        {
            Instantiate(enemys[0], GenerateSpawnPosition(), enemys[0].transform.rotation);
            
        }
        else
        {
            SpawnPowerup();
            for (int i = 0; i < enemyToSpaw; i++)
            {
                enemyNUm = Random.Range(2, 5);
                Instantiate(enemys[enemyNUm], GenerateSpawnPosition(), enemys[enemyNUm].transform.rotation);
            }
        }

    }
    public void SpawnPowerup()
    {
        int powerupenemyNUm = Random.Range(0, 3);
        Instantiate(powerupPrefab[powerupenemyNUm], GenerateSpawnPosition(), powerupPrefab[powerupenemyNUm].transform.rotation);
    }

    public void SpawnMins()
    {
        for (int i = 0; i <= numOfMins; i++)
        {
            Instantiate(enemys[1], GenerateSpawnPosition(), enemys[1].transform.rotation);
        }
        numOfMins++;
    }
}
