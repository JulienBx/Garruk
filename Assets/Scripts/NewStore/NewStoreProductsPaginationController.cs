using UnityEngine;
using TMPro;

public class NewStoreProductsPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewStoreController.instance.paginationProductsHandler();
	}
	public override void resize()
	{
		for (int i=0;i<buttons.Length;i++)
		{
			this.buttons[i].transform.localScale=new Vector3(0.9f,0.9f,0.9f);
		}
		this.buttons[0].transform.localPosition=new Vector3 (-0.5f, 0f, 0f);
		this.buttons[1].transform.localPosition=new Vector3 (0.5f, 0f, 0f);
	}
}

