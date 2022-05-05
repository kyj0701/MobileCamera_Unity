using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public string playerID;
    private int count;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }  
        else Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            count++;
            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1.0f);
       
        }
        else if (count == 2)
        {
            CancelInvoke("DoubleClick");
            Application.Quit();
        }
    }

    private void DoubleClick()
    {
        count = 0;
    }

    public static GameManager Instance
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