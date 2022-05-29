using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDest : MonoBehaviour
{
    private List<string> mapList = new List<string>();
    public List<Text> mapListInBtn = new List<Text>(3);
    public GameObject scrollBar;
    public Text mapBtnText;
    public List<GameObject> dest_list;

    // Start is called before the first frame update
    void Start()
    {
        mapList.Add("coffee machine");
        mapList.Add("multi space");
        mapList.Add("--");

        for (int i = 0; i < mapListInBtn.Count; i++) {
            mapListInBtn[i].text = mapList[i];
        }
    }

    public void ActiveMapList() {
        if (scrollBar.activeSelf == true) scrollBar.SetActive(false);
        else scrollBar.SetActive(true);
    }

    public void Select(Text mapName) {
        mapBtnText.text = mapName.text;
        scrollBar.SetActive(false);
        if (mapBtnText.text == "coffee machine") 
        { 
            dest_list[0].SetActive(true);
            dest_list[1].SetActive(false);
        }
        else if (mapBtnText.text == "multi space") 
        {
            dest_list[0].SetActive(false);
            dest_list[1].SetActive(true);
        }
        else 
        {
            dest_list[0].SetActive(false);
            dest_list[1].SetActive(false);
        }
    }
}
