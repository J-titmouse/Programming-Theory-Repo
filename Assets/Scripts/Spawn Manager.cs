using System.Diagnostics.CodeAnalysis;
using System.Collections;
using UnityEngine;
using NUnit.Framework.Internal;

public class SpawnManager : MonoBehaviour
{
    //public GameObject enemeyPrefab;
    
    public GameObject[] enemys;
    public GameObject[] powerupPrefab;
    private float spawnRange = 9.0f;
    public int enamyCount;
    public int waveNumber = 1;
    public GameObject player;
    public GameObject jukebox;
    public int numOfMins = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enamyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (enamyCount == 0 && player != null)
        {
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
            StartCoroutine(BossFight());
            jukebox.GetComponent<Jukebox>().BossFight();
        }
        else
        {
            SpawnPowerup();
            for (int i = 0; i < enemyToSpaw; i++)
            {
                enemyNUm = Random.Range(2, 5);
                Instantiate(enemys[enemyNUm], GenerateSpawnPosition(), enemys[enemyNUm].transform.rotation);
            }
            jukebox.GetComponent<Jukebox>().switchTracks = true;
        }

    }
    private void SpawnPowerup()
    {
        int powerupenemyNUm = Random.Range(0, 3);
        Instantiate(powerupPrefab[powerupenemyNUm], GenerateSpawnPosition(), powerupPrefab[powerupenemyNUm].transform.rotation);
    }

    IEnumerator BossFight()
    {
        while (GameObject.FindGameObjectsWithTag("Boss").Length > 0 && player != null)
        {
            SpawnPowerup();
            SpawnMins();
            yield return new WaitForSeconds(5);
        }
    }
    private void SpawnMins()
    {
        for (int i = 0; i <= numOfMins; i++)
        {
            Instantiate(enemys[1], GenerateSpawnPosition(), enemys[1].transform.rotation);
        }
        numOfMins++;
    }
}
