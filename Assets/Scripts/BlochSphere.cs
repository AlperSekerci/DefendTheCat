using UnityEngine;

public class BlochSphere : MonoBehaviour
{
    public float rotateSpeed = 100;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}