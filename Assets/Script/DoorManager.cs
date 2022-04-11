
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    public bool keyNeeded = false;              //Is key needed for the door
    public bool gotKey;                  //Has the player acquired key

    private bool playerInZone;                  //Check if the player is in the zone
    private bool doorOpened;                    //Check if door is currently opened or not

    private Animation doorAnim;
    private BoxCollider doorCollider;           //To enable the player to go through the door if door is opened else block him
    public Button door;
    public GameObject open;
    private bool canOpen = false;
    // public GameObject cl;

    enum DoorState
    {
        Closed,
        Opened,
        Jammed
    }

    DoorState doorState = new DoorState();      //To check the current state of the door

    /// <summary>
    /// Initial State of every variables
    /// </summary>
    private void Start()
    {
        gotKey = true;
        doorOpened = false;                     //Is the door currently opened
        playerInZone = false;                   //Player not in zone
        doorState = DoorState.Closed;           //Starting state is door closed


        doorAnim = transform.parent.gameObject.GetComponent<Animation>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider>();

        door.onClick.AddListener(DoorClicked);

        // cl = GameObject.FindGameObjectWithTag("Client");

        //If Key is needed and the KeyGameObject is not assigned, stop playing and throw error
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInZone = true;
        Debug.Log("Enter");
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;
    }

    private void DoorClicked()
    {
        canOpen = true;
    }

    private void Update()
    {
        // Debug.Log(cl.connecting);
        Debug.Log(Client.Instance.connecting);
        if (Client.Instance.connecting == true)
        {
            SceneManager.LoadScene("Main");
        }

        //To Check if the player is in the zone
        if (playerInZone)
        {
            open.SetActive(true);
            if (doorState == DoorState.Opened)
            {
                doorCollider.enabled = false;
            }
            else if (doorState == DoorState.Closed || gotKey)
            {
                doorCollider.enabled = true;
            }
            else if (doorState == DoorState.Jammed)
            {
                doorCollider.enabled = true;
            }
        }
        // else open.SetActive(false);

        if (canOpen && playerInZone)
        {
            doorOpened = !doorOpened;           //The toggle function of door to open/close

            if (doorState == DoorState.Closed && !doorAnim.isPlaying)
            {
                if (!keyNeeded)
                {
                    doorAnim.Play("Door_Open");
                    doorState = DoorState.Opened;
                }
                else if (keyNeeded && !gotKey)
                {
                    if (doorAnim.GetClip("Door_Jam") != null)
                        doorAnim.Play("Door_Jam");
                    doorState = DoorState.Jammed;
                }
            }

            if (doorState == DoorState.Closed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }

            // if (doorState == DoorState.Opened && !doorAnim.isPlaying)
            // {
            //     doorAnim.Play("Door_Close");
            //     doorState = DoorState.Closed;
            // }

            // if (doorState == DoorState.Jammed && !gotKey)
            // {
            //     if (doorAnim.GetClip("Door_Jam") != null)
            //         doorAnim.Play("Door_Jam");
            //     doorState = DoorState.Jammed;
            // }
            // else if (doorState == DoorState.Jammed && gotKey && !doorAnim.isPlaying)
            // {
            //     doorAnim.Play("Door_Open");
            //     doorState = DoorState.Opened;
            // }
        }
    }
}
