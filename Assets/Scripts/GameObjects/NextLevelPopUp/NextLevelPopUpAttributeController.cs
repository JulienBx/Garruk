using UnityEngine;
using TMPro;

public class NextLevelPopUpAttributeController : SimpleButtonController
{

	private int index;
	private int newPower;
	private int newLevel;
	private bool isNotClickable;
	
	public void initialize(int index, int newPower, int newLevel)
	{
		this.index = index;
		this.newPower = newPower;
		this.newLevel = newLevel;
	}
	public override void setHoveredState()
	{
		if(!isNotClickable)
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		}
	}
	public override void setInitialState()
	{
		if(!isNotClickable)
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
	}
	public override void setIsHovered (bool value)
	{
		base.setIsHovered (value);
		if(value)
		{
			if(index<3)
			{
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().displayAttributePopUp(index);
			}
			else
			{
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().displaySkillPopUp(index);
			}
		}
		else
		{
			if(index<3)
			{
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().hideAttributePopUp();
			}
			else
			{
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().hideSkillPopUp();
			}
		}
	}
	public void setIsNotClickable()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
		this.isNotClickable = true;
	}
	void OnMouseDown()
	{
		if(!this.isNotClickable)
		{
			gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().clickOnAttribute (this.index, this.newPower, this.newLevel);
		}
	}
}

