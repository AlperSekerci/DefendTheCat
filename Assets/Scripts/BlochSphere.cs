using UnityEngine;

public class BlochSphere : MonoBehaviour
{
    public const float PI2 = Mathf.PI * 2;
    public float rotateSpeed = 100;
    public float theta = 0;
    public float phi = 0;
    public Transform pointTM;
    public bool rotationOn = true;

    private void Start()
    {
        SetRandomPoint();
    }

    public void SetRandomPoint()
    {
        theta = Random.Range(0, Mathf.PI);
        phi = Random.Range(0, PI2);
        UpdatePoint();
    }

    private void Update()
    {
        if (rotationOn) transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    // Source: http://www.vcpc.univie.ac.at/~ian/hotlist/qc/talks/bloch-sphere.pdf
    public void UpdatePoint()    
    {
        //theta = Mathf.Clamp(theta, 0, Mathf.PI);
        //phi = Mathf.Clamp(phi, 0, PI2);
        Vector3 point = SphericalCoordToRect(theta, phi);
        pointTM.localPosition = point / transform.localScale.x;
    }

    // Source: https://blog.nobel-joergensen.com/2010/10/22/spherical-coordinates-in-unity/#:~:text=Spherical%20coordinate%20system%20is%20an,the%20origin%20to%20the%20point
    private Vector3 SphericalCoordToRect(float theta, float phi)
    {
        float a = Mathf.Cos(phi);      
        return new Vector3(a * Mathf.Sin(theta), a * Mathf.Cos(theta), Mathf.Sin(phi));
    }

    public void OnClick()
    {
        rotationOn = !rotationOn;
    }
}