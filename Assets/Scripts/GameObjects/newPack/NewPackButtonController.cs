using UnityEngine;
using TMPro;

public class NewPackButtonController : SimpleButtonController 
{	
	public override void mainInstruction()
	{
		this.gameObject.transform.parent.GetComponent<NewPackController> ().buyPackHandler ();
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		this.gameObject.transform.parent.GetComponent<NewPackController> ().buttonHovered(value);
	}
}

