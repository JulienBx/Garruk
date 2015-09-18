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
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Bienvenue sur le tableau de bord");
				this.setPopUpDescription("Cet écran d'accueil vous permettra d'accéder rapidement aux combats, au marché ou au centre de recrutement, ainsi qu'à toutes les informations utiles.\n\nAu centre, vos classements de combattant et de collectionneur vous permettent de mesurer votre progression, et vous pouvez également accéder à droite aux actualités de Cristalia et de ses habitants.");
				this.displayBackground(true);

			}
			this.resizeBackground(new Rect(0,0,0,0),0f,10f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Recruter des crystaliens");
				this.setPopUpDescription("4 Cristaliens recrutés depuis la Terre vous attendent déjà. Allons les rencontrer et découvrir leur compétences.\n\n<b>Cliquez sur le bouton 'Mon armée'</b>");
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
				this.displayPopUp(1);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Nouvelle recrue");
				this.setPopUpDescription("Le cristal que nous avons gagné va pouvoir nous permettre de recruter de nouveaux combattants!\n\nAllons au centre de recrutement essayer de nous renseigner");
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

