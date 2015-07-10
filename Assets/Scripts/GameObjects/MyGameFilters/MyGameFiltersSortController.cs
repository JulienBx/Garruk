using UnityEngine;

public class MyGameFiltersSortController : MonoBehaviour 
{
	public Sprite[] sprites;
	private bool isActive;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
	}
	void OnMouseExit()
	{
		if(!isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		}
	}
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		newMyGameController.instance.changeSort (System.Convert.ToInt32(gameObject.name.Substring (4)));
		this.setSprite ();
	}
	private void setSprite()
	{
		if(isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		}
		else
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		}
	}
	public void setActive(bool value)
	{
		this.isActive = value;
		this.setSprite ();
		
	}
}

