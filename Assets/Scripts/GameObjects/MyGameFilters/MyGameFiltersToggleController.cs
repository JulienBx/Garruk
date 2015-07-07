using UnityEngine;

public class MyGameFiltersToggleController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<TextMesh>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<TextMesh>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{

	}
}

