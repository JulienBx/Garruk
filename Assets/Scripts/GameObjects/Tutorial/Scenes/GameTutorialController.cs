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
	private Vector3 secondGameObjectPosition;

	public override IEnumerator launchSequence(int sequenceID)
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
				this.setPopUpDescription("Pret pour votre premier combat ? Le colonel Garruk, fraichement débarqué lui aussi sur Cristalia, terrorise les habitants avec ses prédateurs surentrainés. Anéantissez son armée pour ramener le calme dans la région !\n\nLe terrain de bataille est constitué de cases dont certaines ne peuvent etre franchies. Attention, certains personnages peuvent piéger les cases, rendant les déplacements périlleux ! Heureusement pour vous Garruk, peu adepte de la finesse, préfèrera sans doute foncer dans le tas !");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(2);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Survol d'une unité");
				this.setPopUpDescription("Vos troupes sont pretes pour le combat. Chaque unité est affichée sous forme de carte avec un visuel différent pour chaque classe de personnage.\n\nA chaque début de combat, vous pourrez placer vos troupes sur les deux premières rangées de cases. Vos unités plus résistantes pourront ainsi protéger vos unités les plus faibles.\n\n<b>Survolez votre unité pour accéder au détail de ses compétences</b> ");
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
				this.setPopUpDescription("Les compétences et caractéristiques de chaque unité peuvent etre consultées en survolant celle-ci sur le champ de bataille.\n\nKrudi, célèbre artiste Cristalien, aime à raconter l'histoire suivante :\n<i>A la tete de mille hommes Grimorak s'élança,\nsans connaitre ses troupes, envoya au combat,\ncent nains unijambistes, et cinq cents cancrelats,\nqui rotirent sous le feu de quelques bazookas.");
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
				this.setPopUpDescription("Sélectionnons maintenant notre drogueur pour le déplacer sur une case.");
				this.displayBackground(true);
			}
			
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 4:
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
		case 5:
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
		case 6:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Avancer le prédateur");
				this.setPopUpDescription("<b>Avancez votre prédateur d'une case</b>\nPlacé au centre de l'écran, le prédateur pourra ainsi venir facilement en aide sur les cotés du terrain de bataille");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 7:
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
		case 8:
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
		case 9:
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
		case 10:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("L'infame Garruk");
				this.setPopUpDescription("Sans surprise, Garruk a déployé ses prédateurs et les a avancé. Avant de se lancer dans l'action, prenons un petit moment pour regarder ses unités de plus près... Cliquez sur une unité de Garruk");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(3,6);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,7f,2.5f),0.8f,0.8f);
			this.drawUpArrow();
			break;
		case 11:
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
		case 12:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Le timer");
				this.setPopUpDescription("Vous disposerez de 30 secondes à chaque tour pour choisir vos actions. Un temps très court...\n\nPour ce premier duel néanmoins, aucune limite de temps ne vous est imposée");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTimerGoPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 13:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Ordre de jeu");
				this.setPopUpDescription("Les unités jouent les unes après les autres, l'ordre étant déterminé par la caractéristique de rapidité de la carte.\n\nUne carte très puissante mais lente pourra etre neutralisée avant meme d'avoir pu jouer!\n\nL'unité dont c'est le tour clignote, il s'agit de votre prédateur!");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 14:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Comprendre l'ordre de jeu");
				this.setPopUpDescription("Une fois la partie lancée, sur chaque unité est affichée le nombre de tours à attendre avant de pouvoir jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawDownArrow();
			break;
		case 15:
			StartCoroutine(launchSequence(16));
			break;
//		case 14:
//			if(!isResizing)
//			{
//				this.displayPopUp(0);
//				this.setDownArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Les points de vie");
//				this.setPopUpDescription("Chaque unité dispose d'un certain nombre de points de vie.\n\nUne unité est anéantie dès que son total de points de vie atteint 0");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsLifeZonePosition(3);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.5f,0.75f),0f,0f);
//			this.drawDownArrow();
//			break;
//		case 15:
//			if(!isResizing)
//			{
//				this.displayPopUp(1);
//				this.setDownArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Les points d'attaque");
//				this.setPopUpDescription("Chaque unité dispose de points d'attaque. Il servent à déterminer la puissance d'attaque de chaque personnage mais influent également sur de nombreuses compétences !\n\nLe combat se présente bien, les prédateurs de Garruk ont des niveaux d'attaque équivalents à ceux d'un caniche de Cristalia");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsAttackZonePosition(3);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1f,0.75f),0f,0f);
//			this.drawDownArrow();
//			break;
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
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Tour du prédateur");
				this.setPopUpDescription("Mauvaise nouvelle, notre prédateur ne peut pas atteindre les rangs ennemis. Nous allons l'avancer pour qu'il puisse les atteindre au prochain tour");
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
				this.setPopUpDescription("Notre prédateur est avancé et ne peut pas (encore) utiliser ses compétences, passons au tour suivant");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPassButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 20:
			if(!isResizing)
			{
				//this.setUpArrow();
				this.displayPopUp(-1);
				this.displayArrow(false);
				this.setPopUpTitle("Danger !");
				this.setPopUpDescription("Garruk a bien vu que notre prédateur était à portée du sien, il se déplace logiquement pour nous attaquer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(4);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,6f,7f),0f,0f);
			this.secondGameObjectPosition = GameView.instance.getTilesPosition(2,4);
			if(!isResizing)
			{
				yield return new WaitForSeconds(3);
				StartCoroutine(GameController.instance.moveCharacterRPC(2,4,4));
				this.displayPopUp(0);
				this.displayNextButton(true);
			}
			this.resizePopUp(new Vector3(secondGameObjectPosition.x,secondGameObjectPosition.y-2f,-9.5f));
			//this.drawUpArrow();
			break;
		case 21:
			StartCoroutine(this.launchSequence(22));
			break;
//		case 21:
//			if(!isResizing)
//			{
//				StartCoroutine(GameController.instance.moveCharacterRPC(2,4,4));
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Danger !");
//				this.setPopUpDescription("Garruk a bien vu que notre prédateur était à portée du sien, il se déplace logiquement pour nous attaquer");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getTilesPosition(2,4);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 22:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(0).init(GameView.instance.getCard(4),GameView.instance.getCard(4).GetAttackSkill());
				GameSkills.instance.getSkill(0).addTarget(2,1);
				GameSkills.instance.getSkill(0).applyOn();
				this.displayPopUp(0);
				this.displayArrow(false);
				this.displayNextButton(true);
				this.setPopUpTitle("Escarmouche");
				this.setPopUpDescription("Nous sommes attaqués par le prédateur de Garruk !\n\nPas de panique... Garruk risque de regretter son attaque!");
				this.displayBackground(true);
			}
			//this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(0,0,8f,3.5f),0f,0f);
			this.resizePopUp(new Vector3(0,-3.25f,-9.5f));
			//this.drawUpArrow();
			break;
		case 23:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(-1);
				this.displayArrow(false);
				//this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Encore le tour de Garruk");
				this.setPopUpDescription("Un meme joueur peut jouer deux tours d'affilée selon l'ordre des unités. D'ou l'importance de bien vérifier l'ordre des tours en début de combat !");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(7);
			yield return new WaitForSeconds(1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,6f,7f),0f,0f);
			//this.drawUpArrow();
			this.secondGameObjectPosition = GameView.instance.getTilesPosition(5,4);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(GameController.instance.moveCharacterRPC(5,4,7));
				this.displayPopUp(0);
				this.displayNextButton(true);
			}
			this.resizePopUp(new Vector3(this.secondGameObjectPosition.x,this.secondGameObjectPosition.y-2f,-9.5f));
			//this.drawUpArrow();
			break;
		case 24:
			StartCoroutine(this.launchSequence(25));
			break;
