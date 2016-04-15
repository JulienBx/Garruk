using UnityEngine;
using TMPro;

public class ProductsPopUpProductController : SpriteButtonController
{

	public int id;

	public override void mainInstruction ()
	{
		SoundController.instance.playSound(8);
		NewStoreController.instance.buyProductHandler(this.id);
	}
}

