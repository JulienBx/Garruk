using UnityEngine;

public class MenuDisconnectController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
	}
	void OnMouseDown()
	{
		newMenuController.instance.logOutLink ();
	}
}

