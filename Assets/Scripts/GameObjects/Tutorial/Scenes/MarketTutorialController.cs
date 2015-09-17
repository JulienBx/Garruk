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
				this.displayPopUp(2);
				this.displayNextButton(true);
				if(NewMarketController.instance.areSomeCardsDisplayed())
				{
					this.setPopUpTitle("Les mercenaires");
					this.setPopUpDescription("Certains colons souhaitent rompre leur contrat avec leurs crystaliens et les inscrivent en tant que mercenaire sur le marché.\n\nAccessible à tous, ce marché permet de recruter des crystaliens à des prix parfois intéressants, mais aussi de découvrir de nouvelles compétences et de parfaire sa connaissance de Crystalia");
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
					this.setPopUpTitle("Utiliser les filtres");
					this.setPopUpDescription("Les filtres permettent d'affiner la recherche et de spécifier les caractéristiques ou compétences recherchées.");
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
				this.displayPopUp(2);
				this.setLeftArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Engager un mercenaire");
				this.setPopUpDescription("Engager un mercenaire coute des cristaux, payables directement au colon.\n\nCertains colons sont passsés maitres dans l'art de gagner de grosses sommes d'argent en engageant des mercenaires à bas prix pour les vendre très cher.");
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
				this.setPopUpTitle("N'oubliez pas de revenir");
				this.setPopUpDescription("Les colons peuvent proposer des mercenaires tous les jours, revenez souvent pour ne pas manquer les bonnes affaires!");
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

