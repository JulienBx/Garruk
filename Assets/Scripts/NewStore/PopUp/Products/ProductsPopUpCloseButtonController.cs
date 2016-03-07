using UnityEngine;
using TMPro;

public class ProductsPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		SoundController.instance.playSound(8);
		NewStoreController.instance.hideProductsPopUp();
	}
}

