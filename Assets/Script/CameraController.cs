using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
        // transform.position = new Vector3((GPS.Instance.longitude * 10000) % 100, (GPS.Instance.latitude * 10000) % 100, GPS.Instance.altitude);
    }
}
