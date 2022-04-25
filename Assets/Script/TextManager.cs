using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    // public TextManager instance = null;
    public static TextManager Instance { get; set; }
    public Text information;

    // void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(this.gameObject);
    //     }
    //     else Destroy(this.gameObject);
    // }

    private void Start() 
    {
        Instance = this;
        information.text = "Press the Camera button toward the desired location";
    }

    public void ChangeInfo(string info)
    {
        information.text = info;
    }
}
