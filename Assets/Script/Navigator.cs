using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    public List<GameObject> destination;
    private Vector3 dest_pos;
    private Vector3 navi_pos;
    private Vector3 to_dest;
    public Text dest_text;

    void Update()
    {
        if (dest_text.text == "coffee machine") dest_pos = destination[0].transform.position;
        else if (dest_text.text == "multi space") dest_pos = destination[1].transform.position;
        else dest_pos = new Vector3(0,0,0);
        transform.position = Camera.main.transform.position;
        navi_pos = transform.position;

        dest_pos.y = 0.0f;
        navi_pos.y = 0.0f;
        to_dest = dest_pos - navi_pos;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, to_dest);
    }
}
