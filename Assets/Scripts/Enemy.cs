using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float speed = 200;
    protected int stop = 0;
    protected Rigidbody enemyRB;
    protected GameObject player;
    protected GameObject spawnManager;
    protected AudioSource playerAudio;
    protected bool firestTime = true;

    
    [SerializeField] protected AudioClip fireWorks; 
    [SerializeField] protected ParticleSystem celebration;
    


    virtual protected void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerAudio = GetComponent<AudioSource>();
    }


    virtual protected void Update()
    {
        IsPlayerStillOnPlatfrom();
        IsEnemyStillonPlatform();
    }
    


    protected void IsPlayerStillOnPlatfrom()
    {
        if (player != null)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            lookDirection.y = 0;
            enemyRB.AddForce(lookDirection * speed * Time.deltaTime);
        }
        else if (stop == 0)
        {
            enemyRB.linearVelocity = new Vector3(0, enemyRB.linearVelocity.y, 0);
            enemyRB.angularVelocity = Vector3.zero;              //enemyRB.rotation = Quaternion.Euler(0,0,0);
            StartCoroutine(JumpAnimationTimer());
            stop++;
        }
    }
    virtual protected void IsEnemyStillonPlatform()
    {
        if (transform.position.y < -10)
        {
            DataManagement.Instance.AddToScore(50);
            Destroy(gameObject);
        }
    }

    protected float RandomNumGen()
    {
        float ranNub = Random.Range(0, .5f);
        return ranNub;

    }
    IEnumerator JumpAnimationTimer()
    {
        if (firestTime)
        {
            yield return new WaitForSeconds(1);
            firestTime = false;
            stop--;
        }
        else
        {
            yield return new WaitForSeconds(RandomNumGen());
            for (float i = 0; i < 2.6f; i += .01f)
            {
                transform.position = transform.position + new Vector3(0, .01f, 0);
            }
            celebration.Play();
            playerAudio.PlayOneShot(fireWorks);
            yield return new WaitForSeconds(1);
            stop--;
        }
        
    }
}

