using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour
{
    public Text latitude_t;
    public Text longitude_t;
    public Text altitude_t;
    public Text UTC_t;
    public Text PDOP_t;
    

    private void Update() 
    {
        latitude_t.text = "Lat : " + GPS.Instance.latitude.ToString();
        longitude_t.text = "Lon : " + GPS.Instance.longitude.ToString();
        altitude_t.text = "Alt : " + GPS.Instance.altitude.ToString();
        UTC_t.text = "UTC : " + GPS.Instance.UTC.ToString();
        PDOP_t.text = "PDOP : " + GPS.Instance.PDOP.ToString();
    }
}
