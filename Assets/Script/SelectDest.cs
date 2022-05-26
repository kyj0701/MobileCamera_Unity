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
    public GameObject navigator;

    // Start is called before the first frame update
    void Start()
    {
        mapList.Add("coffee machine");
        mapList.Add("--");
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
        navigator.SetActive(true);
    }
}
