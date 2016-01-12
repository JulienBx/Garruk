using UnityEngine;
using TMPro;

public class NewFocusedCardSkillSkillTypeController : MonoBehaviour 
{
	public int id;
	private bool isHovered;
	
	public void OnMouseOver()
	{
		if(!isHovered)
		{
			gameObject.transform.parent.transform.parent.transform.parent.GetComponent<NewFocusedCardController>().showSkillTypePopUp(this.id);
			this.isHovered=true;
		}
	}
	public void OnMouseExit()
	{
		if(isHovered)
		{
			gameObject.transform.parent.transform.parent.transform.parent.GetComponent<NewFocusedCardController>().hideSkillPopUp();
			this.isHovered=false;
		}
		
	}
	
}

