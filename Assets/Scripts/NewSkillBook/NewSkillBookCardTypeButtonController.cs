using UnityEngine;
using TMPro;

public class NewSkillBookCardTypeButtonController : MonoBehaviour 
{
	
	public int id;
	private bool isActive;

	void Awake()
	{
		this.isActive = false;
	}
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
	}
	void OnMouseExit()
	{
		if(isActive)
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
		else
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(189f/255f,189f/255f,189f/255f);
		}
	}
	void OnMouseDown()
	{
		NewSkillBookController.instance.selectCardTypeHandler (this.id);
	}
	public void setActive(bool value)
	{
		this.isActive = value;
	}
}

