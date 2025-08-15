using System.Collections;
using UnityEngine;

public class Minion : Enemy
{
    protected GameObject Boss;
    [SerializeField] protected AudioClip deathExplotionSound;
    [SerializeField] protected ParticleSystem deathExplotion;
    private bool done = false;
    override protected void Start()
    {
        speed = 100;
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        Boss = GameObject.FindWithTag("Boss");
        playerAudio = GetComponent<AudioSource>();

    }
    override protected void Update()
    {
        IsPlayerStillOnPlatfrom();
        IsEnemyStillonPlatform();
        IsBossStillOnPlatfrom();
    }

    private void IsBossStillOnPlatfrom()
    {
        if (Boss == null && !done)
        {
            done = true;
            
            StartCoroutine(RemoveMin());
        }
    }
    IEnumerator RemoveMin()
    { 
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        deathExplotion.Play();
        playerAudio.PlayOneShot(deathExplotionSound);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}