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
				this.displayPopUp(2);
				this.displayNextButton(false);
				this.setPopUpTitle("Début du combat");
				this.setPopUpDescription("Pret pour votre premier combat ? Le colonel Garruk, fraichement débarqué lui aussi sur Crystalia, terrorise les habitants avec ses berserks surentrainés. Anéantissez son armée pour ramener le calme dans la région !\n\nLe terrain de bataille est constitué de cases dont certaines ne peuvent etre franchies. Attention, certains personnages peuvent piéger les cases, rendant les déplacements périlleux ! Heureusement pour vous Garruk, peu adepte de la finesse, préfèrera sans doute foncer dans le tas !");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Sélectionner une unité");
				this.setPopUpDescription("Vos troupes sont pretes pour le combat. Chaque unité est affichée sous forme de carte avec un visuel différent pour chaque classe de personnage.\n\nA chaque début de combat, vous pourrez placer vos troupes sur les deux premières rangées de cases. Vos unités plus résistantes pourront ainsi protéger vos unités les plus faibles.\n\n<b>Cliquez sur ce roublard pour le déplacer.</b> ");
				this.displayBackground(true);
			}
			
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 2:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.setLeftArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Focus sur les unités");
				this.setPopUpDescription("Les compétences et caractéristiques de chaque unité peuvent etre consultées en survolant celle-ci sur le champ de bataille.\n\nKrudi, célèbre artiste crystalien, aime à raconter l'histoire suivante :\n<i>A la tete de mille hommes Grimorak s'élança,\nsans connaitre ses troupes, envoya au combat,\ncent nains unijambistes, et cinq cents cancrelats,\nqui rotirent sous le feu de quelques bazookas.");
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
				this.setPopUpTitle("Déplacer ses unités");
				this.setPopUpDescription("Déplaçons maintenant notre drogueur sur une case libre en cliquant sur celle-ci.");
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
				this.setPopUpTitle("Préparer le combat");
				this.setPopUpDescription("Pour ce combat nous allons avancer nos unités les plus offensives et laisser en retrait la plus faible. Garruk veut du sang, il va etre servi!");
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
				this.setPopUpTitle("Avancer le berserk");
				this.setPopUpDescription("<b>Avancez votre berserk d'une case</b>\nPlacé au centre de l'écran, le berserk pourra ainsi venir facilement en aide sur les cotés du terrain de bataille");
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
				this.setPopUpTitle("Avancez une troisème unité");
				this.setPopUpDescription("Vous avez compris le principe? Avancez maintenant la troisième unité!");
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
				this.setPopUpTitle("Pendant ce temps...");
				this.setPopUpDescription("... Garruk déplace aussi ses troupes. Ses déplacements ne seront visibles qu'au début du combat.");
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
				this.setPopUpTitle("Pret à jouer");
				this.setPopUpDescription("Une fois que vous etes satisfaits du positionnement de vos unités, <b>cliquez sur start pour démarrer le combat</b> et découvrir la stratégie de l'infame Garruk");
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
				this.setPopUpTitle("L'infame Garruk");
				this.setPopUpDescription("Sans surprise, Garruk a déployé ses berserks et les a avancé. Avant de se lancer dans l'action, prenons un petit moment pour regarder ses unités de plus près... Cliquez sur une unité de Garruk");
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
				this.setPopUpTitle("Focus sur les unités ennemies");
				this.setPopUpDescription("Comme pour vos propres unités, survoler une unité ennemie vous permettra de découvrir ses compétences. Utile pour cibler rapidement les unités les plus dangereuses!");
				this.displayBackground(true);
			}
			
			this.gameObjectPosition = GameView.instance.getHisHoveredRPCPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,11f,10f),0f,0f);
			this.drawRightArrow();
			break;
		case 11:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Le timer");
				this.setPopUpDescription("Vous disposerez de 20 secondes à chaque tour pour choisir vos actions. Un temps très court...\n\nPour ce premier duel néanmoins, aucune limite de temps ne vous est imposée");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTimerGoPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 12:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Ordre de jeu");
				this.setPopUpDescription("Les unités jouent les unes après les autres, l'ordre étant déterminé par la caractéristique de rapidité de la carte.\n\nUne caractéristique plus importante qu'il n'y parait puisqu'une carte très puissante mais lente pourra etre neutralisée avant meme d'avoir pu jouer!\n\nL'unité dont c'est le tour clignote, il s'agit de votre berserk!");
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
				this.setPopUpTitle("Comprendre l'ordre de jeu");
				this.setPopUpDescription("Une fois la partie lancée, sur chaque unité est affichée le nombre de tours à attendre avant de pouvoir jouer");
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
				this.setPopUpTitle("Les points de vie");
				this.setPopUpDescription("Chaque unité dispose d'un certain nombre de points de vie.\n\nUne unité est anéantie dès que son total de points de vie atteint 0");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsLifeZonePosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.5f,0.75f),0f,0f);
			this.drawDownArrow();
			break;
		case 15:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Les points d'attaque");
				this.setPopUpDescription("Chaque unité dispose de points d'attaque. Il servent à déterminer la puissance d'attaque de chaque personnage mais influent également sur de nombreuses compétences !\n\nLe combat se présente bien, les berserks de Garruk ont des niveaux d'attaque équivalents à ceux d'un caniche de Crystalia");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsAttackZonePosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1f,0.75f),0f,0f);
			this.drawDownArrow();
			break;
		case 16:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Compétences d'une unité");
				this.setPopUpDescription("Chaque niveau dispose de 2 à 4 compétences.\n\nLa compétence d'attaque (à gauche) permet d'attaquer une unité adjacente. Les compétences de classe (au centre) dépendent de l'unité, et enfin le bouton passer permet de passer son tour.\n\nLa couleur de la compétence indique sa disponibilité. Ici la compétence d'attaque est affichée en rouge car aucun ennemi n'est à proximité");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,-4.5f,8f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 17:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.displayArrow(false);
				this.displayNextButton(true);
				this.setPopUpTitle("Zone de déplacement");
				this.setPopUpDescription("Une unité peut à chaque tour utiliser une compétence et se déplacer. La zone bleue permet de visualiser la zone de déplacement autorisée.\n\nAttention, certaines classes d'unité peuvent se déplacer plus vite que d'autres!");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,7f,7f),0f,0f);
			this.resizePopUp(new Vector3(0,2f,-9.5f));
			break;
		case 18:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Tour du berserk");
				this.setPopUpDescription("Mauvaise nouvelle, notre berserk ne peut pas atteindre les rangs ennemis. Nous allons l'avancer pour qu'il puisse les atteindre au prochain tour");
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
				this.setPopUpTitle("Fin du tour");
				this.setPopUpDescription("Notre berserk est avancé et ne peut pas (encore) utiliser ses compétences, passons au tour suivant");
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
				this.setPopUpTitle("Au tour de Garruk");
				this.setPopUpDescription("C'est au tour du berserk de Garruk de jouer !");
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
				this.setPopUpTitle("Danger !");
				this.setPopUpDescription("Garruk a bien vu que notre berserk était à portée du sien, il se déplace logiquement pour nous attaquer");
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
				this.setPopUpTitle("Escarmouche");
				this.setPopUpDescription("Nous sommes attaqués par le berserk de Garruk !\n\nPas de panique, notre personnage est résistant. Et Garruk risque de regretter son attaque au prochain tour...");
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
				this.setPopUpTitle("Encore le tour de Garruk");
				this.setPopUpDescription("Un meme joueur peut jouer deux tours d'affilée selon l'odre des unités. D'ou l'importance de bien vérifier l'odre des tours en début de combat !");
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
				this.displayPopUp(1);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Déplacement");
				this.setPopUpDescription("le drogueur dispose d'un déplacement limité par rapport au berserk et ne peut donc pas venir nous attaquer.\n\n Les drogueurs disposent en revanche de plusieurs compétences permettant de gener nos unités...");
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
				this.setPopUpDescription("Le drogueur a utilisé sa compétence <i>Sénilité</i> pour affaiblir notre berserk en diminuant sa compétence d'attaque.\n\n Une icone sur la carte de l'unité touchée permet de voir l'effet de la compétence et sa durée");
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
				this.setPopUpTitle("A nous de jouer !");
				this.setPopUpDescription("C'est au tour de notre drogueur de jouer. Voyons s'il peut nous aider à contrecarrer les plans de l'ignoble Garruk");
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
				this.setPopUpTitle("Encercler le berserk ennemi");
				this.setPopUpDescription("Commençons par approcher notre drogueur du berserk ennemi!");
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
				//GameController.instance.resolvePass(); // A SUPPRIMER DES QUE LA COMPETENCE SERA LA !!!!!!!!!!!!!!!!
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
				StartCoroutine(GameController.instance.quitGame()); // A supprimer dès que ATTAQUE 360 fonctionne
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Récompenses");
				this.setPopUpDescription("A chaque fin de match vous remportez diverses récompenses : des crédits pour acheter ou améliorer des cartes, ainsi que des points d'expérience.");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,-3.5f,-9.5f));
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

