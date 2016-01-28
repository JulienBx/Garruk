using UnityEngine;
using TMPro;

public class SelectPicturePopUpPictureController : MonoBehaviour 
{
	
	public int Id;
	private bool isActive;

	void OnMouseOver()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(!isActive && !ApplicationDesignRules.isMobileScreen)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<SelectPicturePopUpController> ().selectPicture (this.Id);
	}
	public void setActive(bool value)
	{
		this.isActive = value;
		if(value)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		}
		else
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
}