//		case 24:
//			if(!isResizing)
//			{
//				StartCoroutine(GameController.instance.moveCharacterRPC(5,4,7));
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Déplacement");
//				this.setPopUpDescription("le drogueur dispose d'un déplacement limité par rapport au prédateur et ne peut donc pas venir nous attaquer.");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getTilesPosition(5,4);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 25:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(59).init(GameView.instance.getCard(7),GameView.instance.getCard(7).getSkills()[1]);
				GameSkills.instance.getSkill(59).addTarget(3,1,5);
				GameSkills.instance.getSkill(59).applyOn();
				this.displayPopUp(0);
				//this.setDownArrow();
				this.displayArrow(false);
				this.displayNextButton(true);
				this.setPopUpTitle("Sénilité");
				this.setPopUpDescription("Le drogueur a utilisé sa compétence <i>Sénilité</i> pour affaiblir notre prédateur en diminuant sa compétence d'attaque.\n\n Une icone sur la carte de l'unité touchée permet de voir l'effet de la compétence et sa durée");
				this.displayBackground(true);
			}
			//this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(0,0,8f,3.5f),0f,0f);
			this.resizePopUp(new Vector3(0,-3.25f,-9.5f));
			//this.drawDownArrow();
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
				this.setPopUpTitle("Encercler le prédateur ennemi");
				this.setPopUpDescription("Commençons par approcher notre drogueur du prédateur ennemi!");
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
				this.setPopUpTitle("Aider un allié");
				this.setPopUpDescription("Notre drogueur va maintenant pouvoir utiliser la compétence <i>stéroides</i> pour aider l'unité victime du sort de Garruk.");
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
				this.setPopUpTitle("Cibler le prédateur");
				this.setPopUpDescription("Ciblez le prédateur allié!");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 30:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(this.launchSequence(31));
			}
			break;
