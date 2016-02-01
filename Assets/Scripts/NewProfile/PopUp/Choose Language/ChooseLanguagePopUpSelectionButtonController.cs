using UnityEngine;
using TMPro;

public class ChooseLanguagePopUpSelectionButtonController : MonoBehaviour 
{
	
	public int Id;
	private bool isActive;

	void OnMouseOver()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		}
	}
	void OnMouseExit()
	{
		if(!isActive && !ApplicationDesignRules.isMobileScreen)
		{
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		}
	}
	void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<ChooseLanguagePopUpController> ().chooseLanguageHandler(this.Id);
	}
	public void setActive(bool value)
	{
		this.isActive = value;
		if(value)
		{
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		}
		else
		{
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		}
	}
}

