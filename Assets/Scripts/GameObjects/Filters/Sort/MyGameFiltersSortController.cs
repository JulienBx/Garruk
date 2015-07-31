using UnityEngine;

public class MyGameFiltersSortController : MonoBehaviour 
{
	private bool isActive;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		if(!isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		newMyGameController.instance.changeSort (System.Convert.ToInt32(gameObject.name.Substring (4)));
		this.setColor ();
	}
	private void setColor()
	{
		if(isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().color=new Color(155f/255f,220f/255f,1f);
		}
		else
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().color=new Color(1f,1f,1f);
		}
	}
	public void setActive(bool value)
	{
		this.isActive = value;
		this.setColor ();
		
	}
}

