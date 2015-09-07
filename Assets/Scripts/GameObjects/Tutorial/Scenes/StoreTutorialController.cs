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
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Bienvenue dans le magasin");
				this.setPopUpDescription("Le magasin est l'unique lieu ou il vous est possible d'acquérir de nouvelles cartes. Ces cartes pourront être achété grâce à la monnaie virtuelle du jeu, vous pouvez gagner des crédits en les achetant ou bien en remportant des combats.");
				this.displayBackground(true);
				
			}
			this.resizeBackground(new Rect(0,0,0,0),1f,1f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Votre premier pack");
				this.setPopUpDescription("Grâce aux gains obtenus lors de votre premier match, vous allez pouvoir acheter votre première carte. Cliquez sur le bouton 'Acheter'");
				this.displayBackground(true);
			}
			Vector3 packPosition = NewStoreController.instance.getFirstPackPosition();
			this.resizeBackground(new Rect(packPosition.x,packPosition.y,4.5f,5f),0.7f,0.7f);
			this.drawLeftArrow();
			break;
		case 2:
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.displayBackground(false);
				this.displayArrow(false);
			}
			break;
		case 3:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayArrow(false);
				this.displayNextButton(true);
				this.setPopUpTitle("Bravo !");
				this.setPopUpDescription("Vous venez d'acheter votre première carte !");
				this.displayBackground(true);
			}
			Vector3 focusedCardPosition = NewStoreController.instance.getFocusedCardPosition();
			this.resizeBackground(new Rect(focusedCardPosition.x,focusedCardPosition.y,8f,9f),0f,0f);
			this.resizePopUp(new Vector3(focusedCardPosition.x+3f,focusedCardPosition.y+1,-4f));
			break;
		case 4:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Retour au magasin");
				this.setPopUpDescription("Retournons au magasin");
				this.displayBackground(true);
			}
			Vector3 feature5Position = NewStoreController.instance.getFocusedCardFeaturePosition(5);
			this.resizeBackground(new Rect(feature5Position.x,feature5Position.y,3f,2f),0.8f,0.6f);
			this.drawDownArrow();
			break;
		case 5:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Des crédits supplémentaires");
				this.setPopUpDescription("Même si les gains en match vous permettront d'acquérir n'importe quelle carte, n'oubliez pas que vous avez toujours la possibilité d'alimenter votre portefeuille");
				this.displayBackground(true);
			}
			Vector3 buyCreditsButtonPosition = NewStoreController.instance.getBuyCreditsButtonPosition();
			this.resizeBackground(new Rect(buyCreditsButtonPosition.x,buyCreditsButtonPosition.y,5f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 6:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Félicitations");
				this.setPopUpDescription("Vous avez terminé ce premier tutoriel");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),1f,1f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 7:
			StartCoroutine(NewStoreController.instance.endTutorial());
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1: case 2: case 4: 
			this.launchSequence(this.sequenceID+1);
			break;
		}
	}
}

