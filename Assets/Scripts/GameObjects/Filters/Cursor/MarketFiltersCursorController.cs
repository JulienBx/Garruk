using UnityEngine;

public class MarketFiltersCursorController : MonoBehaviour
{
	
	void OnMouseDrag() 
	{
		int cursorId = System.Convert.ToInt32 (gameObject.name.Substring (6));
		if(cursorId>1)
		{
			NewMarketController.instance.moveCursors (cursorId);
		}
		else
		{
			NewMarketController.instance.moveMinMaxCursor(cursorId);
		}
	}
}

