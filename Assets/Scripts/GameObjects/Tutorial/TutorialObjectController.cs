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
	private float arrowWidth;
	private float arrowHeight;
	private float arrowX;
	private float arrowY;
	private float popUpWidth;
	private float popUpHeight;
	private float popUpX;
	private float popUpY;
	private Vector2 cardPosition;
	private Vector2 cardSize;
	private bool moveForward;
	private bool moveBack;
	private bool moveHorizontal;
	private bool inversedMove;
	private bool isResizing;

	void Awake () 
	{
		instance = this;
		this.isResizing = false;
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
		switch(this.sequenceID)
		{
		case 100:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				HomePageController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Bienvenue";
				view.VM.description="Vous êtes ici sur la page d'accueil. Dernières cartes vendues, actualités des amis, compétitions en cours ou bien cartes en vente à la boutique, vous avez tout ici en un clin d'oeil !";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.3f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 101:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				MenuController.instance.setButtonGui(2,true);
				HomePageController.instance.setButtonsGui(false);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Vite des cartes !";
				view.VM.description="Nous allons commencer par découvrir vos cartes. ";
				this.setUpArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=0.35f*Screen.width-(arrowWidth/2f);
			arrowY=0.08f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 200:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				MyGameController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Vos cartes";
				view.VM.description="Dans cet espaces vous allez pouvoir gérer vos cartes et créer vos desck que vous utiliserez en cours de match.";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 201:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Zoom sur une carte";
				view.VM.description="Pour examiner une carte, cliquez avec le bouton droit de la souris sur son visuel.";
				this.setUpArrow();
			}
			cardPosition = MyGameController.instance.getCardsPosition(3);
			cardSize = MyGameController.instance.getCardsSize(3);
			view.VM.popUpRect= new Rect (cardPosition.x-0.15f*Screen.width,
			                    Screen.height-cardPosition.y+cardSize.y/2f+0.12f*Screen.height,
			                    0.3f*Screen.width,0.5f*Screen.height);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+0*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y+1f*(cardSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 202:
			if(!isResizing)
			{
				MyGameController.instance.setGUI(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Comprendre le visuel d'une carte";
				view.VM.description="Chaque comprend une illustration qui correspond à sa classe ainsi qu'un certain nombre d'éléments que nous allons détailler maintenant. Ces éléments sont variables en fonction de la classe de la carte";
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			popUpWidth=0.58f*cardSize.x;
			popUpHeight=this.computePopUpHeight();
			popUpX=cardPosition.x+cardSize.x/2f;
			popUpY=Screen.height-cardPosition.y-cardSize.y/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 203:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points de vie";
				view.VM.description="La zone supérieure droite de la carte donne son nombre de points de vie. Ces points varient au cours du combat en fonction des dégâts qui sont affligés à la créature. Lorsque ces points atteignent 0, la créature meurt.";
				this.setUpArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+0.60f*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y-0.76f*(cardSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			view.VM.popUpRect= new Rect (0.03f*Screen.width,0.25f*Screen.height,0.3f*Screen.width,0.6f*Screen.height);
			this.drawUpArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 204:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points d'attaque";
				view.VM.description="Ils indiquent les dégats qui seront causés par le personnage lors d'une simple attaque. 10 points de dégâts se traduisent par une perte de 10 points de vie sur la carte ciblée par l'attaque.";
				this.setDownArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x-0.66f*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y+0.84f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 205:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points de rapidité";
				view.VM.description="Ce sont ces points là qui déterminent l'ordre de jeu entre les différents personnages. Plus ces points sont élevés plus la carte à de chance de jouer en premier.";
				this.setDownArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x-0f*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y+0.84f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 206:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points de déplacement";
				view.VM.description="Ils indiquent tout simplement le nombre de pas autorisé par la carte. Seule les déplacement verticaux et horizontaux sont possibles.";
				this.setDownArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+0.63f*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y+0.84f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 207:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les compétences";
				view.VM.description="Il s'agit des sorts pouvant être lancés par la carte en situation de combat. Chaque sort est évalué de 0 à 100. En passant la souris sur une compétence vous pouvez lire ses effets";
				this.setDownArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x-0f*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y+0.3f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 208:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="L'expérience de la carte";
				view.VM.description="Au cours de sa vie, une carte progresse en fonction de son expérience. A chaque niveau, une de ses caractèristique est augmentée. Il y a en tout 10 niveau. La jauge symbolise le niveau de progression de la carte sur son nivau";
				this.setDownArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+0.72f*(cardSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-cardPosition.y-0.05f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 209:
			if(!isResizing)
			{
				MyGameController.instance.setButtonGuiOnFocusedCard(0,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Faire progresser la carte";
				view.VM.description="A tout moment et en fonction de vos crédits il vous est possible d'augmenter le niveau d'une carte. A vous de jouer, faites progresser cette carte jusqu'au prochain niveau";
				this.setUpArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+1*(cardSize.x/2f)-(arrowWidth/2f)+cardSize.x/4f;
			arrowY=Screen.height-cardPosition.y-0.35f*(cardSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 210:
			if(!isResizing)
			{
				MyGameController.instance.setGUI(false);
				MyGameController.instance.setButtonGuiOnFocusedCard(1,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Bravo !";
				view.VM.description="Vous verrez que d'autres actions sont possible comme la vente ou le renommage. Retournons à l'écran d'affichage de vos cartes";
				this.setDownArrow();
			}
			cardPosition = MyGameController.instance.getFocusCardsPosition();
			cardSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+1*(cardSize.x/2f)-(arrowWidth/2f)+cardSize.x/4f;
			arrowY=Screen.height-cardPosition.y+0.70f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 211:
			if(!isResizing)
			{
				MyGameController.instance.setGUI(true);
				MyGameController.instance.setButtonsGui(false);
				MyGameController.instance.setButtonGui(1,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Créer un deck";
				view.VM.description="Nous pouvons désormais créer un premier deck. C'est à dire déterminer un ensemble de 5 cartes qui seront utilisées en situation de combat. Commençons par cliquer sur 'nouveau' et donnons un nom à ce deck";
				this.setUpArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=0.16f*Screen.width-(arrowWidth/2f);
			arrowY=0.145f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.32f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 212:
			if(!isResizing)
			{
				MyGameController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=false;
				view.VM.title="Sélectionner des cartes";
				view.VM.description="A l'aide du clic gauche vous pouvez faire basculer les cartes vers le deck (et inversement). Votre deck sera terminé lorsqu'il sera constitué de 5 cartes";
			}
			popUpWidth=0.22f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.78f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 213:
			if(!isResizing)
			{
				MyGameController.instance.setButtonGui(0,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les filtres";
				view.VM.description="A terme, lorsque vous posséderez beaucoup de cartes, les filtres vous seront très utiles pour retrouver vos meilleures cartes et organiser vos decks";
				this.setRightArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			arrowX=0.80f*(Screen.width - 15)+10-arrowWidth;;
			arrowY=0.5f*Screen.height-(arrowHeight/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawRightArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 214:
			if(!isResizing)
			{
				MenuController.instance.setButtonGui(5,true);
				MyGameController.instance.setButtonsGui(false);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Allons jouer !";
				view.VM.description="Maintenant que vous avez constitué votre jeu il est temps de commencer votre premier match !";
				this.setUpArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=0.72f*Screen.width-arrowWidth/2f;
			arrowY=0.08f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.25f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 300:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				LobbyController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le lobby";
				view.VM.description="Bienvenue dans le lobby. C'est un écran d'avant match qui vous permet de choisir le deck avec lequel vous souhaitez commencer le combat. Vous choisirez également le type de rencontre que vous souhaitez débuter. Les matchs de coupe et de division sont classés, ils vous feront gagner davantage de crédit et auront une influence sur votre classement général";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.325f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 301:
			if(!isResizing)
			{
				LobbyController.instance.setButtonGui(1,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Prêt pour le match";
				view.VM.description="Nous alllons commencer par un petit match amical ! Go !";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=1f/6f *(Screen.width - 20f)+5-arrowWidth/2f;
			arrowY=0.45f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.25f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 500:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				if(MarketController.instance.getNbCardsDisplayed()>0)
				{
					view.VM.title="Le marché";
					view.VM.description="Cet espace est là pour vous permettre d'acheter les cartes mises en vente par d'autres joueurs. Depuis l'écran de gestion de votre jeu, vous pourrez aussi mettre en vente vos cartes";
				}
				else
				{
					view.VM.title="Revenez un autre jour";
					view.VM.description="Malheureusement aucune carte n'est aujourd'hui en vente. Revenez un autre jour pour découvrir le marché.";
				}
			}
			MarketController.instance.setGUI(false);
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 501:
			if(MarketController.instance.getNbCardsDisplayed()>0)
			{
				if(!isResizing)
				{
					view.VM.displayArrow=true;
					view.VM.displayNextButton=true;
					view.VM.title="Les filtres";
					view.VM.description="A terme, lorsque vous posséderez beaucoup de cartes, les filtres vous seront très utiles pour retrouver vos meilleures cartes et organiser vos decks";
					this.setRightArrow();
				}
				else
				{
					MarketController.instance.setGUI(false);
				}
				MarketController.instance.setButtonGUI(true);
				arrowHeight=(2f/3f)*0.1f*Screen.height;
				arrowWidth=(3f/2f)*arrowHeight;
				arrowX=0.80f*(Screen.width - 15)+10-arrowWidth;;
				arrowY=0.5f*Screen.height-(arrowHeight/2f);
				view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
				this.drawRightArrow();
				popUpWidth=0.35f*Screen.width;
				popUpHeight=this.computePopUpHeight();
				popUpX=arrowX-0.01f*Screen.width-popUpWidth;
				popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
				view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			}
			else
			{
				StartCoroutine(MarketController.instance.endTutorial(false));
			}
			break;
		case 502:
			if(!isResizing)
			{
				MarketController.instance.initializeFilters();
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les cartes en vente";
				view.VM.description="Tant que vos crédits vous le permettent, vous pouvez acheter les cartes sur le marché. Un clic droit sur la carte pourra vous donner davantage de précisions";
				this.setLeftArrow();
			}
			MarketController.instance.setGUI(false);
			cardPosition = MarketController.instance.getCardsPosition(0);
			cardSize = MarketController.instance.getCardsSize(0);
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			arrowX=cardPosition.x+1*(cardSize.x/2f);
			arrowY=Screen.height-cardPosition.y-(arrowHeight/2f);;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawLeftArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth+0.01f*Screen.width;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 503:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="A vous de jouer";
				view.VM.description="Parcourez les cartes et faites de bonnes affaires ! Bonne chance !";
			}
			else
			{
				MarketController.instance.setGUI(false);
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 504:
			StartCoroutine(MarketController.instance.endTutorial(true));
			break;
		case 600:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				ProfileController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Votre profil de joueur";
				view.VM.description="Bienvenue sur votre profil. Retrouvez ici tous vos amis, vos statistiques ainsi que les trophées remportées";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 601 :
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Vos informations personnelles";
				view.VM.description="Sur l'encart de gauche vous retrouvez vos informations et votre photo, à tout moment vous pouvez les modifier.";
				this.setLeftArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=185;
			}
			else
			{
				arrowX = Screen.height/3+5;
			}
			arrowY=(0.40f*Screen.height);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawLeftArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth+0.01f*Screen.width;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 602:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Vos amis";
				view.VM.description="La partie centrale est dédiée à vos amis. En ajoutant un ami, vous retrouverez sur votre page d'accueil des informations sur son activité dans le jeu (compétitions, amis)";
				this.setUpArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=0.5f*Screen.width-(arrowWidth/2f);
			arrowY=0.35f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 603:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Gérer vos amis";
				view.VM.description="Vous retrouvez les invitations envoyés";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=(Screen.width/2f-5f-190f)/2f + 190f-arrowWidth/2f;
			}
			else
			{
				arrowX = (Screen.width/2f-5f-(Screen.height/3f+10f))/2f + (Screen.height/3f+10f)-arrowWidth/2f;
			}
			arrowY=0.45f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 604:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Gérer vos amis";
				view.VM.description="ainsi que les demandes des autres joueurs";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=(Screen.width/2f-5f-190f)/2f + Screen.width/2f+5f-arrowWidth/2f;
			}
			else
			{
				arrowX =(Screen.width/2f-5f-(Screen.height/3f+10f))/2f + Screen.width/2f+5f-arrowWidth/2f;
			}
			arrowY=0.45f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 605:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Vos statistiques";
				view.VM.description="Vous retrouvez vos statistiques en compétition ainsi que l'état d'avancement de votre collection. En cliquant sur 'Ma collection' vous retrouverez votre niveau d'acquisition des compétences";
				this.setRightArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=Screen.width-185f-arrowWidth;
			}
			else
			{
				arrowX = Screen.width-(Screen.height/3f+5f)-arrowWidth;
			}
			arrowY=0.3f*Screen.height-(arrowHeight/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawRightArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 606:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Vos trophées";
				view.VM.description="C'est dans cette vitrine que vous retrouverez les trophées au cours du jeu";
				this.setRightArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=Screen.width-185f-arrowWidth;
			}
			else
			{
				arrowX = Screen.width-(Screen.height/3f+5f)-arrowWidth;
			}
			arrowY=0.7f*Screen.height-(arrowHeight/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawRightArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 607:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="A vous de jouer";
				view.VM.description="Commencez par mettre à jour vos informations ou bien essayez d'ajouter de nouveaux amis";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 608:
			StartCoroutine(ProfileController.instance.endTutorial());
			break;
		case 700:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				ProfileController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le profil d'un joueur";
				view.VM.description="En consultant le profil d'un autre joueur vous accédez à ces statistiques ainsi qu'à la liste de ses amis";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 701:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Ajouter un ami";
				view.VM.description="Pour ajouter ou retirer le joueur de vos amis, des boutons sont disponibles en haut à droite du profil";
				this.setRightArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=Screen.width-185f-arrowWidth;
			}
			else
			{
				arrowX = Screen.width-(Screen.height/3f+5f)-arrowWidth;
			}
			arrowY=0.15f*Screen.height-(arrowHeight/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawRightArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 702:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="A vous de jouer";
				view.VM.description="Consulter les profils de vos adversaires et tenez vous informé de leur progression";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 703:
			StartCoroutine(ProfileController.instance.endTutorial());
			break;
		case 800:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				SkillBookController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le livre des compétences";
				view.VM.description="Le livre des compétences est là pour vous donner une indication sur votre niveau de collection des compétences. Vous atteindrez le niveau ultime lorsque vous posséderez la totalité des compétences à leur niveau maximum, c'est à dire 100. Dans ce livre vous trouverez également pour chaque classe la totalité des compétences, y compris celle que vous ne possédez pas. Bonne lecture !";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 801:
			StartCoroutine(SkillBookController.instance.endTutorial());
			break;
		case 900:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				StoreController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Bienvenue dans le magasin";
				view.VM.description="Le magasin est l'unique lieu ou il vous est possible d'acquérir de nouvelles cartes. Ces cartes pourront être achété grâce à la monnaie virtuelle du jeu, vous pouvez gagner des crédits en les achetant ou bien en remportant des combats.";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 901:
			if(!isResizing)
			{
				StoreController.instance.setButtonGui(0,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Votre premier pack";
				view.VM.description="Grâce aux gains obtenus lors de votre premier match, vous allez pouvoir acheter votre première carte. Cliquez sur le bouton 'Acheter'";
				this.setLeftArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			arrowX=(Screen.width-Screen.height)/5f+Screen.height/4f;
			arrowY=0.69f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawLeftArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth+0.01f*Screen.width;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 902:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayRect=false;
			}
			break;
		case 903:
			if(!isResizing)
			{
				StoreController.instance.setGUI(false);
				StoreController.instance.setExitButtonGui(true);
				view.VM.displayRect=true;
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Bravo !";
				view.VM.description="Vous venez d'acheter votre première carte ! Retournons à la boutique.";
				this.setDownArrow();
			}
			cardPosition = StoreController.instance.getCardsPosition();
			cardSize = StoreController.instance.getCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=cardPosition.x+1*(cardSize.x/2f)-(arrowWidth/2f)+cardSize.x/4f;
			arrowY=Screen.height-cardPosition.y+0.70f*(cardSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 904:
			if(!isResizing)
			{
				StoreController.instance.setGUI(true);
				StoreController.instance.setButtonsGui(false);
				view.VM.displayRect=true;
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Des crédits supplémentaires";
				view.VM.description="Même si les gains en match vous permettront d'acquérir n'importe quelle carte, n'oubliez pas que vous avez toujours la possibilité d'alimenter votre portefeuille";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=Screen.width/2f-arrowWidth/2f;
			arrowY=0.8f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 905:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				MenuController.instance.setButtonGui(2,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Félicitations";
				view.VM.description="Vous avez terminé ce premier tutoriel. Vous pouvez désormais retourner à l'écran de gestion de vos cartes pour améliorer votre deck existant";
				this.setUpArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=0.35f*Screen.width-(arrowWidth/2f);
			arrowY=0.08f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1000:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				EndGameController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Après match";
				view.VM.description="Voici l'écran d'après match, vous trouverez des statisques sur votre adversaire, et plus tard, lorsque vous disputerez des compétitions, des informations sur votre évolution";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1001:
			if(!isResizing)
			{
				MenuController.instance.setButtonGui(3,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Rendez vous à la boutique";
				view.VM.description="Grâce à votre première victoire, les crédits remportés vont vous permettre d'améliorer votre jeu. Rendez vous à la boutique!";
				this.setUpArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=0.475f*Screen.width-(arrowWidth/2f);
			arrowY=0.08f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1100:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				DivisionLobbyController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'écran de division";
				view.VM.description="Cet écran montre votre évolution au sein de la division. Vous y verrez votre progression pour atteindre les divisions supérieures ainsi que vos derniers résultats";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1101:
			StartCoroutine(DivisionLobbyController.instance.endTutorial());
			break;
		case 1200:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				CupLobbyController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'écran de coupe";
				view.VM.description="Cet écran montre votre évolution au sein de la coupe. Vous y verrez votre progression au travers des différents tours ainsi que vos derniers résultats";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1201:
			StartCoroutine(CupLobbyController.instance.endTutorial());
			break;
		}
	}
	public void resize()
	{
		this.isResizing = true;
		view.VM.resize();
		this.launchSequence(sequenceID);
		this.isResizing = false;
		this.speed = Screen.width/20f;
	}
	private void setUpArrow()
	{
		this.moveHorizontal=false;
		this.inversedMove=true;
		view.VM.arrowStyle.normal.background = this.arrowTextures[0];
	}
	private void setDownArrow()
	{
		this.moveHorizontal=false;
		this.inversedMove=false;
		view.VM.arrowStyle.normal.background = this.arrowTextures[1];
	}
	private void setRightArrow()
	{
		this.moveHorizontal=true;
		this.inversedMove=true;
		view.VM.arrowStyle.normal.background = this.arrowTextures[2];
	}
	private void setLeftArrow()
	{
		this.moveHorizontal=true;
		this.inversedMove=false;
		view.VM.arrowStyle.normal.background = this.arrowTextures[3];
	}
	private void drawUpArrow()
	{
		this.translation=0.02f*Screen.height;
		this.startTranslation=view.VM.arrowRect.y;
		this.moveForward = true;
	}
	private void drawDownArrow()
	{
		this.translation=0.02f*Screen.height;
		this.startTranslation=view.VM.arrowRect.y;
		this.moveForward = true;
	}
	private void drawRightArrow()
	{
		this.translation=0.01f*Screen.width;
		this.startTranslation=view.VM.arrowRect.x;
		this.moveForward = true;
	}
	private void drawLeftArrow()
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
	public void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 101:
			StartCoroutine(HomePageController.instance.endTutorial());
			break;
		case 201: case 209: case 210: case 211: case 901: case 902: case 903:
			this.launchSequence(this.sequenceID+1);
			break;
		case 214:
			StartCoroutine(MyGameController.instance.endTutorial());
			break;
		case 905:
			StartCoroutine(StoreController.instance.endTutorial());
			break;
		case 1001:
			StartCoroutine(EndGameController.instance.endTutorial());
			break;
		}
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

