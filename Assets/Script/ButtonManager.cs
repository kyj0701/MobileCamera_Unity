using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Camera Debugging");
            Permission.RequestUserPermission(Permission.Camera);
            Debug.Log(Permission.HasUserAuthorizedPermission(Permission.Camera));
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
