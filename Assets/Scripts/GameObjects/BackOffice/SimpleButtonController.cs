using UnityEngine;
using TMPro;

public class SimpleButtonController : InterfaceController
{	
	public override void setHoveredState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
	}
	public override void setForbiddenState()
	{
		gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
	}
}

