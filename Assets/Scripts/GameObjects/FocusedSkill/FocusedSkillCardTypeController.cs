using UnityEngine;
using TMPro;

public class FocusedSkillCardTypeController : TextToHoverController 
{
	private int cardTypeId;

	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(this.cardTypeId),WordingCardTypes.getDescription(this.cardTypeId));
	}
	public void setCardType(int cardTypeId)
	{
		this.cardTypeId=cardTypeId;
	}
}

