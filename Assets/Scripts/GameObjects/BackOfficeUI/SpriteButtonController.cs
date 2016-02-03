using UnityEngine;
using TMPro;

public class SpriteButtonController : InterfaceController
{	
	public override void setHoveredState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
	}
}

