using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMyGameSkillSearchBarController : InterfaceController
{	

	public GUISkin popUpGUISkin;
	private string text="";
	private Rect rect;
	private bool isBeingUsed;
	private bool isGUIActive;
	private TouchScreenKeyboard keyboard;

	public void resize()
	{
		float x = ((-System.Convert.ToInt32(ApplicationDesignRules.isMobileScreen)*(ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin)+gameObject.transform.position.x -ApplicationDesignRules.inputTextWorldSize.x/2f+0.15f + ApplicationDesignRules.worldWidth / 2f) / ApplicationDesignRules.worldWidth) * Screen.width;
		float y = ((System.Convert.ToInt32(ApplicationDesignRules.isMobileScreen)*(ApplicationDesignRules.topBarWorldSize.y-0.2f)+ApplicationDesignRules.worldHeight / 2f - this.gameObject.transform.position.y - ApplicationDesignRules.inputTextWorldSize.y / 2f) / ApplicationDesignRules.worldHeight) * Screen.height;
		float width = ((ApplicationDesignRules.inputTextWorldSize.x-0.3f) / ApplicationDesignRules.worldWidth) * Screen.width;
		float heigth = (ApplicationDesignRules.inputTextWorldSize.y / ApplicationDesignRules.worldHeight) * Screen.height;
		this.rect = new Rect (x, y, width, heigth);
		this.popUpGUISkin.textField.fontSize=(int)heigth*50/100;
	}
	void Update()
	{
		if(this.isBeingUsed && ApplicationDesignRules.isMobileDevice)
		{
			if(keyboard!=null && (keyboard.done || keyboard.wasCanceled))
			{
				this.isBeingUsed=false;
			}	
		}
	}
	void OnGUI()
	{
		if(isGUIActive)
		{
			GUILayout.BeginArea (rect);
			{
				GUILayout.FlexibleSpace();
				if(ApplicationDesignRules.isMobileDevice)
				{
					if(GUILayout.Button (text,popUpGUISkin.textField))
			        {
			            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default,false);
						newMyGameController.instance.searchingSkill();
						this.isBeingUsed=true;
			        }
				}
				else
				{
					GUI.SetNextControlName("Textfield");
					text = GUILayout.TextField(text,popUpGUISkin.textField);
					if (GUI.GetNameOfFocusedControl() == "Textfield")
					{
						if(!isBeingUsed)
						{
							newMyGameController.instance.searchingSkill();
							this.isBeingUsed=true;
						}
					}
					else
					{
						this.isBeingUsed=false;
					}
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea ();
		}
	}
	public void setGUI(bool value)
	{
		this.isGUIActive=value;
	}
	public void setText(string text)
	{
		this.text = text;
	}
	public string getText()
	{
		if(ApplicationDesignRules.isMobileDevice)
		{
			return keyboard.text;
		}
		else
		{
			return text;
		}
	}
	public bool getIsBeingUsed()
	{
		return this.isBeingUsed;
	}
	public void closeKeyboard()
	{
		this.keyboard=null;
		this.isBeingUsed=false;
	}
}

