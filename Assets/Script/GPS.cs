using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPS : MonoBehaviour {

    public static GPS Instance { set; get; }

    public float latitude;
    public float longitude;
    public float altitude;
    public double UTC;
    public float VDOP;
    public float HDOP;
    public float PDOP;


    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
        {
            Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        
        UpdateLocationInfo();
    }

    public void UpdateLocationInfo()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        altitude = Input.location.lastData.altitude;
        UTC = Input.location.lastData.timestamp;
        VDOP = Input.location.lastData.verticalAccuracy;
        HDOP = Input.location.lastData.horizontalAccuracy;
        PDOP = Mathf.Sqrt(VDOP * VDOP + HDOP * HDOP);
    }
}