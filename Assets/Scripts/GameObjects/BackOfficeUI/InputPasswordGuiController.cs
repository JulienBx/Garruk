using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InputPasswordGuiController : InterfaceController
{	
	
	public GUISkin popUpGUISkin;
	private string text;
	private Rect rect;
	private bool keyReturnPressed;
	private bool keyEscapePressed;
	private bool toFocus;

	void Update()
	{
		if(keyReturnPressed)
		{
			keyReturnPressed=false;
			BackOfficeController.instance.returnPressed();
		}
		if(keyEscapePressed)
		{
			keyEscapePressed=false;
			BackOfficeController.instance.escapePressed();
		}
	}
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
			GUI.SetNextControlName("PasswordField");
			text = GUILayout.PasswordField(text,'*',popUpGUISkin.textField);
			if(this.toFocus)
			{
				GUI.FocusControl("PasswordField");
				this.toFocus=false;
			}
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea ();
		if (Event.current.keyCode == KeyCode.Return) 
		{
			this.keyReturnPressed=true;
 		}
 		if (Event.current.keyCode == KeyCode.Escape) 
		{
			this.keyEscapePressed=true;
 		}
	}
	public void setFocused()
	{
		this.toFocus=true;
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

