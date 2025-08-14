using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 100;
    void Update()
    {
        Roata();
    }

    private void Roata()
    { 
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
