using UnityEngine;

public class OldPaginationController : MonoBehaviour 
{
	public bool isActive;
	public int id;
	private PaginationRessources ressources;
	
	void Awake()
	{
		this.isActive = false;
		this.ressources = this.gameObject.GetComponent<PaginationRessources> ();
	}
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = ressources.sprites [1];
	}
	void OnMouseExit()
	{
		if(!isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = ressources.sprites [0];
		}
	}
	public void setSprite()
	{
		if(isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = ressources.sprites [1];
		}
		else
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = ressources.sprites [0];
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

