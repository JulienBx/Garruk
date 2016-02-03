using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InputTextGuiController : InterfaceController
{	

	public GUISkin popUpGUISkin;
	private string text;
	private Rect rect;

	public void resize()
	{
		float x = ((this.gameObject.transform.position.x +ApplicationDesignRules.menuPosition.x-ApplicationDesignRules.largeInputTextWorldSize.x/2f+0.15f + ApplicationDesignRules.worldWidth / 2f) / ApplicationDesignRules.worldWidth) * Screen.width;
		float y = ((ApplicationDesignRules.worldHeight / 2f + ApplicationDesignRules.menuPosition.y- this.gameObject.transform.position.y - ApplicationDesignRules.largeInputTextWorldSize.y / 2f) / ApplicationDesignRules.worldHeight) * Screen.height;
		float width = ((ApplicationDesignRules.largeInputTextWorldSize.x-0.3f) / ApplicationDesignRules.worldWidth) * Screen.width;
		float heigth = (ApplicationDesignRules.largeInputTextWorldSize.y / ApplicationDesignRules.worldHeight) * Screen.height;
		this.rect = new Rect (x, y, width, heigth);
		this.popUpGUISkin.textField.fontSize=(int)heigth*50/100;
	}
	void OnGUI()
	{
		GUILayout.BeginArea (rect);
		{
			GUILayout.FlexibleSpace();
			text = GUILayout.TextField(text,popUpGUISkin.textField);
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea ();
	}
	public void setText(string text)
	{
		this.text = text;
	}
	public string getText()
	{
		return text;
	}
	public override void setHoveredState()
	{
		//this.gameObject.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		//this.gameObject.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
	}
}

