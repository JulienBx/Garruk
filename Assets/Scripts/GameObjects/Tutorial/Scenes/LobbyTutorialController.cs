using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LobbyTutorialController : TutorialObjectController 
{
	public static LobbyTutorialController instance;
	
	public override void endInitialization()
	{
		NewLobbyController.instance.endTutorialInitialization ();
	}
	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		default:
			base.launchSequence(this.sequenceID);
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		default:
			base.actionIsDone();
			break;
		}
	}
	public override int getStartSequenceId(int tutorialStep)
	{
		switch(tutorialStep)
		{
		default:
			return base.getStartSequenceId(tutorialStep);
			break;
		}
		return 0;
	}
	public override void endHelp()
	{
		StartCoroutine(NewLobbyController.instance.endHelp ());
		base.endHelp ();
	}
	#region HELP SEQUENCES
	
	public override void launchHelpSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		Vector3 gameObjectPosition2 = new Vector3 ();
		Vector2 gameObjectSize = new Vector2 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Encart de présentation de l'avancement dans la compétition "coupe ou division"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Conquérir un satellite");
				this.setPopUpDescription("La conquete des satellites de Cristalia vous permettra de gagner de plus en plus de cristal. Pour conquérir un satellite, il vous faudra défier de nombreux colons et les vaincre. Vous pouvez ici consulter l'état de la guerre sur le satellite sur lequel vous vous trouvez");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			
			gameObjectPosition=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getLastResultsBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getMainBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 1: // Encart de présentation de la compétition, récompenses, etc.
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Mon satellite");
				this.setPopUpDescription("La guerre fait rage sur les satellites de Cristalia. Plus vous deviendrez fort, plus vous pourrez accéder à des satellites riches en ressources. Consultez ici la richesse du satellite et les conditions d'accès au suivant");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewLobbyController.instance.getCompetitionBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getLastResultsBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getCompetitionBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 2: // Encart d'affichage des derniers résutats au sein de la compétition
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Mes derniers combats");
				this.setPopUpDescription("Accédez ici à un compte-rendu de vos derniers combats sur le satellite");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewLobbyController.instance.getLastResultsBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getLastResultsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 3: // Encart d'affichage des statistiques de l'utilisateur (sur l'ensemble des matchs classés)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Mes statistiques");
				this.setPopUpDescription("Ces statistiques vous permettront d'évaluer l'état de la conquete du satellite et votre niveau par rapport aux autres colons");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewLobbyController.instance.getStatsBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getStatsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 4: 
			this.endHelp();
			break;
		default:
			base.launchHelpSequence(this.sequenceID);
			break;
		}
	}
	
	#endregion

}

