using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageTutorialController : TutorialObjectController 
{
	public static HomePageTutorialController instance;
	
	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
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
		case 1:
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
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1:
			StartCoroutine(HomePageController.instance.endTutorial());
			break;
		}
	}
}

