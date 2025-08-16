using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody PlayerRB;
    private AudioSource playerAudio;
    private Vector3 powerupIndicatorLaserOffset = new Vector3(-0.38f, 1f, -0.6f);
    private Vector3 powerupIndicatorSlamOffset = new Vector3(0.38f, 1f, -0.6f);
    private float gravityModifier = 2;
    private float speed = 500;
    private float powerupStrength = 20;
    private bool hasPowerup = false;
    private int basePowerupTimer = 7;
    private int big = 1;
    private int numberOfLasers;
    private int nymberOfButSlam;

    [SerializeField] private GameObject jukebox;
    [SerializeField] private  GameObject laserBeam;
    [SerializeField] private  GameObject focalPoint;
    [SerializeField] private  GameObject powerupIndicatorLaser;
    [SerializeField] private GameObject powerupIndicatorSlam;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private GameObject MainUIHandler;

    
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
        Move();
    }

    private void Move()
    { 
        if (transform.position.y < -10)
        {
            jukebox.GetComponent<Jukebox>().EndGameMusic();
            MainUIHandler.GetComponent<MainUIHandler>().GameOver();
            DataManagement.Instance.newentry();
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
            playerAudio.PlayOneShot(audioClips[4]);

        }
        else if (other.CompareTag("Powerup 1"))
        {
            nymberOfButSlam++;
            powerupIndicatorSlam.gameObject.SetActive(true);
            playerAudio.PlayOneShot(audioClips[2]);
        }
        else if (other.CompareTag("Powerup 2"))
        {
            numberOfLasers++;
            powerupIndicatorLaser.gameObject.SetActive(true);
            playerAudio.PlayOneShot(audioClips[2]);
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
                    playerAudio.PlayOneShot(audioClips[0]);
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
            playerAudio.PlayOneShot(audioClips[3]);
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            playerAudio.PlayOneShot(audioClips[3]);
        }
        DataManagement.Instance.AddToScore(1);
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
        playerAudio.PlayOneShot(audioClips[1]);
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
