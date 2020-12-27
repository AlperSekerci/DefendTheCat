using UnityEngine;

public class BlochSphere : MonoBehaviour
{
    public const float PI2 = Mathf.PI * 2;
    public float rotateSpeed = 100;
    public float theta = 0;
    public float phi = 0;
    public Transform pointTM;
    public bool rotationOn = true;
    public Vector3 defaultAngle = new Vector3(0, -90, 0);
    private float iniTheta; // ini = initial
    private float iniPhi;

    private void Start()
    {
        iniTheta = theta;
        iniPhi = phi;
        //SetRandomPoint();
        UpdatePoint();
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

    public void Reset()
    {
        theta = iniTheta;
        phi = iniPhi;
        UpdatePoint();
    }

    // Source: http://www.vcpc.univie.ac.at/~ian/hotlist/qc/talks/bloch-sphere.pdf
    public void UpdatePoint()    
    {   
        Vector3 point = SphericalCoordToRect(theta, phi);        
        pointTM.localPosition = point / transform.localScale.x;
    }

    private Vector3 SphericalCoordToRect(float theta, float phi)
    {
        float thetaDeg = Mathf.Rad2Deg * theta;
        float phiDeg = Mathf.Rad2Deg * phi;
        return Quaternion.Euler(0, -phiDeg, -thetaDeg) * Vector3.up;
    }

    public void OnClick()
    {
        rotationOn = !rotationOn;
        transform.eulerAngles = defaultAngle;
    }
}