using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed = 200;
    private int stop = 0;
    private Rigidbody enemyRB;
    private GameObject player;
    public ParticleSystem celebration;
    private bool firestTime = true;
    private AudioSource playerAudio;
    public AudioClip fireWorks;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
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
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
    /*
        IEnumerator JumpAnimationTimer()
        {
            yield return new WaitForSeconds(RandomNumGen());
            transform.position = transform.position + new Vector3(0, 2.5f, 0);
            celebration.Play();
            yield return new WaitForSeconds(1);
            stop--;
        }
     */
    private float RandomNumGen()
    {
        float ranNub = UnityEngine.Random.Range(0, .5f);
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

