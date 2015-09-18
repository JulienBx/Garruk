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
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Mes recrues");
				this.setPopUpDescription("Cet espace vous permet d'accéder à l'ensemble de vos recrues et de constituer des équipes pour participer aux combats de Cristalia\n\nLes recrues sont présentées sous forme de cartes résumant leurs caractéristiques et compétences.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Zoomer sur une recrue");
				this.setPopUpDescription("Pour examiner une recrue, cliquez avec le bouton droit de la souris sur sa carte.");
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
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Recrues et cartes");
				this.setPopUpDescription("La carte représente les caractéristiques et compétences de la recrue.\n\nChaque cristalien appartient à une faction, les factions ayant chacune développé des compétences distinctes au contact du Cristal.\n\nRegardons maintenant de plus près les informations affichées sur la carte.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
			break;
		case 3:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points de vie");
				this.setPopUpDescription("La zone inférieure présente les caractéristiques de la carte. Au centre sont affichés les points de vie. Chaque unité dispose de ses propres points de vie, et est anéantie pendant un combat quand ses points de vie tombent à 0.\n\nUne unité anéantie pendant un combat reste votre propriété et sera disponible pour le combat suivant (moyennant un passage éclair à l'hopital galactique de Cristalia, réputé pour ses caissons de récupération à haute vitesse)");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 healthPointsPosition = newMyGameController.instance.getFocusedCardHealthPointsPosition();
			this.resizeBackground(new Rect(healthPointsPosition.x,healthPointsPosition.y,2.5f,2.5f),0f,0f);
			this.drawDownArrow();
			break;
		case 4:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points d'attaque");
				this.setPopUpDescription("Les points d'attaque définissent les dégats de base infligés par les unités pendant une attaque. Ces points influencent également de nombreuses compétences, le Cristal catalysant la puissance de chacun.");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 attackPointsPosition = newMyGameController.instance.getFocusedCardAttackPointsPosition();
			this.resizeBackground(new Rect(attackPointsPosition.x,attackPointsPosition.y,2.5f,2.5f),0f,0f);
			this.drawDownArrow();
			break;
		case 5:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Les points de rapidité");
				this.setPopUpDescription("La rapidité détermine l'ordre des tours pendant un combat. Un personnage puisant mais lent n'aura peut-etre meme pas le temps d'utiliser ses compétences avant d'etre neutralisé. <i>Une équipe d'unités rapides peut désorganiser meme les équipes les plus fortes</i>, De l'art du Blietzkrieg, Caradec - célèbre militaire Cristalien célèbre.");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 quicknessPointsPosition = newMyGameController.instance.getFocusedCardQuicknessPointsPosition();
			this.resizeBackground(new Rect(quicknessPointsPosition.x,quicknessPointsPosition.y,2.5f,2.5f),0f,0f);
			this.drawDownArrow();
			break;
		case 6:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Spécialité de faction");
				this.setPopUpDescription("Au sein de chaque faction existent plusieurs spécialités. Ces spécialités confèrent des bonus importants aux caractéristiques ou aux compétences, et sont déclenchées automatiquement pendant les combats");
				this.displayBackground(true);
				this.setUpArrow();
			}
			Vector3 skill0Position = newMyGameController.instance.getFocusedCardSkill0Position();
			this.resizeBackground(new Rect(skill0Position.x,skill0Position.y,8f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 7:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Les compétences");
				this.setPopUpDescription("Les compétences sont les savoirs-faire développés par les habitants de Cristalia pendant leur séjour au centre d'entrainement urbain. Nul ne sait exactement prédire exactement les effets du Cristal sur les habitants, ce qui rend chaque compétence unique et plus ou moins puissante.");
				this.displayBackground(true);
				this.setDownArrow();
			}
			Vector3 skillsPosition = newMyGameController.instance.getFocusedCardSkill1Position();
			this.resizeBackground(new Rect(skillsPosition.x,skillsPosition.y,8f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 8:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("L'expérience de la carte");
				this.setPopUpDescription("Chaque Cristalien engrange de l'expérience au fur et à mesure qu'il combat dans vos équipes. Les points d'expérience permettent de débloquer de nouvelles compétences et d'améliorer les caractéristiques d'une carte.\n\nUne recrue semblant faible peut en fait potentiellement développer des compétences extremement puissantes au fil de sa vie dans vos équipes.");
				this.displayBackground(true);
				this.setUpArrow();
			}
			Vector3 experiencePosition = newMyGameController.instance.getFocusedCardExperienceLevelPosition();
			this.resizeBackground(new Rect(experiencePosition.x,experiencePosition.y,3f,1f),0f,0f);
			this.drawUpArrow();
			break;
		case 9:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.displayNextButton(false);
				this.setPopUpTitle("Centre d'entrainement urbain");
				this.setPopUpDescription("Vos recrues peuvent etre envoyées au centre d'entrainement urbain pour acquérir de l'expérience plus vite.\n\nCes séjours payants vous permettent d'accélérer le développement de vos unités et de ne pas envoyer la bleusaille au combat\n\n<b>Envoyez votre recrue au centre d'entrainement pour qu'elle acquiert un niveau d'expérience!</b>");
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
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Félicitations");
				this.setPopUpDescription("Votre recrue vous revient entrainée et améliorée!\n\nVous disposez de nombreuses autres actions pour gérer vos unités : les licencier (vous récupérez alors le Cristal qui les équipait), les mettre à disposition sur le marché des mercenaires, etc.");
				this.displayBackground(true);
			}
			Vector3 experiencePosition2 = newMyGameController.instance.getFocusedCardExperienceGaugePosition();
			this.resizeBackground(new Rect(experiencePosition2.x,experiencePosition2.y,8f,1.5f),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 11:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Mes unités");
				this.setPopUpDescription("Revenons à vos unités");
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
				this.displayPopUp(2);
				this.displayNextButton(false);
				this.setPopUpTitle("Créer une équipe");
				this.setPopUpDescription("Vos recrues peuvent etre organisées en plusieurs équipes de 4 Cristaliens. Ces équipes sont constituées en accord avec le code de Guerre de Cristalia créés après la guerre d'indépendance de la planète.\n\nCe réglement stipule que pour éviter les trop nombreux décès (57% des habitants de la planète ont été décimés pendant la guerre), les combats entre colons s'effectueront désormais entre équipes de 4 cristaliens");
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
				this.setPopUpTitle("Ajouter des recrues à son équipe");
				this.setPopUpDescription("Cliquez sur les recrues proposées pour créer votre première équipe");
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
				this.setPopUpDescription("Des filtres sur le coté vous permettront de trier vos recrues selon différents critères (caractéristiques, compétences, etc.). Utile pour gérer de puissantes armées! Krigeff, général cristalien disposant à l'époque d'une armée de 1700 recrues, a sombré dans la folie le jour ou il a malencontreusement désinstallé son logiciel de filtrage");
				this.displayBackground(true);
			}
			Vector3 filtersPosition = newMyGameController.instance.getFiltersPosition();
			this.resizeBackground(new Rect(filtersPosition.x,filtersPosition.y,5f,10f),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 18:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Carière militaire");
				this.setPopUpDescription("Votre équipe est maintenant constituée et prete à combattre! Entrons sur le champ de bataille");
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
				this.setPopUpTitle("Entrainement");
				this.setPopUpDescription("Commençons par un combat d'entrainement. Les combats officiels rapportent plus de Cristal et d'expérience mais sont également plus difficiles ");
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
		case 1: case 11: case 12: case 13: case 14: case 15: case 16: case 18:
			this.launchSequence(this.sequenceID+1);
			break;
		case 9: 
			newMyGameController.instance.setTutorialStep();
			this.launchSequence(this.sequenceID+1);
			break;
		case 19:
			newMyGameController.instance.endTutorial();
			break;
		
		}
	}
}

