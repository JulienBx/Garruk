using UnityEngine;
using TMPro;

public class ProductsPopUpProductController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		SoundController.instance.playSound(8);
		NewStoreController.instance.buyProductHandler(base.getId());
	}
	public override void setIsActive (bool value)
	{
		base.setIsActive (value);
		if(!value)
		{
			this.setForbiddenState();
		}
		else
		{
			base.setInitialState();
		}
	}
}

