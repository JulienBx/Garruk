using UnityEngine;
using TMPro;

public class NewCardMyGameController : NewCardController
{
	private int id;
	private bool isDeckCard;

	public override void Update()
	{
	}
	
	void OnMouseDrag()
	{
		newMyGameController.instance.isDraggingCard ();
	}
	public override void OnMouseOver()
	{
		base.OnMouseOver ();
		if(this.c.onSale!=1 && this.c.IdOWner!=-1)
		{
			newMyGameController.instance.isHoveringCard ();
		}
		if (Input.GetMouseButton(1)) 
		{
			newMyGameController.instance.rightClickedHandler(this.id,this.isDeckCard);
		}
	}
	public override void OnMouseExit()
	{
		base.OnMouseExit ();
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
	public override void show()
	{
		base.show ();
		if(this.c.IdOWner==-1)
		{
			this.displayPanelSold();
		}
		else
		{
			this.hidePanelSold();
		}
	}
}

