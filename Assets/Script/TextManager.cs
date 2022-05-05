using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; set; }
    public Text information;
    public Text cameraPos;
    private bool firstClick;

    private void Start() 
    {
        Instance = this;
        information.text = "Press the Camera button toward the desired location";
        firstClick = false;
    }

    private void Update()
    {
        cameraPos.text = " X: " + Camera.main.transform.position.x.ToString("N2") + " Y: " + Camera.main.transform.position.y.ToString("N2") + " Z: " + Camera.main.transform.position.z.ToString("N2");
    }

    public void ChangeInfo(string info)
    {
        if (!firstClick)
        {
            information.text = info;
            firstClick = true;
        }
    }
}
