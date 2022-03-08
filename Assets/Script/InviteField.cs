using UnityEngine;
using BackEnd;
using TMPro;

public class InviteField : MonoBehaviour
{
    public TMP_InputField inviteName;

    public void InviteUser()
    {
        Debug.Log(inviteName.text);
        Backend.Match.InviteUser(inviteName.text);
        Backend.Match.OnMatchMakingRoomInvite += (args) => 
        {
            Debug.Log("Invite Success!");
        };
    }
}