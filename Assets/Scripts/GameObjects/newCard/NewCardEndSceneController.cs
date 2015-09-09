using UnityEngine;
using TMPro;

public class NewCardEndSceneController : NewCardController
{
	//private int id;
	
	public override void Update()
	{
	}
	
	void OnMouseDrag()
	{

	}
	public override void OnMouseOver()
	{
		base.OnMouseOver ();

	}
	public override void OnMouseExit()
	{
		base.OnMouseExit ();
	}
	void OnMouseDown()
	{

	}
	void OnMouseUp()
	{

	}
//	public void setId(int value, bool isDeckCard)
//	{
//		this.id = value;
//	}
	public override void show()
	{
		base.show ();
	}
	public override void endUpdatingXp()
	{
		base.endUpdatingXp ();
		EndSceneController.instance.incrementXpDrawn ();
	}
}

