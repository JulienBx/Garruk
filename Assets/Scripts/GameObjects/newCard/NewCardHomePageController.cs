using UnityEngine;
using TMPro;

public class NewCardHomePageController : NewCardController
{
	private int id;
	
	public override void Update()
	{
	}
	
	void OnMouseDrag()
	{
		NewHomePageController.instance.isDraggingCard ();
	}
	public override void OnMouseOver()
	{
		base.OnMouseOver ();
		NewHomePageController.instance.isHoveringCard ();
		if (Input.GetMouseButton(1)) 
		{
			NewHomePageController.instance.rightClickedHandler(this.id);
		}
	}
	public override void OnMouseExit()
	{
		base.OnMouseExit ();
		NewHomePageController.instance.endHoveringCard ();
	}
	void OnMouseDown()
	{
		NewHomePageController.instance.leftClickedHandler (this.id);
	}
	void OnMouseUp()
	{
		NewHomePageController.instance.leftClickReleaseHandler ();
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

