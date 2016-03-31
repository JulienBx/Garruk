using UnityEngine;
using TMPro;

public class NextLevelPopUpAttributeController : SpriteButtonController
{

	private int index;
	
	public void initialize(int index)
	{
		this.index = index;
	}
	public override void mainInstruction ()
	{
		this.gameObject.transform.parent.GetComponent<NextLevelPopUpController>().clickOnAttributeHandler(this.index);
		SoundController.instance.playSound(8);
	}
	public override void setInitialState ()
	{
		if(this.index!=3)
		{
			base.setInitialState ();
		}
		else
		{
			this.gameObject.transform.GetComponent<SpriteRenderer> ().color = new Color(0f,0f,0f);
		}
	}
}

