using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    public Image off;
    public Image on;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Off() 
    {
        Client.Instance.toggle = 0;
        off.gameObject.SetActive(true);
        on.gameObject.SetActive(false);
    }

    public void On()
    {
        Client.Instance.toggle = 1;
        on.gameObject.SetActive(true);
        off.gameObject.SetActive(false);
    }
}
