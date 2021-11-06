using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject camParent;
    void Start()
    {
        Input.gyro.enabled = true;
        camParent = new GameObject("CamParent");
        camParent.transform.position = this.transform.position;
        this.transform.parent = camParent.transform;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = new Vector3((GPS.Instance.longitude * 10000) % 100, (GPS.Instance.latitude * 10000) % 100, GPS.Instance.altitude);
        camParent.transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
        this.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);
    }
}
