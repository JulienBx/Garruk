using UnityEngine;
using TMPro;

public class NewCardSkillController : NewFocusedCardSkillController 
{

	public override void show(bool isPassiveSkill)
	{
		if(this.isPassiveSkill)
		{
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto(gameObject.transform.parent.GetComponent<NewCardController>().getCardType().getPictureId());
		}
		else
		{
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.s.getPictureId());
			this.gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.s.Level);
		}
	}
	public override void setSkill(Skill s, bool isPassiveSkill)
	{
		base.setSkill (s,isPassiveSkill);
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
}

