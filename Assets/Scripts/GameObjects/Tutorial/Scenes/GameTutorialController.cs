using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GameTutorialController : TutorialObjectController 
{
	public static GameTutorialController instance;
	private int playerCount;
	private Vector3 gameObjectPosition;

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
				this.displayNextButton(false);
				this.setPopUpTitle("Le mode match");
				this.setPopUpDescription("Pret pour votre premier duel ! Le match se joue sur un damier sur lequel vous pouvez déplacer vos cartes. Vous démarrez la partie en choisissant une disposition pour vos cartes sur les deux premiè!res lignes.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-4f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("Nous allons commencer par déplacer une première carte. Pour sélectionner une carte il suffit de cliquer dessus.");
				this.displayBackground(true);
			}
			
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 2:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setLeftArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("Lorsque vous survolez ou cliquez sur une carte, un panneau s'affiche avec des informations sur la carte.");
				this.displayBackground(true);
			}
			
			this.gameObjectPosition = GameView.instance.getMyHoveredRPCPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,11f,10f),0f,0f);
			this.drawLeftArrow();
			break;
		case 3:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("Maintenant nous allons cliquer sur la case sur laquelle nous souhaitons déplacer la carte. Avant le combat, il vous est possible de déplacer vos cartes sur les deux premières lignes.");
				this.displayBackground(true);
			}

			this.gameObjectPosition = GameView.instance.getTilesPosition(1,1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 4:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("Vous pouvez déplacer toutes vos troupes. Nous allons pour le moment déplacer vos deux cartes les plus offensives et les approcher du campement ennemi.");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 5:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("En rapprochant ainsi vos cartes, vous pourrez attaquer votre adversaire plus rapidement.");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 6:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("Vous pouvez déplacer toutes vos troupes. Nous allons pour le moment déplacer vos deux cartes les plus offensives et les approcher du campement ennemi.");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 7:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Préparons le combat");
				this.setPopUpDescription("En rapprochant ainsi vos cartes, vous pourrez attaquer votre adversaire plus rapidement.");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(5,1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 8:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(1,6,4));
				StartCoroutine(GameController.instance.moveCharacterRPC(2,6,5));
				StartCoroutine(GameController.instance.moveCharacterRPC(4,6,6));
				StartCoroutine(GameController.instance.moveCharacterRPC(5,6,7));
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("On commence !");
				this.setPopUpDescription("Une fois que vous avez déplacé vos cartes, cliquez sur 'Je suis prêt' pour démarrer le combat");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getStartButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,4f,1.5f),0.8f,0.6f);
			this.drawDownArrow();
			break;
		case 9:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Je découvre le jeu de l'adversaire");
				this.setPopUpDescription("Lorsque le combat démarre, vous dévoilez le jeu de l'adversaire ainsi que sa disposition. Cliquez sur l'une des cartes de votre adveraire pour continuer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,6);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,7f,2.5f),0.8f,0.8f);
			this.drawUpArrow();
			break;
		case 10:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setRightArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Je découvre le jeu de l'adversaire");
				this.setPopUpDescription("Le panneau latéral de droite vous permet de découvrir les caractéristiques des cartes de l'adversaire");
				this.displayBackground(true);
			}
			
			this.gameObjectPosition = GameView.instance.getHisHoveredRPCPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,11f,10f),0f,0f);
			this.drawRightArrow();
			break;
		case 11:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Timer");
				this.setPopUpDescription("Lorsque vous jouerez contre d'autres joueurs, vous disposerez d'un temps limité pour jouer. Un compteur est affiché en haut de l'écran");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTimerGoPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 12:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Principes de jeu");
				this.setPopUpDescription("La carte ayant la plus grande rapidité commence à jouer, ici il s'agit de votre carte. Celle-ci clignote, avant de commencer, détaillons les caractéristiques d'une carte");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 13:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Caractéristiques d'une carte");
				this.setPopUpDescription("Chaque carte joue dans un ordre précis et susceptible d'évoluer en cours de jeu. Sur chaque carte vous retrouverez le nombre de tour avant que cette dernière puisse jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsQuicknessZonePosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1f,0.75f),0f,0f);
			this.drawDownArrow();
			break;
		case 14:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Caractéristiques d'une carte");
				this.setPopUpDescription("Chaque carte possède une jauge de vie, lorsque celle-ci tombe à zéro, la carte quitte le terrain");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsLifeZonePosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.5f,0.75f),0f,0f);
			this.drawDownArrow();
			break;
		case 15:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Caractéristiques d'une carte");
				this.setPopUpDescription("Chaque carte possède une attaque rapprochée. Il s'agit de la valeur des dégâts infligés par la carte en corps à corps");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsAttackZonePosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1f,0.75f),0f,0f);
			this.drawDownArrow();
			break;
		case 16:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Principes de jeu");
				this.setPopUpDescription("Elle peut également lancer des actions : attaque au corps à corps, déclenchement d'une compétence ou passage d'un tour. Les actions sont disponibles dans la partie inférieure de l'écran.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,-4.5f,8f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 17:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayArrow(false);
				this.displayNextButton(true);
				this.setPopUpTitle("Principes de jeu");
				this.setPopUpDescription("Lorsqu'une carte joue, elle peut se déplacer, sa zone d'action correspond aux cases en bleue");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,7f,7f),0f,0f);
			this.resizePopUp(new Vector3(0,2f,-4f));
			break;
		case 18:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Premier déplacement de la carte 2");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 19:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Passer le tour");
				this.setPopUpDescription("On passe le tour de la carte 2");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPassButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 20:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 4 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 21:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(2,4,4));
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("La carte 4 s'est déplacée");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(2,4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 22:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(0).init(GameView.instance.getCard(4),GameView.instance.getCard(4).GetAttackSkill());
				GameSkills.instance.getSkill(0).applyOn(2);
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Attaque");
				this.setPopUpDescription("La carte 4 a attaqué votre carte 2");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 23:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 7 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(7);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 24:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(5,4,7));
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("La carte 7 s'est déplacée");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(5,4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 25:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(59).init(GameView.instance.getCard(7),GameView.instance.getCard(7).getSkills()[1]);
				GameSkills.instance.getSkill(59).applyOn(3);
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Attaque");
				this.setPopUpDescription("La carte 7 a lancé sénilité sur la carte 3");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 26:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 0 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 27:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Déplacement de la carte 0");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(1,3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 28:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Stéroïdes");
				this.setPopUpDescription("On lance stéroïdes");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getSkillButtonPosition(0); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 29:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Stéroïdes");
				this.setPopUpDescription("On cible le personnage allié");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 30:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Stéroïdes");
				this.setPopUpDescription("Explication des effets");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 31:
			if(!isResizing)
			{
				GameController.instance.resolvePass(); // A SUPPRIMER DES QUE LA COMPETENCE SERA LA !!!!!!!!!!!!!!!!
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 6 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(4,6);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 32:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(4,4,6));
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("La carte 6 s'est déplacée");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(4,4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 33:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(0).init(GameView.instance.getCard(6),GameView.instance.getCard(6).GetAttackSkill());
				GameSkills.instance.getSkill(0).applyOn(2);
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Attaque");
				this.setPopUpDescription("La carte 6 a attaqué votre carte 2");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,4f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 34:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 1 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(2,0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 35:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Déplacement de la carte 1");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(2,2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 36:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Renforcement");
				this.setPopUpDescription("On lance renforcement");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getSkillButtonPosition(0); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 37:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Renforcement");
				this.setPopUpDescription("On cible le personnage allié");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 38:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Lancer Renforcement");
				this.setPopUpDescription("Explication des effets");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 39:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 5 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 40:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(1,4,5));
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("La carte 5 s'est déplacée");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 41:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(65).init(GameView.instance.getCard(5),GameView.instance.getCard(5).getSkills()[1]);
				GameSkills.instance.getSkill(65).applyOn(0);
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Attaque");
				this.setPopUpDescription("La carte 5 a lancé une attaque massue sur votre carte 0");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 42:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 3 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 43:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Déplacement de la carte 3");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(5,3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 44:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Attaque bersek");
				this.setPopUpDescription("On lance attaque bersek");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getSkillButtonPosition(0); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 45:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer attaque bersek");
				this.setPopUpDescription("On cible le personnage ennemi");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(7);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 46:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Lancer attaque bersek");
				this.setPopUpDescription("Explication des effets");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(7);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 47:
			if(!isResizing)
			{
				//GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 2 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 48:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Attaque 360");
				this.setPopUpDescription("On lance attaque 360");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getSkillButtonPosition(0); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 49:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer attaque 360");
				this.setPopUpDescription("On cible le personnage ennemi");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 50:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Lancer attaque bersek");
				this.setPopUpDescription("Explication des effets");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,6f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 51:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Déplacement de la carte 2");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(2,4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 52:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Mouvement");
				this.setPopUpDescription("Au tour de la carte 0 à jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 53:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer Attaque simple");
				this.setPopUpDescription("On lance attaque");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getAttackButtonPosition(); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 54:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Lancer attaque simple");
				this.setPopUpDescription("On cible le personnage ennemi");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 55:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Lancer attaque simple");
				this.setPopUpDescription("Explication des effets");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 56:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Récompenses");
				this.setPopUpDescription("A chaque fin de match vous remportez diverses récompenses : des crédits pour acheter ou améliorer des cartes, ainsi que des points d'expérience.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,-3f,-4f));
			break;
		case 57:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Retour au lobby");
				this.setPopUpDescription("Vous pouvez cliquer sur Quitter et retourner au lobby");
				this.displayBackground(true);
			}
			this.gameObjectPosition = EndSceneController.instance.getQuitButtonPosition(); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,4f,2f),0.7f,0.7f);
			this.drawDownArrow();
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 0:
			playerCount++;
			if(playerCount==2)
			{
				this.displayNextButton(true);
			}
			break;
		case 1: case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 18: case 19: case 27: case 28: case 29: case 35: case 36: case 37: case 43: case 44: case 45: case 48: case 49: case 51: case 53: case 54: 
			this.launchSequence(this.sequenceID+1);
			break;
		case 56:
			this.displayNextButton(true);
			break;
		case 301:
			StartCoroutine(GameController.instance.endTutorial());
			break;
		}
	}
}

