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
	private bool toFocus;
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
		if(this.isBeingUsed)
		{
			if(ApplicationDesignRules.isMobileDevice)
			{
				text=keyboard.text;
				if(keyboard!=null && (keyboard.done || keyboard.wasCanceled))
				{
					newMyGameController.instance.stopSearchingSkill();
				}
			}
		}
	}
	public void resetSearchBar()
	{
		gameObject.transform.FindChild("Title").gameObject.SetActive(true);
		this.isBeingUsed=false;
	}
	void OnMouseUp()
	{
		this.startInput();
	}
	void OnGUI()
	{
		if(isBeingUsed)
		{
			GUILayout.BeginArea (rect);
			{
				GUILayout.FlexibleSpace();
				if(ApplicationDesignRules.isMobileDevice)
				{
					GUILayout.Label (text,popUpGUISkin.textField);
				}
				else
				{
					GUI.SetNextControlName("Textfield");
					text = GUILayout.TextField(text,popUpGUISkin.textField);
					if(toFocus)
					{
						GUI.FocusControl("Textfield");
						this.toFocus=false;
					}
					if (GUI.GetNameOfFocusedControl() != "Textfield")
					{
						newMyGameController.instance.stopSearchingSkill();
					}
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea ();
		}
	}
	public void startInput()
	{
		this.isBeingUsed=true;
		this.toFocus=true;
		this.text="";
		newMyGameController.instance.searchingSkill();
		if(ApplicationDesignRules.isMobileDevice)
		{
			keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default,false);
		}
		gameObject.transform.FindChild("Title").gameObject.SetActive(false);
	}
	public void setButtonText(string text)
	{
		this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=text;
	}
	public string getInputText()
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
}

