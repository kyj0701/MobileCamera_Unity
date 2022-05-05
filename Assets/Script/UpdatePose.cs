using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;



public class UpdatePose : MonoBehaviour
{
    // Start is called before the first frame update
    string inDate;
    private void Start()
    {
        string owner_inDate = Backend.UserInDate;
        var bro = Backend.GameData.GetMyData("Position", new Where());
        inDate = bro.GetInDate();
        Debug.Log(inDate);
        Debug.Log("bro : " + bro);
        Debug.Log("bro success : " + bro.GetReturnValuetoJSON());
        if(bro.GetReturnValuetoJSON()["rows"].Count <= 0){
            Param param = GetCurrentParam();
            Backend.GameData.Insert ("Position", param );
        }
    }
    
    static Param GetCurrentParam()
    {
        Param param = new Param();
        param.Add("tx", Camera.main.transform.position.x);
        param.Add("ty", Camera.main.transform.position.y);
        param.Add("tz", Camera.main.transform.position.z);
        param.Add("qw", Camera.main.transform.rotation.w);
        param.Add("qx", Camera.main.transform.rotation.x);
        param.Add("qy", Camera.main.transform.rotation.y);
        param.Add("qz", Camera.main.transform.rotation.z);
        return param;
    }
    // Update is called once per frame

    int count = 0;
    private void FixedUpdate()
    {
        if(count % 15 == 0){
            Param param = GetCurrentParam();
            Backend.GameData.UpdateV2("Position", inDate, Backend.UserInDate, param, ( callback ) =>
            {
                print("dd");
                Debug.Log("mybro---------------------------------------");
            });

            string[] select = {"gamer_Id", "owner_inDate", "tx", "ty", "tz", "qw", "qx", "qy", "qz"};
            Where where = new Where();
            //var otherbro = Backend.GameData.Get("Position", where);
            Backend.GameData.GetMyData("Position", new Where(), otherbro => {
                if (otherbro.IsSuccess() == true)
                {
                    Debug.Log(otherbro);
                    return;
                }
                JsonData gameDataListJson = otherbro.FlattenRows();
                for(int i = 0; i < gameDataListJson.Count; i++){
                    if(gameDataListJson[i]["owner_inDate"].ToString() != Backend.UserInDate){
                        Debug.Log(gameDataListJson[i]["owner_inDate"].ToString() + " ----- qw : " + gameDataListJson[i]["qw"].ToString());

                    }
                }
            });
        }
        //Debug.Log(otherbro);
        count++;
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application Quits!");
        Backend.GameData.DeleteV2 ( "Position", inDate, Backend.UserInDate ); 
    }
}