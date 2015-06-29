using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TutorialObjectController : MonoBehaviour 
{
	public static TutorialObjectController instance;
	public TutorialObjectView view;
	private TutorialObjectRessources ressources;
	public int sequenceID;
	private float translation;
	private float startTranslation;
	private float currentTranslation;
	private float translationRatio;
	private float speed;
	public float arrowWidth;
	public float arrowHeight;
	public float arrowX;
	public float arrowY;
	public float popUpWidth;
	public float popUpHeight;
	public float popUpX;
	public float popUpY;
	public Vector2 GOPosition;
	public Vector2 GOSize;
	private bool moveForward;
	private bool moveBack;
	private bool moveHorizontal;
	private bool inversedMove;
	public bool isResizing;

	void Awake () 
	{
		instance = this;
		this.isResizing = false;
		this.ressources = gameObject.GetComponent<TutorialObjectRessources> ();
		this.view = gameObject.AddComponent <TutorialObjectView>();
		this.sequenceID = -1;
		this.initStyles ();
		this.resize ();
		this.speed = Screen.width/20f;
	}
	void Update()
	{
		if(this.moveForward)
		{
			this.currentTranslation=this.currentTranslation+Time.deltaTime*this.speed;
			this.translationRatio=this.currentTranslation/this.translation;
			if(this.translationRatio>1)
			{
				this.currentTranslation=this.translation;
				this.moveForward=false;
				this.moveBack=true;
				this.translationRatio=0;
			}
			if(moveHorizontal)
			{
				if(!this.inversedMove)
				{
					view.VM.arrowRect.x=this.startTranslation+this.currentTranslation;
				}
				else
				{
					view.VM.arrowRect.x=this.startTranslation-this.currentTranslation;
				}
			}
			else
			{
				if(!this.inversedMove)
				{
					view.VM.arrowRect.y=this.startTranslation-this.currentTranslation;
				}
				else
				{
					view.VM.arrowRect.y=this.startTranslation+this.currentTranslation;
				}
			}
		}
		else if(this.moveBack)
		{
			this.currentTranslation=this.currentTranslation-Time.deltaTime*this.speed;
			this.translationRatio=1-(this.currentTranslation/this.translation);
			if(this.translationRatio>1)
			{
				this.currentTranslation=0;
				this.moveForward=true;
				this.moveBack=false;
				this.translationRatio=0;
			}
			if(moveHorizontal)
			{
				if(!this.inversedMove)
				{
					view.VM.arrowRect.x=this.startTranslation+this.currentTranslation;
				}
				else
				{
					view.VM.arrowRect.x=this.startTranslation-this.currentTranslation;
				}
			}
			else
			{
				if(!this.inversedMove)
				{
					view.VM.arrowRect.y=this.startTranslation-this.currentTranslation;
				}
				else
				{
					view.VM.arrowRect.y=this.startTranslation+this.currentTranslation;
				}
			}
		}
	}
	public void initStyles()
	{
		view.VM.buttonStyle = ressources.styles.button;
		view.VM.windowStyle = ressources.styles.window;
		view.VM.labelStyle = ressources.styles.label;
		view.VM.titleStyle = ressources.styles.customStyles[0];
	}
	public void nextStepHandler()
	{
		this.launchSequence (this.sequenceID + 1);
	}
	public virtual void launchSequence(int sequenceID)
	{
	}
	public void resize()
	{
		this.isResizing = true;
		view.VM.resize();
		this.launchSequence(sequenceID);
		this.isResizing = false;
		this.speed = Screen.width/20f;
	}
	public void setUpArrow()
	{
		this.moveHorizontal=false;
		this.inversedMove=true;
		view.VM.arrowStyle.normal.background = ressources.arrowTextures[0];
	}
	public void setDownArrow()
	{
		this.moveHorizontal=false;
		this.inversedMove=false;
		view.VM.arrowStyle.normal.background = ressources.arrowTextures[1];
	}
	public void setRightArrow()
	{
		this.moveHorizontal=true;
		this.inversedMove=true;
		view.VM.arrowStyle.normal.background = ressources.arrowTextures[2];
	}
	public void setLeftArrow()
	{
		this.moveHorizontal=true;
		this.inversedMove=false;
		view.VM.arrowStyle.normal.background = ressources.arrowTextures[3];
	}
	public void drawUpArrow()
	{
		this.translation=0.02f*Screen.height;
		this.startTranslation=view.VM.arrowRect.y;
		this.moveForward = true;
	}
	public void drawDownArrow()
	{
		this.translation=0.02f*Screen.height;
		this.startTranslation=view.VM.arrowRect.y;
		this.moveForward = true;
	}
	public void drawRightArrow()
	{
		this.translation=0.01f*Screen.width;
		this.startTranslation=view.VM.arrowRect.x;
		this.moveForward = true;
	}
	public void drawLeftArrow()
	{
		this.translation=0.01f*Screen.width;
		this.startTranslation=view.VM.arrowRect.x;
		this.moveForward = true;
	}
	public float computePopUpHeight()
	{
		float height;
		float width = this.popUpWidth - view.VM.windowStyle.padding.left - view.VM.windowStyle.padding.right;
		height=2f*System.Convert.ToInt32(view.VM.displayNextButton)*view.VM.buttonStyle.CalcHeight(new GUIContent(view.VM.nextButtonLabel),width)
			+ view.VM.titleStyle.CalcHeight(new GUIContent(view.VM.title),width)
				+view.VM.labelStyle.CalcHeight(new GUIContent(view.VM.description),width)
				+0.05f*Screen.height;
		height = height + view.VM.windowStyle.padding.top + view.VM.windowStyle.padding.bottom;
		return height;
	}
	public virtual void actionIsDone()
	{
	}
	public int getSequenceID()
	{
		return this.sequenceID;
	}
	public void setNextButtonDisplaying(bool value)
	{
		view.VM.displayNextButton = value;
		this.resize ();
	}
}

