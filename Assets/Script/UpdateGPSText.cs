using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour
{
    public Text latitude_t;
    public Text longitude_t;
    public Text altitude_t;
    public Text UTC_t;

    private void Update() 
    {
        latitude_t.text = "Lat : " + GPS.Instance.latitude.ToString();
        longitude_t.text = "Lon : " + GPS.Instance.longitude.ToString();
        altitude_t.text = "Lon : " + GPS.Instance.altitude.ToString();
        UTC_t.text = "UTC : " + GPS.Instance.UTC.ToString();
    }
}
