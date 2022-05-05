using UnityEngine; 
using System.Collections; 

public class TouchObject : MonoBehaviour 
{

	Camera _mainCam = null; 
	private bool _mouseState;
	private GameObject target;
	private Vector3 MousePos;

	private void Awake() 
    {
		_mainCam = Camera.main;
    } 

	private void Update () 
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
			transform.localScale = new Vector3(0.09f, 0.09f, 0.09f);
		}
		else
		{
			transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
