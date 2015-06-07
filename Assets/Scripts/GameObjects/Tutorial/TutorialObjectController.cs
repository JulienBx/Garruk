using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TutorialObjectController : MonoBehaviour 
{
	public GUISkin styles;
	public string[] titles;
	public string[] descriptions;
	public Texture2D[] arrowTextures;
	public static TutorialObjectController instance;
	private TutorialObjectView view;
	private int sequenceID;
	private bool flashingArrow;
	private float timer;

	void Awake () 
	{
		instance = this;
		this.view = gameObject.AddComponent <TutorialObjectView>();
		this.sequenceID = -1;
		this.initStyles ();
	}
	void Update()
	{
		if(this.flashingArrow)
		{
			this.timer += Time.deltaTime;
			if (this.timer > 0.5f) 
			{	
				this.timer=0;
				view.VM.displayArrow= !view.VM.displayArrow;
			}
		}
	}
	public void initStyles()
	{
		view.VM.buttonStyle = this.styles.button;
		view.VM.windowStyle = this.styles.window;
		view.VM.labelStyle = this.styles.label;
		view.VM.titleStyle = this.styles.customStyles[0];
	}
	public void nextStepHandler()
	{
		this.launchSequence (this.sequenceID + 1);
	}
	public void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		this.setSceneGUI ();
		view.VM.title = this.titles [sequenceID];
		view.VM.description = this.descriptions [sequenceID];
		view.VM.displayNextButton = this.displayNextButton ();
		if(this.displayArrow ())
		{
			view.VM.arrowStyle.normal.background=this.getArrowTexture();
			this.flashingArrow=true;
		}
		this.resize ();
	}
	public void resize()
	{
		if(this.view!=null && this.sequenceID!=-1)
		{
			view.VM.popUpRect = getPopUpRect ();
			if(this.flashingArrow)
			{
				view.VM.arrowRect=getArrowRect();
			}
			view.VM.resize ();
		}
	}
	private bool displayArrow()
	{
		bool tempBool = false;
		switch(this.sequenceID)
		{
		case 1:
			tempBool=true;
			break;
		case 3:
			tempBool=true;
			break;
		}
		return tempBool;
	}
	private bool displayNextButton()
	{
		bool tempBool = true;
		switch(this.sequenceID)
		{
		case 1:
			tempBool=false;
			break;
		case 3:
			tempBool=false;
			break;
		case 4:
			tempBool=false;
			break;
		}
		return tempBool;
	}
	private Rect getPopUpRect()
	{
		Rect tempRect = new Rect ();
		switch(this.sequenceID)
		{
		case 0:
			tempRect= new Rect (0.35f*Screen.width,0.35f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 1:
			tempRect = new Rect (0.2f*Screen.width,0.2f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 2:
			tempRect= new Rect (0.35f*Screen.width,0.35f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 3:
			tempRect= new Rect (0.35f*Screen.width,0.35f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 4:
			tempRect= new Rect (0.03f*Screen.width,0.25f*Screen.height,0.2f*Screen.width,0.6f*Screen.height);
			break;
		}
		return tempRect;
	}
	private Rect getArrowRect()
	{
		Rect tempRect = new Rect ();
		switch(this.sequenceID)
		{
		case 1:
			tempRect= new Rect (0.325f*Screen.width,0.08f*Screen.height,0.05f*Screen.width,0.1f*Screen.height);
			break;
		case 3:
			tempRect= new Rect (0.044f*Screen.width,0.51f*Screen.height,0.05f*Screen.width,0.1f*Screen.height);
			break;
		}
		return tempRect;
	}
	private Texture2D getArrowTexture()
	{
		Texture2D tempTexture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
		switch(this.sequenceID)
		{
		case 1:
			tempTexture = this.arrowTextures[0];
			break;
		case 3:
			tempTexture = this.arrowTextures[0];
			break;
		}
		return tempTexture;
	}
	private void setSceneGUI()
	{
		switch(this.sequenceID)
		{
		case 0:
			MenuController.instance.setButtonsGui(false);
			HomePageController.instance.setButtonsGui(false);
			break;
		case 1:
			MenuController.instance.setButtonsGui(false);
			MenuController.instance.setButtonGui(2,true);
			HomePageController.instance.setButtonsGui(false);
			break;
		case 2:
			MenuController.instance.setButtonsGui(false);
			MyGameController.instance.setButtonsGui(false);
			break;
		case 4:
			MyGameController.instance.setGUI(false);
			break;
		}
	}
	public void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1:
			StartCoroutine(HomePageController.instance.endTutorial());
			break;
		case 3:
			this.launchSequence(this.sequenceID+1);
			break;
		}
	}
	public int getSequenceID()
	{
		return this.sequenceID;
	}
}

