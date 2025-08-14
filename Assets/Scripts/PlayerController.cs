using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public GameObject jukebox;
    private Rigidbody PlayerRB;
    public GameObject laserBeam;
    private GameObject focalPoint;
    public GameObject powerupIndicatorLaser;
    private Vector3 powerupIndicatorLaserOffset = new Vector3(-0.38f, 1f, -0.6f);
    public GameObject powerupIndicatorSlam;
    private Vector3 powerupIndicatorSlamOffset = new Vector3(0.38f, 1f, -0.6f);
    public ParticleSystem explosionParticle;
    public AudioClip laserShot;
    public float laserShotVolume = 1.0f;
    public AudioClip butSlam;
    public float butSlamVolume = 1.0f;
    public AudioClip powerupCollect;
    public float powerupCollectVolume = 1.0f;
    public AudioClip ballCollision;
    public float ballCollisionVolume = 1.0f;
    public AudioClip getBiggerSound;
    public float getBiggerSoundVolume = 1.0f;
    private AudioSource playerAudio;
    public float gravityModifier;
    public float speed = 500;
    public float powerupStrength;
    public bool hasPowerup = false;
    public int numberOfLasers;
    public int nymberOfButSlam;
    public bool gameOver = false;
    private int basePowerupTimer = 7;
    private int big = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Poin");
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            jukebox.GetComponent<Jukebox>().EndGameMusic();
            Destroy(gameObject);
        }

        float forWardInput = Input.GetAxis("Vertical");
        PlayerRB.AddForce(focalPoint.transform.forward * speed * forWardInput * Time.deltaTime);
        powerupIndicatorLaser.transform.position = transform.position + powerupIndicatorLaserOffset;
        powerupIndicatorSlam.transform.position = transform.position + powerupIndicatorSlamOffset;
        ShootLaser();
        TrigerSlam();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup 0"))
        {
            BasePowerup();
            basePowerupTimer = 7;
            big = 2;
            transform.localScale = new Vector3(3, 3, 3);
            powerupIndicatorLaserOffset = powerupIndicatorLaserOffset * 2;
            powerupIndicatorSlamOffset = powerupIndicatorSlamOffset * 2;
            playerAudio.PlayOneShot(getBiggerSound, getBiggerSoundVolume);
            
        }
        else if (other.CompareTag("Powerup 1"))
        {
            nymberOfButSlam++;
            powerupIndicatorSlam.gameObject.SetActive(true);
            playerAudio.PlayOneShot(powerupCollect, powerupCollectVolume);
        }
        else if (other.CompareTag("Powerup 2"))
        {
            numberOfLasers++;
            powerupIndicatorLaser.gameObject.SetActive(true);
            playerAudio.PlayOneShot(powerupCollect, powerupCollectVolume);
        }
        
        
        Destroy(other.gameObject);
    }

    private void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.E) && numberOfLasers > 0)
        {
            StartCoroutine(LaserDelay());
            numberOfLasers--;
            if (numberOfLasers <= 0)
            {
                powerupIndicatorLaser.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator LaserDelay()
    {
        Enemy[] enamyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        float angle;
        Vector3 location;
        for (int i = 0; i < 3; i++)
        {
            for (int ii = 0; ii < enamyCount.Length; ii++)
            {
                if (enamyCount[ii] != null)
                {
                    location = enamyCount[ii].gameObject.transform.position - transform.position;
                    angle = Mathf.Atan2(location.x, location.z) * Mathf.Rad2Deg;
                    float xPos = (float)Math.Sin(Mathf.Deg2Rad * angle) * 1.75f * big;
                    float zPos = (float)Math.Cos(Mathf.Deg2Rad * angle) * 1.75f * big;
                    Instantiate(laserBeam, transform.position + new Vector3(xPos, 0, zPos), Quaternion.Euler(90, angle, 0));
                    playerAudio.PlayOneShot(laserShot, laserShotVolume);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }

    private void BasePowerup()
    {
        hasPowerup = true;
        StartCoroutine(PowerupCountDownRoutine());
    }
    IEnumerator PowerupCountDownRoutine()
    {
        yield return new WaitForSeconds(basePowerupTimer);
        hasPowerup = false;
        big = 1;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        powerupIndicatorLaserOffset = new Vector3(-0.38f, 1f, -0.6f);
        powerupIndicatorSlamOffset = new Vector3(0.38f, 1f, -0.6f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPowerup && (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")))
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            playerAudio.PlayOneShot(ballCollision, ballCollisionVolume);
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            playerAudio.PlayOneShot(ballCollision, ballCollisionVolume);
        }
    }

    private float Pathageria(float B, float C)
    {
        float A;
        A = math.sqrt(math.square(B) + math.square(C));
        return A;
    }

    private void TrigerSlam()
    {
        if (Input.GetKeyDown(KeyCode.Space) && nymberOfButSlam > 0)
        {
            StartCoroutine(JumpAnimationTimer());
        }

    }
    IEnumerator JumpAnimationTimer()
    {
        transform.position = transform.position + new Vector3(0, 2.5f, 0);
        yield return new WaitForSeconds(.1f);
        transform.position = transform.position - new Vector3(0, 2.4f, 0);
        yield return new WaitForSeconds(.04f);
        playerAudio.PlayOneShot(butSlam, butSlamVolume);
        ButSlam();
        explosionParticle.Play();

    }
    private void ButSlam()
    {
        Enemy[] enamyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Vector3 awayFromPlayer;
        float distance;

        for (int i = 0; i < enamyCount.Length; i++)
        {
            Rigidbody enemyRigidBody = enamyCount[i].gameObject.GetComponent<Rigidbody>();
            awayFromPlayer = enamyCount[i].gameObject.transform.position - transform.position;
            distance = Pathageria(awayFromPlayer.x, awayFromPlayer.z);

            if (distance < 3)
            {
                enemyRigidBody.AddForce(awayFromPlayer * 7, ForceMode.Impulse);
            }
            else if (distance >= 3 && distance < 7)
            {
                enemyRigidBody.AddForce(awayFromPlayer * 5, ForceMode.Impulse);
            }
            else if (distance >= 7 && distance < 14)
            {
                enemyRigidBody.AddForce(awayFromPlayer * 3, ForceMode.Impulse);
            }
        }
        nymberOfButSlam--;
        if (nymberOfButSlam <= 0)
        {
            powerupIndicatorSlam.gameObject.SetActive(false);
        }
        
         
    }
    
}
