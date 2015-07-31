using UnityEngine;
using TMPro;

public class NewCardStoreController : NewCardController
{
	private int id;
	
	public override void Update()
	{
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButton(1)) 
		{
			NewStoreController.instance.rightClickedHandler(this.id);
		}
	}
	public void setId(int value)
	{
		this.id = value;
	}
	public override void show()
	{
		base.show ();
	}
}

