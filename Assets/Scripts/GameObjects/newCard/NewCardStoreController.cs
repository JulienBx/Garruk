using UnityEngine;
using TMPro;

public class NewCardStoreController : NewCardController
{
	private int id;

	public override void Update()
	{
	}
	public override void OnMouseOver()
	{
		base.OnMouseOver ();
//		if (Input.GetMouseButton(1)) 
//		{
//			NewStoreController.instance.rightClickedHandler(this.id);
//		}
	}
	void OnMouseUp()
	{
		if(!BackOfficeController.instance.getIsSwiping())
		{
			NewStoreController.instance.leftClickedHandler (this.id);
		}
	}
	public override void OnMouseExit()
	{
		base.OnMouseExit ();
	}
	public void setId(int value)
	{
		this.id = value;
	}
	public override void show()
	{
		base.show ();
	}
	public override Camera getCurrentCamera()
	{
		return NewStoreController.instance.returnCurrentCamera ();
	}
}

