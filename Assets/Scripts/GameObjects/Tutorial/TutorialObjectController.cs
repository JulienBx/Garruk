using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TutorialObjectController : MonoBehaviour 
{
	public GUISkin styles;
	public Texture2D[] arrowTextures;
	public static TutorialObjectController instance;
	private TutorialObjectView view;
	private int sequenceID;
	private float translation;
	private float startTranslation;
	private float currentTranslation;
	private float translationRatio;
	private float speed;
	private bool moveForward;
	private bool moveBack;
	private bool moveHorizontal;
	private bool inversedMove;

	void Awake () 
	{
		instance = this;
		this.view = gameObject.AddComponent <TutorialObjectView>();
		this.sequenceID = -1;
		this.initStyles ();
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
		view.VM.title = this.getTitle();
		view.VM.description = getDescription();
		view.VM.displayNextButton = this.displayNextButton ();
		view.VM.displayArrow = this.displayArrow ();
		if(view.VM.displayArrow)
		{
			view.VM.arrowStyle.normal.background=this.getArrowTexture();
		}
		else
		{
			this.moveBack=false;
			this.moveForward=false;
		}
		this.resize ();
	}
	public void resize()
	{
		if(this.view!=null && this.sequenceID!=-1)
		{
			view.VM.popUpRect = getPopUpRect ();
			if(view.VM.displayArrow)
			{
				view.VM.arrowRect=getArrowRect();
				this.currentTranslation=0;
				this.getTranslation();
				this.moveForward=true;
			}
			view.VM.resize ();
		}
		this.speed = Screen.width/20f;
	}
	private bool displayArrow()
	{
		bool tempBool = false;
		switch(this.sequenceID)
		{
		case 101:case 201: case 203: case 204: case 205: case 206: case 207:case 208: case 209: case 210: case 211: case 213: case 214:
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
		case 101: case 201: case 209: case 210: case 211: case 212: case 214:
			tempBool=false;
			break;
		}
		return tempBool;
	}
	private void getTranslation()
	{
		switch(this.sequenceID)
		{
		case 101: case 201: case 209: case 211: case 214:
			this.translation=0.02f*Screen.height;
			this.moveHorizontal=false;
			this.startTranslation=view.VM.arrowRect.y;
			this.inversedMove=true;
			break;
		case 203: case 207: case 208:
			this.translation=0.01f*Screen.width;
			this.moveHorizontal=true;
			this.startTranslation=view.VM.arrowRect.x;
			this.inversedMove=false;
			break;
		case 204: case 205: case 206:case 210:
			this.translation=0.02f*Screen.height;
			this.moveHorizontal=false;
			this.startTranslation=view.VM.arrowRect.y;
			this.inversedMove=false;
			break;
		case 213:
			this.translation=0.01f*Screen.width;
			this.moveHorizontal=true;
			this.startTranslation=view.VM.arrowRect.x;
			this.inversedMove=true;
			break;
		}
	}
	private Rect getPopUpRect()
	{
		Rect tempRect = new Rect ();
		switch(this.sequenceID)
		{
		case 100:
			tempRect= new Rect (0.35f*Screen.width,0.35f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 101:
			tempRect = new Rect (0.2f*Screen.width,0.2f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 200: case 211: case 212: case 213: case 214:
			tempRect= new Rect (0.35f*Screen.width,0.35f*Screen.height,0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 201:
			Vector2 cardPosition = MyGameController.instance.getCardsPosition(3);
			Vector2 cardSize = MyGameController.instance.getCardsSize(3);
			tempRect= new Rect (cardPosition.x-0.15f*Screen.width,
			                    Screen.height-cardPosition.y+cardSize.y/2f+0.12f*Screen.height,
			                    0.3f*Screen.width,0.5f*Screen.height);
			break;
		case 202:case 203: case 204: case 205: case 206: case 207: case 208: case 209: case 210:
			tempRect= new Rect (0.03f*Screen.width,0.25f*Screen.height,0.3f*Screen.width,0.6f*Screen.height);
			break;
		}
		return tempRect;
	}
	private Rect getArrowRect()
	{
		Vector2 cardPosition = new Vector2 ();
		Vector2 cardSize = new Vector2 ();
		float width;
		float height;
		float x;
		float y;
		Rect tempRect = new Rect ();
		switch(this.sequenceID)
		{
		case 101:
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=0.35f*Screen.width-(width/2f);
			y=0.08f*Screen.height;
			tempRect= new Rect (x,y,width,height);
			break;
		case 201:
			cardPosition = MyGameController.instance.getCardsPosition(3);
			cardSize = MyGameController.instance.getCardsSize(3);
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=cardPosition.x+0*(cardSize.x/2f)-(width/2f);
			y=Screen.height-cardPosition.y+1f*(cardSize.y/2f);
			tempRect= new Rect (x,y,width,height);
			break;
		case 203:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=(2f/3f)*0.1f*Screen.height;
			width=(3f/2f)*height;
			x=cardPosition.x+0.91f*(cardSize.x/2f);
			y=Screen.height-cardPosition.y-0.85f*(cardSize.y/2f)-(height/2f);
			tempRect= new Rect (x,y,width,height);
			break;
		case 204:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=cardPosition.x-0.66f*(cardSize.x/2f)-(width/2f);
			y=Screen.height-cardPosition.y+0.84f*(cardSize.y/2f)-height;
			tempRect= new Rect (x,y,width,height);
			break;
		case 205:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=cardPosition.x-0f*(cardSize.x/2f)-(width/2f);
			y=Screen.height-cardPosition.y+0.84f*(cardSize.y/2f)-height;
			tempRect= new Rect (x,y,width,height);
			break;
		case 206:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=cardPosition.x+0.63f*(cardSize.x/2f)-(width/2f);
			y=Screen.height-cardPosition.y+0.84f*(cardSize.y/2f)-height;
			tempRect= new Rect (x,y,width,height);
			break;
		case 207:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=(2f/3f)*0.1f*Screen.height;
			width=(3f/2f)*height;
			x=cardPosition.x+0.91f*(cardSize.x/2f);
			y=Screen.height-cardPosition.y+0.14f*(cardSize.y/2f)+(height/2f);
			tempRect= new Rect (x,y,width,height);
			break;
		case 208:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=(2f/3f)*0.1f*Screen.height;
			width=(3f/2f)*height;
			x=cardPosition.x+0.91f*(cardSize.x/2f);
			y=Screen.height-cardPosition.y+0.04f*(cardSize.y/2f)-(height/2f);
			tempRect= new Rect (x,y,width,height);
			break;
		case 209:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=cardPosition.x+1*(cardSize.x/2f)-(width/2f)+cardSize.x/4f;
			y=Screen.height-cardPosition.y-0.35f*(cardSize.y/2f);
			tempRect= new Rect (x,y,width,height);
			break;
		case 210:
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=cardPosition.x+1*(cardSize.x/2f)-(width/2f)+cardSize.x/4f;
			y=Screen.height-cardPosition.y+0.70f*(cardSize.y/2f)-height;
			tempRect= new Rect (x,y,width,height);
			break;
		case 211:
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=0.16f*Screen.width-(width/2f);
			y=0.145f*Screen.height;
			tempRect= new Rect (x,y,width,height);
			break;
		case 213:
			height=(2f/3f)*0.1f*Screen.height;
			width=(3f/2f)*height;
			x=0.80f*(Screen.width - 15)+10-width;;
			y=0.5f*Screen.height-(height/2f);
			tempRect= new Rect (x,y,width,height);
			break;
		case 214:
			height=0.1f*Screen.height;
			width=(2f/3f)*height;
			x=0.72f*Screen.width-width/2f;
			y=0.08f*Screen.height;
			tempRect= new Rect (x,y,width,height);
			break;
		}
		return tempRect;
	}
	private Texture2D getArrowTexture()
	{
		Texture2D tempTexture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
		switch(this.sequenceID)
		{
		case 101: case 201: case 209: case 211: case 214: 
			tempTexture = this.arrowTextures[0];
			break;
		case 203: case 207: case 208: 
			tempTexture = this.arrowTextures[3];
			break;
		case 204: case 205: case 206:case 210:
			tempTexture = this.arrowTextures[1];
			break;
		case 213: 
			tempTexture = this.arrowTextures[2];
			break;
		}
		return tempTexture;
	}
	private void setSceneGUI()
	{
		switch(this.sequenceID)
		{
		case 100:
			MenuController.instance.setButtonsGui(false);
			HomePageController.instance.setButtonsGui(false);
			break;
		case 101:
			MenuController.instance.setButtonsGui(false);
			MenuController.instance.setButtonGui(2,true);
			HomePageController.instance.setButtonsGui(false);
			break;
		case 200:
			MenuController.instance.setButtonsGui(false);
			MyGameController.instance.setButtonsGui(false);
			break;
		case 202:
			MyGameController.instance.setGUI(false);
			break;
		case 209:
			MyGameController.instance.setButtonGuiOnFocusedCard(0,true);
			break;
		case 210:
			MyGameController.instance.setGUI(false);
			MyGameController.instance.setButtonGuiOnFocusedCard(1,true);
			break;
		case 211:
			MyGameController.instance.setButtonsGui(false);
			MyGameController.instance.setButtonGui(1,true);
			break;
		case 212:
			MyGameController.instance.setButtonsGui(false);
			break;
		case 213:
			MyGameController.instance.setButtonGui(0,true);
			break;
		case 214:
			MenuController.instance.setButtonGui(5,true);
			MyGameController.instance.setButtonsGui(false);
			break;
		}
	}
	public void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 101:
			StartCoroutine(HomePageController.instance.endTutorial());
			break;
		case 201: case 209: case 210: case 211: 
			this.launchSequence(this.sequenceID+1);
			break;
		case 214:
			StartCoroutine(MyGameController.instance.endTutorial());
			break;
		}
	}
	public int getSequenceID()
	{
		return this.sequenceID;
	}
	public string getTitle()
	{
		string title = "";
		switch(this.sequenceID)
		{
		case 100:
			title="Bienvenue";
			break;
		case 101:
			title="Vite des cartes !";
			break;
		case 200:
			title="Vos cartes";
			break;
		case 201:
			title="Zoom sur une carte";
			break;
		case 202:case 203: case 204: case 205: case 206: case 207:case 208:
			title="Comprendre le visuel d'une carte";
			break;
		case 209:
			title="Comprendre le visuel d'une carte";
			break;
		case 210:
			title="Comprendre le visuel d'une carte";
			break;
		case 211:
			title="Comprendre le visuel d'une carte";
			break;
		case 212:
			title="Comprendre le visuel d'une carte";
			break;
		case 213:
			title="Comprendre le visuel d'une carte";
			break;
		case 214:
			title="Comprendre le visuel d'une carte";
			break;
		}
		return title;
	}
	public string getDescription()
	{
		string description = "";
		switch(this.sequenceID)
		{
		case 100:
			description="Vous êtes ici sur la page d'accueil. Dernières cartes vendues, actualités des amis, compétitions en cours ou bien cartes en vente à la boutique, vous avez tout ici en un clin d'oeil !";
			break;
		case 101:
			description="Nous allons commencer par découvrir vos cartes. ";
			break;
		case 200:
			description="Dans cet espaces vous allez pouvoir gérer vos cartes et créer vos desck que vous utiliserez en cours de match.";
			break;
		case 201:
			description="Pour examiner une carte, cliquez avec le bouton droit de la souris sur son visuel.";
			break;
		case 202:
			description="Une carte comprend un certain nombre d'éléments que nous allons détailler maintenant.";
			break;
		case 203:
			description="La zone supérieure droite de la carte donne son nombre de points de vie. Ces points varient au cours du combat en fonction des dégâts qui sont affligés à la créature. Lorsque ces points atteignent 0, la créature meurt.";
			break;
		case 204: case 205: case 206: case 207: case 208: case 209: case 210: case 211: case 212: case 213: case 214:
			description="A compléter";
			break;
		}
		return description;
	}
	public void setNextButtonDisplaying(bool value)
	{
		view.VM.displayNextButton = value;
	}
}

