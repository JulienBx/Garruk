using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class StoreTutorialController : TutorialObjectController 
{
	public static StoreTutorialController instance;
	
	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				//StoreController.instance.setButtonsGui(false);
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
		case 1:
			if(!isResizing)
			{
				//StoreController.instance.setButtonGui(0,true);
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
		case 2:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayRect=false;
			}
			break;
		case 3:
			if(!isResizing)
			{
				//StoreController.instance.setGUI(false);
				//StoreController.instance.setExitButtonGui(true);
				view.VM.displayRect=true;
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Bravo !";
				view.VM.description="Vous venez d'acheter votre première carte ! Retournons à la boutique.";
				this.setDownArrow();
			}
			//GOPosition = StoreController.instance.getCardsPosition();
			//GOSize = StoreController.instance.getCardsSize();
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
		case 4:
			if(!isResizing)
			{
				//StoreController.instance.setGUI(true);
				//StoreController.instance.setButtonsGui(false);
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
		case 5:
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
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1: case 2: case 3: 
			this.launchSequence(this.sequenceID+1);
			break;
		case 5:
			//StartCoroutine(StoreController.instance.endTutorial());
			break;
		}
	}
}

