using UnityEngine;
using TMPro;

public class NextLevelPopUpAttributeController :MonoBehaviour
{

	private int index;
	private int newPower;
	private int newLevel;
	private bool isNotClickable;
	private bool isHovered;
	
	public void initialize(int index, int newPower, int newLevel)
	{
		this.index = index;
		this.newPower = newPower;
		this.newLevel = newLevel;
	}
	public void setIsNotClickable()
	{
		this.isNotClickable = true;
	}
	void OnMouseOver()
	{
		if(!this.isHovered && !ApplicationDesignRules.isMobileScreen)
		{
			this.isHovered=true;
			if(this.index<3)
			{
				if(!this.isNotClickable)
				{
					gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
				}
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().displayAttributePopUp(this.index);
			}
			else
			{
				if(!this.isNotClickable)
				{
					gameObject.transform.parent.transform.FindChild("SkillButton"+(this.index-3)).GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
				}
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().displaySkillPopUp(this.index);
			}
		}
	}
	void OnMouseExit()
	{
		if(this.isHovered && !ApplicationDesignRules.isMobileScreen)
		{
			this.isHovered=false;
			if(this.index<3)
			{
				if(!this.isNotClickable)
				{
					gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
				}
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().hideAttributePopUp();
			}
			else
			{
				if(!this.isNotClickable)
				{
					gameObject.transform.parent.transform.FindChild("SkillButton"+(this.index-3)).GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
				}
				gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().hideSkillPopUp();
			}
		}
	}
	void OnMouseDown()
	{
		if(!this.isNotClickable)
		{
			BackOfficeController.instance.playSound(8);
			gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().clickOnAttribute (this.index, this.newPower, this.newLevel);
		}
	}
}

