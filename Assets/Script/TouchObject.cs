using UnityEngine; 
using System.Collections; 

public class TouchObject : MonoBehaviour 
{

	Camera _mainCam = null; 
	private bool _mouseState;
	private GameObject target;
	private Vector3 MousePos;

	void Awake() 
    {
		_mainCam = Camera.main;
    } 

	void Update () 
    {
        if ( true == Input.GetMouseButtonDown(0)) 
        {
            target = GetClickedObject(); 
			
			if ( true == target.Equals(gameObject)) 
            {
				_mouseState = true; 
			}

        }
		else if ( true == Input.GetMouseButtonUp(0)) 
        {
            _mouseState = false; 
        }

		if (true == _mouseState)
		{
			transform.localScale = new Vector3(9f, 9f, 9f);
		}
		else
		{
			transform.localScale = new Vector3(10f, 10f, 10f);
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
