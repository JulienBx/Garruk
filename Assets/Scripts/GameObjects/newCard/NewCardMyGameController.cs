using UnityEngine;
using TMPro;

public class NewCardMyGameController : NewCardController
{
	private int id;
	private bool isDeckCard;

	void OnMouseDrag()
	{
		newMyGameController.instance.isDraggingCard ();
	}
	void OnMouseOver()
	{
		if(this.c.onSale!=1 && this.c.IdOWner!=-1)
		{
			newMyGameController.instance.isHoveringCard ();
		}
		if (Input.GetMouseButton(1)) 
		{
			newMyGameController.instance.rightClickedHandler(this.id,this.isDeckCard);
		}
	}
	void OnMouseExit()
	{
		if(this.c.onSale!=1 && this.c.IdOWner!=-1)
		{
			newMyGameController.instance.endHoveringCard ();
		}
	}
	void OnMouseDown()
	{
		newMyGameController.instance.leftClickedHandler (this.id, this.isDeckCard);
	}
	void OnMouseUp()
	{
		newMyGameController.instance.leftClickReleaseHandler ();
	}
	public void setId(int value, bool isDeckCard)
	{
		this.id = value;
		this.isDeckCard = isDeckCard;
	}
}

