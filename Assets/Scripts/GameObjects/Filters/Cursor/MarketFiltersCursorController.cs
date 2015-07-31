using UnityEngine;

public class MarketFiltersCursorController : MonoBehaviour
{
	
	void OnMouseDrag() 
	{
		int cursorId = System.Convert.ToInt32 (gameObject.name.Substring (6));
		NewMarketController.instance.moveMinMaxCursor (cursorId);
	}
}

