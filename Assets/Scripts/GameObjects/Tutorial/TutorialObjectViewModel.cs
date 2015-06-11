using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TutorialObjectViewModel {

	public Rect popUpRect;
	public Rect arrowRect;
	public string nextButtonLabel;
	public string title;
	public string description;
	public bool displayArrow;
	public bool displayNextButton;
	public bool displayRect;
	public GUIStyle buttonStyle;
	public GUIStyle titleStyle;
	public GUIStyle labelStyle;
	public GUIStyle windowStyle;
	public GUIStyle arrowStyle;
	
	public TutorialObjectViewModel ()
	{
		this.popUpRect = new Rect ();
		this.arrowRect = new Rect ();
		this.nextButtonLabel = "Continuer";
		this.title = "";
		this.description = "";
		this.buttonStyle = new GUIStyle ();
		this.labelStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.windowStyle = new GUIStyle ();
		this.arrowStyle = new GUIStyle ();
		this.displayArrow = false;
		this.displayNextButton = true;
		this.displayRect = true;

	}
	public void resize()
	{	
		this.buttonStyle.fontSize = Screen.height * 2 / 100;
		this.buttonStyle.fixedHeight = Screen.height * 3 / 100;
		this.labelStyle.fontSize = Screen.height * 2 / 100;
		this.titleStyle.fontSize = Screen.height * 3 / 100;
	}
}