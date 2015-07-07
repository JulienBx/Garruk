using UnityEngine;

public class MenuButtonController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		newMenuController.instance.moveButton (System.Convert.ToInt32(gameObject.name.Substring (6)));
	}
	void OnMouseDown()
	{
		newMenuController.instance.changePage (System.Convert.ToInt32(gameObject.name.Substring (6)));
	}
	
}

