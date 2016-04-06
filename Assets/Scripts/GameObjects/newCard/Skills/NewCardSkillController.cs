using UnityEngine;
using TMPro;

public class NewCardSkillController : NewFocusedCardSkillController 
{

	public override void show()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.s.getPictureId());
		this.gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.s.Level);
	}
	public override void setSkill(Skill s)
	{
		base.setSkill (s);
	}
	public override void setDescription(string d)
	{
		base.setDescription (d);
	}
	public override void showDescription()
	{
	}
	public void changeLayer(int layerVariation, int sortingLayerId)
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().sortingOrder +=layerVariation;
		this.gameObject.transform.GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerId;
	}
	public override void highlightSkill(bool value)
	{
		
	}
	public override void OnMouseDown()
	{
	}
	public override void OnMouseOver()
	{
		if(!base.getIsHovered() && !ApplicationDesignRules.isMobileScreen)
		{
			base.setIsHovered(true);
			BackOfficeController.instance.displayToolTip(true, false,"", this.gameObject,WordingSkills.getName(s.Id),this.d);
		}	
	}
	public override void OnMouseExit()
	{
		if(base.getIsHovered() && !ApplicationDesignRules.isMobileScreen)
		{
			base.setIsHovered(false);
			BackOfficeController.instance.hideToolTip();
		}
	}
}

