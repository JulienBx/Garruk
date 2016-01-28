using UnityEngine;
using TMPro;

public class NewFocusedCardSkillProbaController : MonoBehaviour 
{
	public int id;
	private bool isHovered;

	public void OnMouseOver()
	{
		if(!isHovered && !ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.parent.transform.parent.transform.parent.GetComponent<NewFocusedCardController>().showSkillProbaPopUp(this.id);
			this.isHovered=true;
		}
	}
	public void OnMouseExit()
	{
		if(isHovered && !ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.parent.transform.parent.transform.parent.GetComponent<NewFocusedCardController>().hideSkillPopUp();
			this.isHovered=false;
		}

	}

}

