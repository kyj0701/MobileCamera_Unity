
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    public bool keyNeeded;              //Is key needed for the door
    public bool gotKey;                  //Has the player acquired key

    private bool playerInZone;                  //Check if the player is in the zone
    private bool doorOpened;                    //Check if door is currently opened or not

    private Animation doorAnim;
    private BoxCollider doorCollider;           //To enable the player to go through the door if door is opened else block him
    private bool canOpen = false;
    public GameObject doorObject;

    Camera _mainCam = null; 
	private bool _mouseState;
	private GameObject target;
	private Vector3 MousePos;

    enum DoorState
    {
        Closed,
        Opened,
        Jammed
    }

    DoorState doorState = new DoorState();      //To check the current state of the door

    private void Awake() 
    {
		_mainCam = Camera.main;
    } 

    /// <summary>
    /// Initial State of every variables
    /// </summary>
    private void Start()
    {
        gotKey = false;
        keyNeeded = true;
        doorOpened = false;                     //Is the door currently opened
        playerInZone = false;                   //Player not in zone
        doorState = DoorState.Closed;           //Starting state is door closed


        doorAnim = transform.parent.gameObject.GetComponent<Animation>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider>();


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

    public void KeyGotted()
    {
        gotKey = true;
        keyNeeded = false;
    }

    private void Update()
    {
        if ( true == Input.GetMouseButtonDown(0)) 
        {
            target = GetClickedObject(); 
			
			if ( gotKey == true && target.Equals(doorObject) == true) 
            {
				_mouseState = true;
                TextManager.Instance.ChangeInfo("Waiting for Seconds...");                
			}

        }
		else if ( true == Input.GetMouseButtonUp(0)) 
        {
            _mouseState = false; 
        }

        //When Client receives traj, we load main scene
        if (Client.Instance.connecting == true)
        {
            doorObject.SetActive(false);
            TextManager.Instance.ChangeInfo(" ");
        }

        //To Check if the player is in the zone
        if (playerInZone)
        {
            // Debug.Log(keyNeeded);
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

        if (true == _mouseState && playerInZone)
        {
            doorOpened = !doorOpened;           //The toggle function of door to open/close

            if (doorState == DoorState.Closed && !doorAnim.isPlaying)
            {
                if (!keyNeeded)
                {
                    // Debug.Log(keyNeeded + " " + gotKey + " Open!");
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
            if (doorState == DoorState.Opened && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Close");
                doorState = DoorState.Closed;
            }
            if (doorState == DoorState.Jammed && !gotKey)
            {
                if (doorAnim.GetClip("Door_Jam") != null)
                    doorAnim.Play("Door_Jam");
                doorState = DoorState.Jammed;
            }
            else if (doorState == DoorState.Jammed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }
        }
    }

    private GameObject GetClickedObject() 
    {
		RaycastHit hit;
		GameObject target = null; 

		Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition); 
		
		if( true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit))) 
		{
			target = hit.collider.gameObject; 
		} 

        return target; 
    } 
}
