using UnityEngine;
using TMPro;

public class NewSkillBookSkillPowerSelectionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		gameObject.transform.parent.GetComponent<NewSkillBookSkillController> ().selectPowerHandler (base.getId ());
	}
	public override void setHoveredState()
	{
		gameObject.transform.parent.GetComponent<NewSkillBookSkillController> ().startHoverPowerHandler (base.getId ());
	}
	public override void setInitialState()
	{
		gameObject.transform.parent.GetComponent<NewSkillBookSkillController> ().endHoverPowerHandler (base.getId ());
	}
	public override void reset()
	{
		base.setIsHovered(false);
		base.setIsActive (true);
		base.setIsSelected (false);
	}
}

