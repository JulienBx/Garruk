using UnityEngine;
using TMPro;

public class ProductsPopUpProductController : SpriteButtonController
{

	public int id;

	public override void mainInstruction ()
	{
	}
	public void show (Product p)
	{
		this.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().sprite=NewStoreController.instance.productsIcons[p.Id];
		this.gameObject.transform.FindChild("Price").GetComponent<TextMeshPro>().text=p.Price.ToString()+" €";
		this.gameObject.transform.FindChild("Crystals").GetComponent<TextMeshPro>().text=p.Crystals.ToString();
	}
}

