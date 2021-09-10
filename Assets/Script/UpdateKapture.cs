using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateKapture : MonoBehaviour
{
    public Text accel;
    public Text gyro;
    public Text pos;

    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKaptureFormat();
    }

    private void UpdateKaptureFormat()
    {
        accel.text = "ACC - " + "X:" + Input.acceleration.x.ToString("N2") + " Y: " + Input.acceleration.y.ToString("N2") + " Z: " + Input.acceleration.z.ToString("N2");
        gyro.text = "GYRO - " + "X:" + Input.gyro.rotationRate.x.ToString("N2") + " Y: " + Input.gyro.rotationRate.y.ToString("N2") + " Z: " + Input.gyro.rotationRate.z.ToString("N2");
        pos.text = " X: " + Camera.main.transform.position.x.ToString("N2") + " Y: " + Camera.main.transform.position.y.ToString("N2") + " Z: " + Camera.main.transform.position.z.ToString("N2");
    }
}
