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
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Centre de recrutement");
				this.setPopUpDescription("Le centre de recrutement vous permet de recruter de nouveaux Cristaliens. Les unités se présentent spontanément au centre de recrutement et sont uniques!\n\nElles ne peuvent pas etre consultées avant de les recruter, il vous faudra donc recruter beaucoup de Cristaliens avant de trouver des perles rares.\n\nMoyennant un prix plus élevé, les responsables du centre pourront sélectionner des candidats selon leur faction, facilitant votre travail de recrutement.");
				this.displayBackground(true);
				
			}
			this.resizeBackground(new Rect(0,0,0,0),1f,1f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Première recrue");
				this.setPopUpDescription("le Cristal obtenu lors de votre combat face à Garruk vous permet de recruter un Cristalien. <i>Cliquez sur le Cristalien</i>");
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
				this.setPopUpDescription("Vous venez d'acheter votre première carte ! Selon leur puissance les Cristaliens sont affichés sur un fond argenté (les moins puissants), bleu (puissantes), et rouges (très puissantes)");
				this.displayBackground(true);
			}
			Vector3 focusedCardPosition = NewStoreController.instance.getFocusedCardPosition();
			this.resizeBackground(new Rect(focusedCardPosition.x,focusedCardPosition.y,8f,9f),0f,0f);
			this.resizePopUp(new Vector3(focusedCardPosition.x+3f,focusedCardPosition.y+1,-9.5f));
			break;
		case 4:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Retour");
				this.setPopUpDescription("Retournons au centre de recrutement");
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
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 7:
			NewStoreController.instance.endTutorial();
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1: case 4: 
			this.launchSequence(this.sequenceID+1);
			break;
		case 2:
			NewStoreController.instance.setTutorialStep();
			this.launchSequence(this.sequenceID+1);
			break;
		}
	}
}