//			if(!isResizing)
//			{
//				this.displayPopUp(1);
//				this.setDownArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Stéroides");
//				this.setPopUpDescription("Cette compétence donnera un bonus à l'attaque de l'unité pour compenser le sort du drogueur adverse.\n\nL'effet de la compétence s'ajoute sur la carte et peut etre consulté à tout moment");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
//			this.drawDownArrow();
//			break;
		case 31:
			if(!isResizing)
			{
				//GameController.instance.resolvePass(); // A SUPPRIMER DES QUE LA COMPETENCE SERA LA !!!!!!!!!!!!!!!!
				this.displayPopUp(-1);
				this.displayArrow(false);
				//this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Au tour de Garruk");
				this.setPopUpDescription("C'est au prédateur adverse de jouer !");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getTilesPosition(4,6);
			//this.drawUpArrow();
			yield return new WaitForSeconds(1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,6f,7f),0f,0f);
			//this.drawUpArrow();
			this.secondGameObjectPosition = GameView.instance.getTilesPosition(4,4);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(GameController.instance.moveCharacterRPC(4,4,6));
				this.displayPopUp(0);
				this.displayNextButton(true);
			}
			this.resizePopUp(new Vector3(this.secondGameObjectPosition.x,this.secondGameObjectPosition.y-2f,-9.5f));
			//this.drawUpArrow();
			break;
		case 32:
			StartCoroutine(this.launchSequence(33));
			break;
//		case 32:
//			if(!isResizing)
//			{
//				StartCoroutine(GameController.instance.moveCharacterRPC(4,4,6));
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Un allié en danger");
//				this.setPopUpDescription("L'unité se déplace vers votre prédateur, Garruk choisit de concentrer ses attaques sur votre unité blessée.");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getTilesPosition(4,4);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 33:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(0).init(GameView.instance.getCard(6),GameView.instance.getCard(6).GetAttackSkill());
				GameSkills.instance.getSkill(0).addTarget(2,1);
				GameSkills.instance.getSkill(0).applyOn();
				this.displayPopUp(0);
				//this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Attaque !");
				this.setPopUpDescription("Votre prédateur est à nouveau attaqué!");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,8f,3.5f),0f,0f);
			this.resizePopUp(new Vector3(0,-3.25f,-9.5f));
			//this.drawUpArrow();
			break;
		case 34:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(1);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Nouveau tour");
				this.setPopUpDescription("Au tour de votre drogueur de jouer.\n\nPersonnage de soutien, le drogueur va pouvoir aider votre prédateur, entouré d'ennemis, pour lui permettre de renverser la situation");
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
				this.setPopUpTitle("Déplacement");
				this.setPopUpDescription("Commençons par avancer notre drogueur");
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
				this.setPopUpTitle("Renforcement");
				this.setPopUpDescription("Cette compétence va nous permettre de protéger notre prédateur en lui ajoutant un bouclier");
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
				this.setPopUpTitle("Cibler un allié");
				this.setPopUpDescription("Ciblez le prédateur pour augmenter sa défense!");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 38:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(this.launchSequence(39));
			}
			break;
		case 39:
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.displayArrow(false);
				//this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Au tour de Garruk");
				this.setPopUpDescription("Au tour d'un prédateur ennemi de jouer");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
			yield return new WaitForSeconds(1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,6f,7f),0f,0f);
			//this.drawUpArrow();
			this.secondGameObjectPosition = GameView.instance.getTilesPosition(1,4);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(GameController.instance.moveCharacterRPC(1,4,5));
				this.displayPopUp(0);
				this.displayNextButton(true);
			}
			this.resizePopUp(new Vector3(this.secondGameObjectPosition.x,this.secondGameObjectPosition.y-2f,-9.5f));
			//this.drawUpArrow();
			break;
		case 40:
			StartCoroutine(this.launchSequence(41));
			break;
//		case 40:
//			if(!isResizing)
//			{
//				StartCoroutine(GameController.instance.moveCharacterRPC(1,4,5));
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Déplacement");
//				this.setPopUpDescription("Le prédateur se déplace lui aussi vers notre unité!");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 41:
			if(!isResizing)
			{
				GameSkills.instance.getSkill(65).init(GameView.instance.getCard(5),GameView.instance.getCard(5).getSkills()[1]);
				GameSkills.instance.getSkill(65).addTarget(0,1,12);
				GameSkills.instance.getSkill(65).applyOn();
				this.displayPopUp(0);
				//this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Attaque");
				this.setPopUpDescription("Le prédateur utilise <i>Massue</i> sur notre unité.\nCette compétence dépend directement du niveau d'attaque de l'unité");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,8f,3.5f),0f,0f);
			this.resizePopUp(new Vector3(0,-3.25f,-9.5f));
			//this.drawDownArrow();
			break;
		case 42:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("A nous de jouer");
				this.setPopUpDescription("A notre second prédateur de rentrer en action");
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
				this.setPopUpTitle("Se déplacer");
				this.setPopUpDescription("Allons de ce pas attaquer les troupes de Garruk!");
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
				this.setPopUpTitle("Berserk");
				this.setPopUpDescription("La compétence Berserk va nous permettre de tuer d'un coup l'unité de Garruk!");
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
				this.setPopUpTitle("Attaquer");
				this.setPopUpDescription("Ciblez l'unité ennemie!");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(7);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 46:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(3);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y+1f,2f,4f),0f,0f);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(this.launchSequence(47));
			}
			break;
