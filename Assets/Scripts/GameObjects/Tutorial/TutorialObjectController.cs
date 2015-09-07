using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class TutorialObjectController : MonoBehaviour 
{
	public static TutorialObjectController instance;
	public TutorialObjectView view;
	private TutorialObjectRessources ressources;
	public int sequenceID;
	private float startTranslation;
	private float currentTranslation;
	private float speed;
	public float worldWidth;
	public float worldHeight;
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
	private bool move;
	private bool moveForward;
	private bool moveBack;
	private bool moveHorizontal;
	private bool inversedMove;
	public bool isResizing;

	private GameObject arrow;
	private GameObject popUpTitle;
	private GameObject popUpDescription;
	private GameObject background;
	private GameObject popUpNextButton;
	private GameObject popUp;

	private Rect backgroundRect;
	private float popUpHalfHeight;

	void Update()
	{
		
		if(this.move)
		{
			if(!moveBack)
			{
				this.currentTranslation=this.currentTranslation+Time.deltaTime*this.speed;
				if(this.currentTranslation>0.5f)
				{
					this.currentTranslation=0.5f;
					this.moveForward=false;
					this.moveBack=true;
				}
			}
			else
			{
				this.currentTranslation=this.currentTranslation-Time.deltaTime*this.speed;
				if(this.currentTranslation<0f)
				{
					this.currentTranslation=0f;
					this.moveForward=true;
					this.moveBack=false;
				}
			}
			if(moveHorizontal)
			{
				Vector3 arrowPosition = gameObject.transform.FindChild("Arrow").position;
				arrowPosition.x=this.startTranslation+this.currentTranslation;
				gameObject.transform.FindChild("Arrow").position=arrowPosition;
			}
			else
			{
				Vector3 arrowPosition = gameObject.transform.FindChild("Arrow").position;
				arrowPosition.y=this.startTranslation+this.currentTranslation;
				gameObject.transform.FindChild("Arrow").position=arrowPosition;
			}
		}
	}
	void Awake () 
	{
		instance = this;
		this.isResizing = false;
		this.ressources = gameObject.GetComponent<TutorialObjectRessources> ();
		this.sequenceID = -1;
		this.speed = 2f;
		this.arrow = gameObject.transform.FindChild ("Arrow").gameObject;
		this.background = gameObject.transform.FindChild ("Background").gameObject;
	}
	public void displayBackground(bool value)
	{
		this.background.SetActive (value);
	}
	public void resizeBackground(Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
	{
		this.background.GetComponent<TutorialBackgroundController> ().resize (rect,clickableSectionXRatio,clickableSectionYRatio);
		this.backgroundRect = rect;
	}
	public void setPopUpTitle(string title)
	{
		this.popUpTitle.GetComponent<TextMeshPro> ().text = title;
	}
	public void setPopUpDescription(string description)
	{
		this.popUpDescription.GetComponent<TextMeshPro> ().text = description;
	}
	public void displayArrow(bool value)
	{
		this.arrow.SetActive (value);
		this.move = false;
	}
	public void resizeArrow(Vector3 position)
	{
		this.arrow.transform.position = position;
	}
	public void resizePopUp(Vector3 position)
	{
		this.popUp.transform.position = position;
	}
	public void displayNextButton(bool value)
	{
		this.popUpNextButton.SetActive (value);
	}
	public void displayPopUp(int value)
	{
		if(value==-1)
		{
			this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
			this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
			this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
		}
		else
		{
			if(value==0)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(true);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
				this.popUp=this.gameObject.transform.FindChild("PopUpSmall").gameObject;
				this.popUpHalfHeight=2.75f;
			}
			else if(value==1)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(true);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
				this.popUp=this.gameObject.transform.FindChild("PopUp").gameObject;
				this.popUpHalfHeight=3.375f;
			}
			else if(value==2)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(true);
				this.popUp=this.gameObject.transform.FindChild("PopUpLarge").gameObject;
				this.popUpHalfHeight=4f;

			}
			this.popUpTitle = this.popUp.transform.FindChild ("Title").gameObject;
			this.popUpDescription = this.popUp.transform.FindChild ("Description").gameObject;
			this.popUpNextButton= this.popUp.transform.FindChild ("NextButton").gameObject;
			this.popUpNextButton.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			this.popUpNextButton.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}

	}
//	public void initStyles()
//	{
//		view.VM.buttonStyle = ressources.styles.button;
//		view.VM.windowStyle = ressources.styles.window;
//		view.VM.labelStyle = ressources.styles.label;
//		view.VM.titleStyle = ressources.styles.customStyles[0];
//	}
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
		this.launchSequence(sequenceID);
		this.isResizing = false;
	}
	public void setUpArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=false;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, 90.0f);
	}
	public void setDownArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=false;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, -90.0f);
	}
	public void setRightArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=true;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, 0.0f);
	}
	public void setLeftArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=true;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, 180.0f);
	}
	public void drawUpArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x,backgroundRect.y+0.5f-backgroundRect.height/2f,-4f));
		this.startTranslation=backgroundRect.y-1f-backgroundRect.height/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.y = this.startTranslation +0.5f- this.popUpHalfHeight;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void drawDownArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x,backgroundRect.y+0.5f+backgroundRect.height/2f,-4f));
		this.startTranslation=backgroundRect.y+0.5f+backgroundRect.height/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.y = this.startTranslation + this.popUpHalfHeight;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void drawRightArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x-0.5f-backgroundRect.width/2f,backgroundRect.y,-4f));
		this.startTranslation=backgroundRect.x-1f-backgroundRect.width/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.x = this.startTranslation+0.5f - 4f;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void drawLeftArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x+0.5f+backgroundRect.width/2f,backgroundRect.y,-4f));
		this.startTranslation=backgroundRect.x+0.5f+backgroundRect.width/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.x = this.startTranslation + 4f;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public float computePopUpHeight()
	{
		float height=0;
//		float width = this.popUpWidth - view.VM.windowStyle.padding.left - view.VM.windowStyle.padding.right;
//		height=2f*System.Convert.ToInt32(view.VM.displayNextButton)*view.VM.buttonStyle.CalcHeight(new GUIContent(view.VM.nextButtonLabel),width)
//			+ view.VM.titleStyle.CalcHeight(new GUIContent(view.VM.title),width)
//				+view.VM.labelStyle.CalcHeight(new GUIContent(view.VM.description),width)
//				+0.05f*Screen.height;
//		height = height + view.VM.windowStyle.padding.top + view.VM.windowStyle.padding.bottom;
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
//		view.VM.displayNextButton = value;
//		this.resize ();
	}
}

