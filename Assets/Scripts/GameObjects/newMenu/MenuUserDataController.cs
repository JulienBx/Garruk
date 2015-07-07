using UnityEngine;

public class MenuUserDataController : MonoBehaviour 
{
	public Sprite[] sprites;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		gameObject.transform.FindChild("Username").GetComponent<TextMesh>().color=new Color(155f/255f,220f/255f,1f);
		gameObject.transform.FindChild("Credits").GetComponent<TextMesh>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		gameObject.transform.FindChild("Username").GetComponent<TextMesh>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Credits").GetComponent<TextMesh>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		newMenuController.instance.profileLink ();
	}
}

