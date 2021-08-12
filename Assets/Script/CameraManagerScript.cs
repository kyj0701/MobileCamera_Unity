using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManagerScript : MonoBehaviour
{
    WebCamTexture camTexture;

    public RawImage cameraViewImage;

    private int selectedCameraIndex;

    // Start is called before the first frame update
    void Start()
    {
        selectedCameraIndex = -1;
    }

    public void CameraOn()
    {
        if (selectedCameraIndex == -1)
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
                camTexture = new WebCamTexture(devices[selectedCameraIndex].name);

                camTexture.requestedFPS = 60;

                cameraViewImage.texture = camTexture;

                camTexture.Play();
            }
        }
    }

    public void CameraOff()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
            WebCamTexture.Destroy(camTexture);
            camTexture = null;
        }
        selectedCameraIndex = -1;
    }
}
