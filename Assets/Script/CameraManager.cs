using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class CameraManager : MonoBehaviour
{
    public WebCamTexture camTexture;
    public RawImage cameraViewImage;
    private int selectedCameraIndex;
    public Texture2D m_LastCameraTexture;
    public RenderTexture renderTexture;
    public ARCameraBackground m_ARCameraBackground;

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
        // Copy the camera background to a RenderTexture
        Graphics.Blit(null, renderTexture, m_ARCameraBackground.material);
        Debug.Log("renderTexture : " + renderTexture);

        Debug.Log("AR Back : "+ m_ARCameraBackground.material);
        // Copy the RenderTexture from GPU to CPU
        var activeRenderTexture = RenderTexture.active;
        Debug.Log("1. m_LastCameraTexture : " + m_LastCameraTexture);
        RenderTexture.active = renderTexture;
        Debug.Log("2. m_LastCameraTexture : " + m_LastCameraTexture);        
        
        // if (m_LastCameraTexture == null)
        m_LastCameraTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, true);
        Debug.Log("2. m_LastCameraTexture : " + m_LastCameraTexture);
        m_LastCameraTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        m_LastCameraTexture.Apply();
        Debug.Log("3. m_LastCameraTexture : " + m_LastCameraTexture);
        RenderTexture.active = activeRenderTexture;

        // WebCamDevice[] devices = WebCamTexture.devices;

        // for (int i = 0; i < devices.Length; i++)
        // {
        //     if(devices[i].isFrontFacing == false)
        //     {
        //         selectedCameraIndex = i;
        //         break;
        //     }
        // }

        // if (selectedCameraIndex >= 0)
        // {
        //     camTexture = new WebCamTexture(devices[selectedCameraIndex].name, 1080, 1920);

        //     camTexture.requestedFPS = 60;
        //     cameraViewImage.texture = camTexture;

        //     camTexture.Play();
        // }
    }
}