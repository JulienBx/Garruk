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
		case 1:
			if(MarketController.instance.getNbCardsDisplayed()>0)
			{
				if(!isResizing)
				{
					view.VM.displayArrow=true;
					view.VM.displayNextButton=true;
					view.VM.title="Les filtres";
					view.VM.description="Lorsque beaucoup de cartes sont en vente, les filtres vous seront très utiles pour retrouver les cartes qui vous intéressent";
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
				if(!isResizing)
				{
					view.VM.displayArrow=false;
					view.VM.displayRect=false;
					StartCoroutine(MarketController.instance.endTutorial(false));
				}
			}
			break;
		case 2:
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
			GOPosition = MarketController.instance.getCardsPosition(0);
			GOSize = MarketController.instance.getCardsSize(0);
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			arrowX=GOPosition.x+1*(GOSize.x/2f);
			arrowY=Screen.height-GOPosition.y-(arrowHeight/2f);;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawLeftArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth+0.01f*Screen.width;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 3:
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
		case 4:
			StartCoroutine(MarketController.instance.endTutorial(true));
			break;
		}
	}
	public override void actionIsDone()
	{
	}
}

