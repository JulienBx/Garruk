using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InputTextGuiController : InterfaceController
{	

	public Font font;
	private string text;
	private Rect rect;
	private bool keyReturnPressed;
	private bool keyEscapePressed;
	private bool toFocus;
	private GUIStyle textFieldStyle;
	private bool isFocused;

	public override void Awake()
	{
		base.Awake();
		this.textFieldStyle=new GUIStyle();
		this.textFieldStyle.font=this.font;
		this.textFieldStyle.fontSize=20;
		this.textFieldStyle.alignment=TextAnchor.MiddleCenter;
		this.textFieldStyle.normal.textColor=ApplicationDesignRules.whiteTextColor;
	}

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
	}
	void OnGUI()
	{
		if(!BackOfficeController.instance.getIsLoadingScreenDisplayed())
		{
			GUILayout.BeginArea (rect);
			{
				GUILayout.FlexibleSpace();
				GUI.SetNextControlName("TextField");
				text = GUILayout.TextField(text,textFieldStyle);
				if(this.toFocus)
				{
					GUI.FocusControl("TextField");
					this.toFocus=false;
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea ();
		}
		Event e = Event.current;
		if (e.isKey && e.type == EventType.KeyUp && GUI.GetNameOfFocusedControl()=="TextField")
		{
             switch(e.keyCode)
             {
                 case KeyCode.Return: 
                 	this.keyReturnPressed=true; 
                 	break;
                 case KeyCode.Escape: 
                 	this.keyEscapePressed=true; 
                 	break;    
             }
        }
		if(GUI.GetNameOfFocusedControl()=="TextField" && !this.isFocused)
		{
			this.isFocused=true;
			this.gameObject.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
		}
		else if(GUI.GetNameOfFocusedControl()!="TextField" && this.isFocused)
		{
			this.isFocused=false;
			this.gameObject.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteTextColor;
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
	}
	public override void setInitialState()
	{
	}
	public override void OnMouseDown()
	{
		if(!this.isFocused)
		{
			this.setFocused();
		}
	}
	public override void OnMouseUp()
	{
	}
}

