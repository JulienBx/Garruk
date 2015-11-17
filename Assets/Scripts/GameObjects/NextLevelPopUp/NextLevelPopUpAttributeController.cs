using UnityEngine;
using TMPro;

public class NextLevelPopUpAttributeController : SimpleButtonController
{

	private int index;
	private int newPower;
	private int newLevel;
	private bool isHovering;

	public void initialize(int index, int newPower, int newLevel)
	{
		this.index = index;
		this.newPower = newPower;
		this.newLevel = newLevel;
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
	public override void setIsActive (bool value)
	{
		base.setIsActive (value);
		if(!value)
		{
			gameObject.transform.FindChild("Limit").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		}
	}
	void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().clickOnAttribute (this.index, this.newPower, this.newLevel);
	}
}

