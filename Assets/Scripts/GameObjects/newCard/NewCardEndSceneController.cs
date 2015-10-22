using UnityEngine;
using TMPro;

public class NewCardEndSceneController : NewCardController
{
	private int id;
	
	public override void Awake()
	{
		base.Awake ();
		base.experience.SetActive (true);
	}
	public override void Update()
	{
		base.Update ();
	}
	void OnMouseDrag()
	{

	}
	public override void OnMouseOver()
	{
	}
	public override void OnMouseExit()
	{
	}
	void OnMouseDown()
	{

	}
	void OnMouseUp()
	{

	}
	public void setId(int value)
	{
		this.id = value;
	}
	public override void show()
	{
		base.show ();
		base.setExperience ();
	}
	public override void endUpdatingXp(bool hasChangedLevel)
	{
		base.endUpdatingXp (hasChangedLevel);
		EndSceneController.instance.incrementXpDrawn (hasChangedLevel,this.id);
	}
}

