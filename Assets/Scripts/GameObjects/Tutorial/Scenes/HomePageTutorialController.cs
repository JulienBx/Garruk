using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

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
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Bienvenue");
				this.setPopUpDescription("Vous êtes ici sur la page d'accueil. Dernières cartes vendues, actualités des amis, compétitions en cours ou bien cartes en vente à la boutique, vous avez tout ici en un clin d'oeil !");
				this.displayBackground(true);

			}
			this.resizeBackground(new Rect(0,0,0,0),0f,10f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Vite des cartes !");
				this.setPopUpDescription("Nous allons commencer par découvrir vos cartes. ");
				this.displayBackground(true);

			}
			Vector3 buttonPosition1 = newMenuController.instance.getButtonPosition(1);
			this.resizeBackground(new Rect(buttonPosition1.x+1f,buttonPosition1.y,3f,2f),1f,0.45f);
			this.drawLeftArrow();
			break;
		case 2:
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.displayBackground(false);	
			}
			break;
		case 3:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Une récompense");
				this.setPopUpDescription("Allons à la boutique découvrir ce que nous avons gagné");
				this.displayBackground(true);
				
			}
			Vector3 buttonPosition2 = newMenuController.instance.getButtonPosition(2);
			this.resizeBackground(new Rect(buttonPosition2.x+1f,buttonPosition2.y,3f,2f),1f,0.45f);
			this.drawLeftArrow();
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1:
			StartCoroutine(NewHomePageController.instance.endTutorial());
			break;
		case 2: 
			this.launchSequence(this.sequenceID+1);
			break;
		case 3:
			StartCoroutine(NewHomePageController.instance.endTutorial());
			break;
		}
	}
}

