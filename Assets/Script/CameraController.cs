using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject camParent;
    private float qw, qx, qy, qz, tx, ty, tz;

    void Start()
    {
        // Input.gyro.enabled = true;
        // camParent = new GameObject("CamParent");
        // camParent.transform.position = this.transform.position;
        // this.transform.parent = camParent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = new Vector3((GPS.Instance.longitude), (GPS.Instance.latitude), GPS.Instance.altitude);
        // transform.rotation = Quaternion.Euler(new Vector3())\

        // camParent.transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
        // this.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);
    }

    public void UpdateLocation()
    {
        qw = Client.Instance.qw;
        qx = Client.Instance.qx;
        qy = Client.Instance.qy;
        qz = Client.Instance.qz;
        tx = Client.Instance.tx;
        ty = Client.Instance.ty;
        tz = Client.Instance.tz;
    }
}