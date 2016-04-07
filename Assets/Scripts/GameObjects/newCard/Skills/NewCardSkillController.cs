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
	public override void setHoveredState()
	{
	}
	public override void setInitialState()
	{
	}
	public override void mainInstruction()
	{
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingSkills.getName(s.Id),this.d);
	}
}

