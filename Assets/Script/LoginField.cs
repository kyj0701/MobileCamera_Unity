using UnityEngine;
using UnityEngine.SceneManagement;
using BackEnd;
using BackEnd.Tcp;
using TMPro;

public class LoginField : MonoBehaviour
{
    void Update()
    {
        Backend.Match.Poll();
    }

    public void Guest()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin( "게스트 로그인으로 로그인함" );
        if(bro.IsSuccess())
        {
            Debug.Log("게스트 로그인에 성공했습니다");
            SceneManager.LoadScene("Loading");
        }
        else
        {
            Debug.Log("게스트 로그인에 실패했습니다");
            Backend.BMember.DeleteGuestInfo( );
        }
    }

    public void AutoLogin()
    {
        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();
        if(bro.IsSuccess())
        {
            Debug.Log("자동 로그인에 성공했습니다");
            SceneManager.LoadScene("Loading");
        }
        else
        {
            Debug.Log("자동 로그인에 실패했습니다");
            Guest();
        }
    }
}
