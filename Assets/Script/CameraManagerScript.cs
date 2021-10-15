using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManagerScript : MonoBehaviour
{
    public WebCamTexture camTexture;

    public RawImage cameraViewImage;

    private int selectedCameraIndex;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No Camera!");
            return;
        }
    }

    void Start() 
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        for (int i = 0; i < devices.Length; i++)
        {
            if(devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }
        }

        if (selectedCameraIndex >= 0)
        {
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name, 1080, 1920);

            camTexture.requestedFPS = 60;

            cameraViewImage.texture = camTexture;

            camTexture.Play();
        }
    }
}