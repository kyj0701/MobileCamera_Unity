using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public Text playerPos;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        // Camera.main.transform.position = transform.position;
        transform.position = Camera.main.transform.position;
        // playerPos.text = " X: " + transform.position.x.ToString("N2") + " Y: " + transform.position.y.ToString("N2") + " Z: " + transform.position.z.ToString("N2");
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3 (0, 0, 0.01f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3 (-0.01f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3 (0, 0, -0.01f);
        } 
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3 (0.01f, 0, 0);
        }
    }
}
