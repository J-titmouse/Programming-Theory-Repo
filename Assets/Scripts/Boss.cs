using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    protected GameObject jukebox;
    override protected void Start()
    {
        speed = 500;
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerAudio = GetComponent<AudioSource>();
        jukebox = GameObject.Find("Jukebox");
        spawnManager = GameObject.Find("Spawn Manager");
        jukebox.GetComponent<Jukebox>().BossFight();
        StartCoroutine(BossFight());
    }
    override protected void Update()
    {
        IsPlayerStillOnPlatfrom();
        IsEnemyStillonPlatform();
    }

    override protected void IsEnemyStillonPlatform()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
            jukebox.GetComponent<Jukebox>().switchTracks = true;
        }
    }
    IEnumerator BossFight()
    {
        while (GameObject.FindGameObjectsWithTag("Boss").Length > 0 && player != null)
        {
            spawnManager.GetComponent<SpawnManager>().SpawnPowerup();
            spawnManager.GetComponent<SpawnManager>().SpawnMins();
            yield return new WaitForSeconds(5);
        }
    }
    
}
