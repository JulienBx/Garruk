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
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Vos cartes");
				this.setPopUpDescription("Dans cet espaces vous allez pouvoir gérer vos cartes et créer vos desck que vous utiliserez en cours de match.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Zoom sur une carte");
				this.setPopUpDescription("Pour examiner une carte, cliquez avec le bouton droit de la souris sur son visuel.");
				this.displayBackground(true);
			}
			Vector3 cardPosition = newMyGameController.instance.getCardsPosition(0);
			this.resizeBackground(new Rect(cardPosition.x,cardPosition.y,3f,3f),0.55f,0.7f);
			this.drawLeftArrow();
			break;
		case 2:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Comprendre le visuel d'une carte");
				this.setPopUpDescription("Chaque comprend une illustration qui correspond à sa classe ainsi qu'un certain nombre d'éléments que nous allons détailler maintenant. Ces éléments sont variables en fonction de la classe de la carte");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
			break;
		case 3:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points de vie");
				this.setPopUpDescription("La zone supérieure droite de la carte donne son nombre de points de vie. Ces points varient au cours du combat en fonction des dégâts qui sont affligés à la créature. Lorsque ces points atteignent 0, la créature meurt.");
				this.displayBackground(true);
				this.setLeftArrow();
			}
			Vector3 healthPointsPosition = newMyGameController.instance.getFocusedCardHealthPointsPosition();
			this.resizeBackground(new Rect(healthPointsPosition.x,healthPointsPosition.y,2f,2.5f),0f,0f);
			this.drawLeftArrow();
			break;
		case 4:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points d'attaque");
				this.setPopUpDescription("Ils indiquent les dégats qui seront causés par le personnage lors d'une simple attaque. 10 points de dégâts se traduisent par une perte de 10 points de vie sur la carte ciblée par l'attaque.");
				this.displayBackground(true);
				this.setLeftArrow();
			}
			Vector3 attackPointsPosition = newMyGameController.instance.getFocusedCardAttackPointsPosition();
			this.resizeBackground(new Rect(attackPointsPosition.x,attackPointsPosition.y,2f,2.5f),0f,0f);
			this.drawLeftArrow();
			break;
		case 5:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points de rapidité");
				this.setPopUpDescription("Ce sont ces points là qui déterminent l'ordre de jeu entre les différents personnages. Plus ces points sont élevés plus la carte à de chance de jouer en premier.");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 quicknessPointsPosition = newMyGameController.instance.getFocusedCardQuicknessPointsPosition();
			this.resizeBackground(new Rect(quicknessPointsPosition.x,quicknessPointsPosition.y,2f,2.5f),0f,0f);
			this.drawDownArrow();
			break;
		case 6:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points de déplacement");
				this.setPopUpDescription("Ils indiquent tout simplement le nombre de pas autorisé par la carte. Seule les déplacement verticaux et horizontaux sont possibles.");
				this.displayBackground(true);
				this.setLeftArrow();
			}
			Vector3 movePointsPosition = newMyGameController.instance.getFocusedCardMovePointsPosition();
			this.resizeBackground(new Rect(movePointsPosition.x,movePointsPosition.y,2f,2.5f),0f,0f);
			this.drawLeftArrow();
			break;
		case 7:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les compétences");
				this.setPopUpDescription("Il s'agit des sorts pouvant être lancés par la carte en situation de combat. Chaque sort est évalué de 0 à 100. En passant la souris sur une compétence vous pouvez lire ses effets");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 skillsPosition = newMyGameController.instance.getFocusedCardSkillsPosition();
			this.resizeBackground(new Rect(skillsPosition.x,skillsPosition.y,7f,4.5f),0f,0f);
			this.drawDownArrow();
			break;
		case 8:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("L'expérience de la carte");
				this.setPopUpDescription("Au cours de sa vie, une carte progresse en fonction de son expérience. A chaque niveau, une de ses caractèristique est augmentée. Il y a en tout 10 niveau. La jauge symbolise le niveau de progression de la carte sur son nivau");
				this.displayBackground(true);
				this.setUpArrow();
			}
			Vector3 experiencePosition = newMyGameController.instance.getFocusedCardExperiencePosition();
			this.resizeBackground(new Rect(experiencePosition.x,experiencePosition.y,7f,1f),0f,0f);
			this.drawUpArrow();
			break;
		case 9:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Faire progresser la carte");
				this.setPopUpDescription("A tout moment et en fonction de vos crédits il vous est possible d'augmenter le niveau d'une carte. A vous de jouer, faites progresser cette carte jusqu'au prochain niveau");
				this.displayBackground(true);
				this.setRightArrow();
			}
			Vector3 feature1Position = newMyGameController.instance.getFocusedCardFeaturePosition(1);
			this.resizeBackground(new Rect(feature1Position.x,feature1Position.y,3f,2f),0.8f,0.6f);
			this.drawRightArrow();
			break;
		case 10:
			if(!isResizing)
			{
				
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Bravo");
				this.setPopUpDescription("Vous verrez que d'autres actions sont possible comme la vente ou le renommage.");
				this.displayBackground(true);
			}
			Vector3 experiencePosition2 = newMyGameController.instance.getFocusedCardExperiencePosition();
			this.resizeBackground(new Rect(experiencePosition2.x,experiencePosition2.y,7f,1f),0f,0f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 11:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Retour aux cartes");
				this.setPopUpDescription("Retournons à l'écran d'affichage de vos cartes");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 feature5Position = newMyGameController.instance.getFocusedCardFeaturePosition(5);
			this.resizeBackground(new Rect(feature5Position.x,feature5Position.y,3f,2f),0.8f,0.6f);
			this.drawDownArrow();
			break;
		case 12:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.displayNextButton(false);
				this.setPopUpTitle("Créer un deck");
				this.setPopUpDescription("Nous pouvons désormais créer un premier deck. C'est à dire déterminer un ensemble de 5 cartes qui seront utilisées en situation de combat. Commençons par cliquer sur 'nouveau' et donnons un nom à ce deck");
				this.displayBackground(true);
				this.setUpArrow();
			}
			Vector3 newDeckButtonPosition = newMyGameController.instance.getNewDeckButtonPosition();
			this.resizeBackground(new Rect(newDeckButtonPosition.x,newDeckButtonPosition.y,4f,0.75f),0.8f,0.8f);
			this.drawUpArrow();
			break;
		case 13:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Sélectionner des cartes");
				this.setPopUpDescription("A l'aide du clic gauche vous pouvez faire basculer les cartes vers le deck (et inversement). Votre deck sera terminé lorsqu'il sera constitué de 5 cartes");
				this.displayBackground(true);
				this.setLeftArrow();			
			}
			Vector3 firstCardPosition = newMyGameController.instance.getCardsPosition(0);
			this.resizeBackground(new Rect(firstCardPosition.x,firstCardPosition.y,3f,3f),0.55f,0.7f);
			this.drawLeftArrow();
			break;
		case 14:
			if(!isResizing)
			{
				this.displayBackground(true);
				this.setLeftArrow();			
			}
			Vector3 secondCardPosition = newMyGameController.instance.getCardsPosition(2);
			this.resizeBackground(new Rect(secondCardPosition.x,secondCardPosition.y,3f,3f),0.55f,0.7f);
			this.drawLeftArrow();
			break;
		case 15:
			if(!isResizing)
			{
				this.displayBackground(true);
				this.setLeftArrow();			
			}
			Vector3 thirdCardPosition = newMyGameController.instance.getCardsPosition(1);
			this.resizeBackground(new Rect(thirdCardPosition.x,thirdCardPosition.y,3f,3f),0.55f,0.7f);
			this.drawLeftArrow();
			break;
		case 16:
			if(!isResizing)
			{
				this.displayBackground(true);
				this.setLeftArrow();			
			}
			Vector3 fourthCardPosition = newMyGameController.instance.getCardsPosition(0);
			this.resizeBackground(new Rect(fourthCardPosition.x,fourthCardPosition.y,3f,3f),0.55f,0.7f);
			this.drawLeftArrow();
			break;
		case 17:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les filtres");
				this.setPopUpDescription("A terme, lorsque vous posséderez beaucoup de cartes, les filtres vous seront très utiles pour retrouver vos meilleures cartes et organiser vos decks");
				this.displayBackground(true);
			}
			Vector3 filtersPosition = newMyGameController.instance.getFiltersPosition();
			this.resizeBackground(new Rect(filtersPosition.x,filtersPosition.y,5f,10f),0f,0f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 18:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Allons jouer !");
				this.setPopUpDescription("Maintenant que vous avez constitué votre jeu il est temps de commencer votre premier match !");
				this.displayBackground(true);
				
			}
			Vector3 buttonPosition = newMenuController.instance.getButtonPosition(5);
			this.resizeBackground(new Rect(buttonPosition.x+1f,buttonPosition.y,3f,2f),1f,0.45f);
			this.drawLeftArrow();
			break;
		case 19:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Match amical pour commencer");
				this.setPopUpDescription("Choisisson pour démarrer un match amical, par la suite vous pourrez démarrer des parties classées en division et coupe.");
				this.displayBackground(true);
				this.setUpArrow();
			}
			Vector3 friendlyGameButtonPosition = PlayPopUpController.instance.getFriendlyGameButtonPosition();
			this.resizeBackground(new Rect(friendlyGameButtonPosition.x,friendlyGameButtonPosition.y,6f,1f),0.8f,0.8f);
			this.drawUpArrow();
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1: case 9: case 11: case 12: case 13: case 14: case 15: case 16: case 18:
			this.launchSequence(this.sequenceID+1);
			break;
		case 19:
			StartCoroutine(newMyGameController.instance.endTutorial());
			break;
		
		}
	}
}