//			if(!isResizing)
//			{
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Résultat");
//				this.setPopUpDescription("L'unité ciblée est anéantie en un seul coup !\n\nLes prédateurs peuvent infliger d'énormes dégats bien qu'étant assez lents");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(7);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 47:
			if(!isResizing)
			{
				//GameController.instance.resolvePass();
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("Enfoncer le clou");
				this.setPopUpDescription("Au tour de notre prédateur de contre-attaquer.\n\nPourra-t-il faire la différence?");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 48:
			if(!isResizing)
			{
				this.displayPopUp(1);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Attaque 360");
				this.setPopUpDescription("Garruk a sans doute oublié de consulter vos unités au début du combat.\n\nEn effet votre prédateur possède la compétence <i>Attaque 360</i> qui va lui permettre d'infliger des dégats à tous les unités adjacentes d'un seul coup!");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getSkillButtonPosition(0); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawDownArrow();
			break;
		case 49:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,4f,2f),0f,0f);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(this.launchSequence(51));
			}
			break;
//		case 49:
			// a supprimer, pas de ciblage
//			if(!isResizing)
//			{
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(false);
//				this.setPopUpTitle("Passer son tour");
//				this.setPopUpDescription("Terminez votre tour sans vous déplacer. La fin du combat est proche!");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(4);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
//			this.drawUpArrow();
//			break;
//		case 50:
//			if(!isResizing)
//			{
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Lancer attaque bersek");
//				this.setPopUpDescription("Explication des effets");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(2);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,6f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 51:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Se déplacer");
				this.setPopUpDescription("Déplacez votre prédateur près du dernier ennemi restant");
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
				this.setPopUpTitle("A vous de jouer");
				this.setPopUpDescription("Votre drogueur va pouvoir achever l'ennemi");
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
				this.setPopUpTitle("Attaque");
				this.setPopUpDescription("Attaquez-le !");
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
				this.setPopUpTitle("Ciblez l'ennemi");
				this.setPopUpDescription("Ciblez le drogueur ennemi...");
				this.displayBackground(true);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0.4f,0.4f);
			this.drawUpArrow();
			break;
		case 55:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
			}
			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(0);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y+1f,2f,4f),0f,0f);
			if(!isResizing)
			{
				yield return new WaitForSeconds(2);
				StartCoroutine(this.launchSequence(56));
			}
			break;
//		case 55:
//			if(!isResizing)
//			{
//				this.displayPopUp(0);
//				this.setUpArrow();
//				this.displayNextButton(true);
//				this.setPopUpTitle("Dernière attaque");
//				this.setPopUpDescription("Et terrassez Garruk!");
//				this.displayBackground(true);
//			}
//			this.gameObjectPosition = GameView.instance.getPlayingCardsPosition(5);
//			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2f,2f),0f,0f);
//			this.drawUpArrow();
//			break;
		case 56:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.quitGame()); 
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(false);
				this.setPopUpTitle("Fin de combat");
				this.setPopUpDescription("chaque fin de combat, vos troupes reçoivent des bonus d'expérience et vous recevez du cristal.\n\nDisputer des combats officiels rapporte plus d'expérience et de cristal!");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0f,0f,40f,20f),0f,0f);
			this.resizePopUp(new Vector3(0,-3f,-9.5f));
			break;
		case 57:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setDownArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Quitter le combat");
				this.setPopUpDescription("Cliquez sur le bouton <i>quitter</i> quand vous avez terminé pour revenir au tableau de bord");
				this.displayBackground(true);
			}
			this.gameObjectPosition = EndSceneController.instance.getQuitButtonPosition(); 
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,4f,2f),0.7f,0.7f);
			this.drawDownArrow();
			break;
		}
		yield break;
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
		case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 18: case 19: case 27: case 28: case 29: case 35: case 36: case 37: case 43: case 44: case 45: case 48: case 51: case 53: case 54: 
			StartCoroutine(this.launchSequence(this.sequenceID+1));
			break;
		case 25: case 30 :
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

