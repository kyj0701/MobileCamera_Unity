using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }   
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void ActiveImage(GameObject image)
    {
        image.SetActive(true);
    }
}