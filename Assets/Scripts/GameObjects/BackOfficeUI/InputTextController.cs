using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InputTextController : InterfaceController
{	
	public void setText(string text)
	{
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = text;
	}
	public override void setHoveredState()
	{
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
	}
}

