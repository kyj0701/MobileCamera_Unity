using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float qw, qx, qy, qz, tx, ty, tz;

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