using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour
{
    public Text UTC_t;
    public Text PDOP_t;    

    private void Update() 
    {
        UTC_t.text = "UTC : " + GPS.Instance.UTC.ToString();
        PDOP_t.text = "PDOP : " + GPS.Instance.PDOP.ToString();
    }
}
