using UnityEngine;

public class MenuNotificationsController : MonoBehaviour 
{

	private bool isActive;

	void OnMouseOver()
	{
		if(this.isActive)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
		}
	}
	void OnMouseExit()
	{
		if(this.isActive)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f, 1f);
		}
	}
	void OnMouseDown()
	{
		MenuController.instance.homePageLink ();
	}
	public void setIsActive(bool value)
	{
		this.isActive = value;
		if (value) 
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f, 1f);
		}
		else 
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(54f/255f,54f/255f, 54f/255f);
		}
	}
}

