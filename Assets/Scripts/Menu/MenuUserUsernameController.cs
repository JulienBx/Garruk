using UnityEngine;
using TMPro;

public class MenuUserUsernameController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		gameObject.GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
		gameObject.transform.parent.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
	}
	void OnMouseExit()
	{
		gameObject.GetComponent<TextMeshPro>().color=new Color(228f/255f,228f/255f,228f/255f);
		gameObject.transform.parent.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		MenuController.instance.profileLink ();
	}
}

