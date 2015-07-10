using UnityEngine;

public class MyGamePaginationController : MonoBehaviour 
{
	private bool isActive;
	public Sprite[] sprites;
	private int id;
	
	void Awake()
	{
		this.isActive = false;
	}
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
		this.setSprite ();
		newMyGameController.instance.paginationHandler (this.id);

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
	public void setId(int value)
	{
		this.id = value;
	}
}

