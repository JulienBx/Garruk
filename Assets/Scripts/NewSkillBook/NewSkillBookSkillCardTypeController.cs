using UnityEngine;
using TMPro;

public class NewSkillBookSkillCardTypeController : TextToHoverController	
{	
	private int cardTypeId;

	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(this.cardTypeId),WordingCardTypes.getName(this.cardTypeId));
	}
	public void setCardType(int id)
	{
		this.cardTypeId=id;
	}
}

