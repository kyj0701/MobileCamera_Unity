using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendServer : MonoBehaviour
{
    public static BackendServer instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            // 초기화 성공 시 로직
            Debug.Log("초기화 성공!");
        }
        else
        {
            // 초기화 실패 시 로직
            Debug.LogError("초기화 실패!");
        }
    }

    public static BackendServer Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}
