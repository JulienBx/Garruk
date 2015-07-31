using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameTutorialController : TutorialObjectController 
{
	public static MyGameTutorialController instance;
	
	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				//MyGameController.instance.setButtonsGui(false);
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
		case 1:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Zoom sur une carte";
				view.VM.description="Pour examiner une carte, cliquez avec le bouton droit de la souris sur son visuel.";
				this.setUpArrow();
			}
			//GOPosition = MyGameController.instance.getCardsPosition(3);
			//GOSize = MyGameController.instance.getCardsSize(3);
			view.VM.popUpRect= new Rect (GOPosition.x-0.15f*Screen.width,
			                             Screen.height-GOPosition.y+GOSize.y/2f+0.12f*Screen.height,
			                             0.3f*Screen.width,0.5f*Screen.height);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x+0*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+1f*(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 2:
			if(!isResizing)
			{
				//MyGameController.instance.setGUI(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Comprendre le visuel d'une carte";
				view.VM.description="Chaque comprend une illustration qui correspond à sa classe ainsi qu'un certain nombre d'éléments que nous allons détailler maintenant. Ces éléments sont variables en fonction de la classe de la carte";
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			popUpWidth=0.58f*GOSize.x;
			popUpHeight=this.computePopUpHeight();
			popUpX=GOPosition.x+GOSize.x/2f;
			popUpY=Screen.height-GOPosition.y-GOSize.y/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 3:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points de vie";
				view.VM.description="La zone supérieure droite de la carte donne son nombre de points de vie. Ces points varient au cours du combat en fonction des dégâts qui sont affligés à la créature. Lorsque ces points atteignent 0, la créature meurt.";
				this.setUpArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x+0.60f*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-0.76f*(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			view.VM.popUpRect= new Rect (0.03f*Screen.width,0.25f*Screen.height,0.3f*Screen.width,0.6f*Screen.height);
			this.drawUpArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 4:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points d'attaque";
				view.VM.description="Ils indiquent les dégats qui seront causés par le personnage lors d'une simple attaque. 10 points de dégâts se traduisent par une perte de 10 points de vie sur la carte ciblée par l'attaque.";
				this.setDownArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-0.66f*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+0.84f*(GOSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 5:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points de rapidité";
				view.VM.description="Ce sont ces points là qui déterminent l'ordre de jeu entre les différents personnages. Plus ces points sont élevés plus la carte à de chance de jouer en premier.";
				this.setDownArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-0f*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+0.84f*(GOSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 6:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les points de déplacement";
				view.VM.description="Ils indiquent tout simplement le nombre de pas autorisé par la carte. Seule les déplacement verticaux et horizontaux sont possibles.";
				this.setDownArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x+0.63f*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+0.84f*(GOSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 7:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Les compétences";
				view.VM.description="Il s'agit des sorts pouvant être lancés par la carte en situation de combat. Chaque sort est évalué de 0 à 100. En passant la souris sur une compétence vous pouvez lire ses effets";
				this.setDownArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-0f*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+0.3f*(GOSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 8:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="L'expérience de la carte";
				view.VM.description="Au cours de sa vie, une carte progresse en fonction de son expérience. A chaque niveau, une de ses caractèristique est augmentée. Il y a en tout 10 niveau. La jauge symbolise le niveau de progression de la carte sur son nivau";
				this.setDownArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x+0.72f*(GOSize.x/2f)-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-0.05f*(GOSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 9:
			if(!isResizing)
			{
				//MyGameController.instance.setButtonGuiOnFocusedCard(0,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Faire progresser la carte";
				view.VM.description="A tout moment et en fonction de vos crédits il vous est possible d'augmenter le niveau d'une carte. A vous de jouer, faites progresser cette carte jusqu'au prochain niveau";
				this.setUpArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x+1*(GOSize.x/2f)-(arrowWidth/2f)+GOSize.x/4f;
			arrowY=Screen.height-GOPosition.y-0.35f*(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 10:
			if(!isResizing)
			{
				//MyGameController.instance.setGUI(false);
				//MyGameController.instance.setButtonGuiOnFocusedCard(1,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Bravo !";
				view.VM.description="Vous verrez que d'autres actions sont possible comme la vente ou le renommage. Retournons à l'écran d'affichage de vos cartes";
				this.setDownArrow();
			}
			//GOPosition = MyGameController.instance.getFocusCardsPosition();
			//GOSize = MyGameController.instance.getFocusCardsSize();
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x+1*(GOSize.x/2f)-(arrowWidth/2f)+GOSize.x/4f;
			arrowY=Screen.height-GOPosition.y+0.70f*(GOSize.y/2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 11:
			if(!isResizing)
			{
				//MyGameController.instance.setGUI(true);
				//MyGameController.instance.setButtonsGui(false);
				//MyGameController.instance.setButtonGui(1,true);
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
		case 12:
			if(!isResizing)
			{
				//MyGameController.instance.setButtonsGui(false);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Sélectionner des cartes";
				view.VM.description="A l'aide du clic gauche vous pouvez faire basculer les cartes vers le deck (et inversement). Votre deck sera terminé lorsqu'il sera constitué de 5 cartes";
				this.setUpArrow();			
			}
			//GOPosition = MyGameController.instance.getCardsPosition(0);
			//GOSize = MyGameController.instance.getCardsSize(0);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.22f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.78f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 13:
			if(!isResizing)
			{
				//MyGameController.instance.setButtonsGui(false);
			}
			//GOPosition = MyGameController.instance.getCardsPosition(2);
			//GOSize = MyGameController.instance.getCardsSize(2);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.22f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.78f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 14:
			if(!isResizing)
			{
				//MyGameController.instance.setButtonsGui(false);
			}
			//GOPosition = MyGameController.instance.getCardsPosition(1);
			//GOSize = MyGameController.instance.getCardsSize(1);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.22f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.78f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 15:
			if(!isResizing)
			{
				//MyGameController.instance.setButtonsGui(false);
			}
			//GOPosition = MyGameController.instance.getCardsPosition(0);
			//GOSize = MyGameController.instance.getCardsSize(0);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y+(GOSize.y/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawUpArrow();
			popUpWidth=0.22f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.78f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 16:
			if(!isResizing)
			{
				//MyGameController.instance.setButtonsGui(false);
				//MyGameController.instance.setButtonGui(0,true);
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
		case 17:
			if(!isResizing)
			{
				MenuController.instance.setButtonGui(5,true);
				//MyGameController.instance.setButtonsGui(false);
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
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1: case 9: case 10: case 11: case 12: case 13: case 14: case 15:
			this.launchSequence(this.sequenceID+1);
			break;
		case 17:
			//StartCoroutine(MyGameController.instance.endTutorial());
			break;
		}
	}
}

