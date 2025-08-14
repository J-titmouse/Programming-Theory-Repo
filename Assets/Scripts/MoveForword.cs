using UnityEngine;

public class MoveForword : MonoBehaviour
{
    private float speed = 50;
    private float powerupStrength = 15;
    private float bounds = 20;

    void Update()
    {
        Move();
    }



    
    private void Move()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        if (transform.position.x < -bounds || transform.position.x > bounds)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -bounds || transform.position.z > bounds)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            Rigidbody enemyRigidBody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
