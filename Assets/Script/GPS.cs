using System.Collections;
using UnityEngine;
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
    public int maxWait = 5;
    public int delay;

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    void Update()
    {
        // StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
        {
            Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
            while (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
            {
                yield return null;
            }
        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
        }

        Input.location.Start();
        
        while (Input.location.status == LocationServiceStatus.Initializing && delay < maxWait)
        {
            Debug.Log("HERE?");
            yield return new WaitForSeconds(3);
            delay++;
        }

        if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("Unable to determine device location");
        }

        if (delay >= maxWait)
        {
            Debug.Log("Time out");
        }
        
        UpdateLocationInfo();
        // Debug.Log("GPS Update");
        yield return new WaitForSeconds(2);
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