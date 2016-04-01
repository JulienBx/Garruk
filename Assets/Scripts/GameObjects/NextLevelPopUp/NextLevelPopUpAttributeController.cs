using UnityEngine;
using TMPro;

public class NextLevelPopUpAttributeController : SpriteButtonController
{

	public override void mainInstruction ()
	{
		this.gameObject.transform.parent.GetComponent<NextLevelPopUpController>().clickOnAttributeHandler(this.getId());
		SoundController.instance.playSound(8);
	}
	public override void setInitialState ()
	{
		if(this.getId()!=3)
		{
			base.setInitialState ();
		}
		else
		{
			this.gameObject.transform.GetComponent<SpriteRenderer> ().color = new Color(0f,0f,0f);
		}
	}
}

