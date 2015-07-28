using UnityEngine;
using TMPro;

public class NewCardLobbyController : NewCardController
{
	private int id;
	
	public override void Update()
	{
	}
	
	void OnMouseDrag()
	{
		NewLobbyController.instance.isDraggingCard ();
	}
	void OnMouseOver()
	{
		NewLobbyController.instance.isHoveringCard ();
		if (Input.GetMouseButton(1)) 
		{
			NewLobbyController.instance.rightClickedHandler(this.id);
		}
	}
	void OnMouseExit()
	{
		NewLobbyController.instance.endHoveringCard ();
	}
	void OnMouseDown()
	{
		NewLobbyController.instance.leftClickedHandler (this.id);
	}
	void OnMouseUp()
	{
		NewLobbyController.instance.leftClickReleaseHandler ();
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

