using UnityEngine;

public class MyGameFiltersToggleController : MonoBehaviour 
{
	
	public bool isActive;

	void Awake()
	{
		this.isActive = false;
	}
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<TextMesh>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		if(!isActive)
		{
			gameObject.transform.GetComponent<TextMesh>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		int cursorId = System.Convert.ToInt32 (gameObject.name.Substring (6));
		newMyGameController.instance.changeToggle (cursorId);
		this.setColor ();
	}
	private void setColor()
	{
		if(isActive)
		{
			gameObject.transform.GetComponent<TextMesh>().color=new Color(155f/255f,220f/255f,1f);
		}
		else
		{
			gameObject.transform.GetComponent<TextMesh>().color=new Color(1f,1f,1f);
		}
	}
	public void setActive(bool value)
	{
		this.isActive = value;
		this.setColor ();

	}
}

