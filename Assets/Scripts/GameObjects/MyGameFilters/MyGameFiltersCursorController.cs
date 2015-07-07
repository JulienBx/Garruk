using UnityEngine;

public class MyGameFiltersCursorController : MonoBehaviour 
{
	void OnMouseDrag() 
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		int cursorId = System.Convert.ToInt32 (gameObject.name.Substring (6));
		newMyGameController.instance.moveCursor (mousePosition.x, cursorId);
	}
}

