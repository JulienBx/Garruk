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
	public void show (DisplayedProduct p)
	{
		this.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().sprite=NewStoreController.instance.productsIcons[p.Id];
		this.gameObject.transform.FindChild("Price").GetComponent<TextMeshPro>().text=NewStoreController.instance.getProductsPrice(this.id);
		this.gameObject.transform.FindChild("Crystals").GetComponent<TextMeshPro>().text=p.Crystals.ToString();
	}
}

