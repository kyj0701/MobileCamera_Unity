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
    private void Awake()
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

    public void ARCamera()
    {
        Graphics.Blit(null, renderTexture, m_ARCameraBackground.material);
        // Copy the RenderTexture from GPU to CPU
        var activeRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture; 
        if (m_LastCameraTexture == null)
            m_LastCameraTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, true);
        m_LastCameraTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        m_LastCameraTexture.Apply();
        RenderTexture.active = activeRenderTexture;
    }
}