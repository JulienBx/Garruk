using UnityEngine;

public class MenuAdminController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
	}
	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f, 1f);
	}
	void OnMouseDown()
	{
		MenuController.instance.adminBoardLink ();
	}
}

