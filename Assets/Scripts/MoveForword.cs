using Unity.VisualScripting;
using UnityEngine;

public class MoveForword : MonoBehaviour
{
    public float speed;
    public float powerupStrength;
    public float xBounds;
    public float zBounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        if (transform.position.x < -xBounds || transform.position.x > xBounds)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -xBounds || transform.position.z > xBounds)
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
