using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }   
    }
}