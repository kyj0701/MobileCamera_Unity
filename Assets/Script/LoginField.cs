using UnityEngine;
using UnityEngine.SceneManagement;
using BackEnd;
using BackEnd.Tcp;
using TMPro;

public class LoginField : MonoBehaviour
{
    

    public TMP_InputField idInput;
    public TMP_InputField pwInput;

    void Update()
    {
        Backend.Match.Poll();
    }

    public void SignUp()
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp ( idInput.text , pwInput.text );
        if(bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다");
        }
        else Debug.Log("회원가입에 실패했습니다");
    }

    public void SignIn()
    {
        string ID = idInput.text;
        string PW = pwInput.text;
        BackendReturnObject bro = Backend.BMember.CustomLogin( ID , PW );
        if(bro.IsSuccess())
        {
            Debug.Log("로그인에 성공했습니다");
            // more
            GameManager.Instance.playerID = ID;
            MatchingSystem();
            SceneManager.LoadScene("Main");
        }
        else
        { 
            Debug.Log("로그인에 실패했습니다");
            Debug.Log(bro);
        }    
    }

    public void Guest()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin( "게스트 로그인으로 로그인함" );
        if(bro.IsSuccess())
        {
            Debug.Log("게스트 로그인에 성공했습니다");
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.Log("게스트 로그인에 실패했습니다");
            Backend.BMember.DeleteGuestInfo( );
            Guest();
        }
    }

    public void AutoLogin()
    {
        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();
        if(bro.IsSuccess())
        {
            Debug.Log("자동 로그인에 성공했습니다");
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.Log("자동 로그인에 실패했습니다");
            Guest();
        }
    }

    private void MatchingSystem()
    {
        ErrorInfo errorInfo;
        Backend.Match.JoinMatchMakingServer(out errorInfo);
        // Debug.Log(errorInfo);
        Backend.Match.OnJoinMatchMakingServer += (args) => 
        {
            // TODO
            // Debug.Log("Success!");
            if (idInput.text == "yejun283")
            {
                Backend.Match.CreateMatchRoom();
                Debug.Log("Make Room Success!");
            }
        };
    }    
}
