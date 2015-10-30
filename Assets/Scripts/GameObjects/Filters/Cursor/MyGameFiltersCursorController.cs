using UnityEngine;

public class MyGameFiltersCursorController : MonoBehaviour
{

	void OnMouseDrag() 
	{
		int cursorId = System.Convert.ToInt32 (gameObject.name.Substring (6));
		//newMyGameController.instance.moveCursors (cursorId);
	}
}

