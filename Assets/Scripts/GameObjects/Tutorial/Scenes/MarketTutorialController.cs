using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketTutorialController : TutorialObjectController 
{
	public static MarketTutorialController instance;
	
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
				if(NewMarketController.instance.areSomeCardsDisplayed())
				{
					this.setPopUpTitle("Le marché");
					this.setPopUpDescription("Cet espace est là pour vous permettre d'acheter les cartes mises en vente par d'autres joueurs. Depuis l'écran de gestion de votre jeu, vous pourrez aussi mettre en vente vos cartes");
				}
				else
				{
					this.setPopUpTitle("Revenez un autre jour");
					this.setPopUpDescription("Malheureusement aucune carte n'est aujourd'hui en vente. Revenez un autre jour pour découvrir le marché.");
				}
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),1f,1f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(NewMarketController.instance.areSomeCardsDisplayed())
			{
				if(!isResizing)
				{
					this.displayArrow(false);
					this.displayPopUp(0);
					this.displayNextButton(true);
					this.setPopUpTitle("Les filtres");
					this.setPopUpDescription("Lorsque beaucoup de cartes sont en vente, les filtres vous seront très utiles pour retrouver les cartes qui vous intéressent");
					this.displayBackground(true);
				}
				Vector3 filtersPosition = NewMarketController.instance.getFiltersPosition();
				this.resizeBackground(new Rect(filtersPosition.x,filtersPosition.y,5f,10f),0f,0f);
				this.resizePopUp(new Vector3(0,0,-9.5f));
			}
			else
			{
				if(!isResizing)
				{
					this.displayPopUp(-1);
					this.displayBackground(false);	
				}
				StartCoroutine(NewMarketController.instance.endTutorial(false));
			}
			break;
		case 2:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Les cartes en vente");
				this.setPopUpDescription("Tant que vos crédits vous le permettent, vous pouvez acheter les cartes sur le marché. Un clic droit sur la carte pourra vous donner davantage de précisions");
				this.displayBackground(true);
			}
			Vector3 cardPosition = NewMarketController.instance.getCardsPosition(0);
			this.resizeBackground(new Rect(cardPosition.x,cardPosition.y,3f,3f),0f,0f);
			this.drawLeftArrow();
			break;
		case 3:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("A vous de jouer");
				this.setPopUpDescription("Parcourez les cartes et faites de bonnes affaires ! Bonne chance !");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,10f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 4:
			StartCoroutine(NewMarketController.instance.endTutorial(true));
			break;
		}
	}
	public override void actionIsDone()
	{
	}
}

