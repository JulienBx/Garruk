using UnityEngine;
using TMPro;

public class TextButtonController : InterfaceController
{	
	public override void setHoveredState()
	{
		this.gameObject.transform.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		this.gameObject.transform.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
	}

}

