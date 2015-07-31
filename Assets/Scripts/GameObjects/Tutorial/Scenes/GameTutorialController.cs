using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GameTutorialController : TutorialObjectController 
{
	public static GameTutorialController instance;

	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				GameController.instance.setButtonsGUI(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=false;
				view.VM.title="Le mode match";
				view.VM.description="Pret pour votre premier duel ! Le match se joue sur un damier sur lequel vous pouvez déplacer vos cartes. Vous démarrez la partie en choisissant une disposition pour vos cartes sur les deux premiè!res lignes.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1:
			if(!isResizing)
			{
				GameController.instance.activeSingleCharacter(1);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Préparons le combat";
				view.VM.description="Nous allons commencer par déplacer une première carte. Pour sélectionner une carte il suffit de cliquer dessus.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getPlayingCardsPosition(1);
			GOSize =GameController.instance.getPlayingCardsSize(1);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 2:
			if(!isResizing)
			{
				//GameController.instance.setDestination(2,1,true);
				GameController.instance.addTileHalo(2,1,5, true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Préparons le combat";
				view.VM.description="Maintenant nous allons cliquer sur la case sur laquelle nous souhaitons déplacer la carte. Avant le combat, il vous est possible de déplacer vos cartes sur les deux premières lignes.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(2,1);
			GOSize =GameController.instance.getTilesSize(2,1);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 3:
			if(!isResizing)
			{
				GameController.instance.hideTileHalo(2,1);
				GameController.instance.activeSingleCharacter(2);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Préparons le combat";
				view.VM.description="Vous pouvez déplacer toutes vos troupes. Nous allons pour le moment déplacer vos deux cartes les plus offensives et les approcher du campement ennemi.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getPlayingCardsPosition(2);
			GOSize =GameController.instance.getPlayingCardsSize(2);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 4:
			if(!isResizing)
			{
				//GameController.instance.setDestination(3,1,true);
				GameController.instance.addTileHalo(3,1,5, true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Préparons le combat";
				view.VM.description="En rapprochant ainsi vos cartes, vous pourrez attaquer votre adversaire plus rapidement.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(3,1);
			GOSize =GameController.instance.getTilesSize(3,1);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 5:
			if(!isResizing)
			{
				GameController.instance.hideTileHalo(3,1);
				GameController.instance.setButtonGUI(1,true);
				StartCoroutine(GameController.instance.moveCharacterRPC(1,6,4));
				StartCoroutine(GameController.instance.moveCharacterRPC(2,6,5));
				StartCoroutine(GameController.instance.moveCharacterRPC(4,6,6));
				StartCoroutine(GameController.instance.moveCharacterRPC(5,6,7));
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="On commence !";
				view.VM.description="Une fois que vous avez déplacé vos cartes, cliquez sur 'Je suis prêt' pour démarrer le combat";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=Screen.width/2f-(arrowWidth/2f);
			arrowY=Screen.height/(1.75f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 6:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Je découvre le jeu de l'adversaire";
				view.VM.description="Lorsque le combat démarre, vous dévoilez le jeu de l'adversaire ainsi que sa disposition.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 7:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Timeline";
				view.VM.description="Une timeline apparaît sur la gauche de l'écran, elle vous permettra de voir quels seront les prochaines cartes à jouer. Vous retrouverez également l'historique des coups joués précédemment";
				this.setLeftArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			arrowX=0.33f*Screen.width;
			arrowY=0.25f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawLeftArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth+0.01f*Screen.width;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 8:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Le compte à rebours";
				view.VM.description="Un compte à rebours est affiché sur la gauche. Celui-ci sera actif lorsque vous commencerez des combats avec d'autres adversaires. Vous n'aurez que 30 secondes pour jouer !";
				this.setRightArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			arrowX=0.9f*Screen.width-arrowWidth;;
			arrowY=0.51f*Screen.height-(arrowHeight/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawRightArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 9:
			if(!isResizing)
			{
				//GameController.instance.setDestination(3,4,true);
				GameController.instance.addTileHalo(3,4,5, true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Mon premier tour";
				view.VM.description="C'est l'une de vos cartes qui est la plus rapide. Vous commencez donc à jouer. Nous allons déplacer votre carte pour la rapprocher de l'ennemi.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(3,4);
			GOSize =GameController.instance.getTilesSize(3,4);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 10:
			if(!isResizing)
			{
				GameController.instance.hideTileHalo(3,4);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Mon premier tour";
				view.VM.description="Votre carte n'étant pas à portée immédiate d'un adversaire, vous ne pourrez pas attaquer à ce tour. Vous pouvez donc passer.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(5);
			GOSize =GameController.instance.getSkillObjectsSize(5);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 11:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(2,4,4));
				GameController.instance.setAllSkillObjects(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="L'adversaire joue à son tour et vient de déplacer une carte à proximité de votre carte";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 12:
			if(!isResizing)
			{
				//GameController.instance.launchSkill(4,new List<int> {2});
				GameController.instance.setAllSkillObjects(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="Il déclenche une attaque simple et blesse votre personnage. ";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 13:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(5,4,7));
				GameController.instance.setAllSkillObjects(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="Comme l'adversaire possède une autre carte plus rapide que les votres, celui-ci peut jouer une deuxième fois et en profite pour déplacer une de ses cartes.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 14:
			if(!isResizing)
			{
				GameController.instance.resolvePass();
				GameController.instance.disableAllCharacters();
				//GameController.instance.setDestination(1,3,true);
				GameController.instance.addTileHalo(1,3,5, true);
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Mon Deuxième tour";
				view.VM.description="C'est votre tour, nous allons maintenant déplacer une deuxième carte à proximité du terrain adverse.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(1,3);
			GOSize =GameController.instance.getTilesSize(1,3);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 15:
			if(!isResizing)
			{
				GameController.instance.hideTileHalo(1,3);
				GameController.instance.disableAllCharacters();
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeSingleSkillObjects(0);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Lancement de compétence";
				view.VM.description="Votre carte n'est pas à proximité d'un adversaire, mais vous pouvez tout de même lancer la compétence rugissement, cette dernière va permettre de donner des points supplémentaire d'attaque à vos autres cartes. Pour déclencher une compétence il suffit de cliquer sur le bouton correspondant, affiché en bas de l'écran.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(0);
			GOSize =GameController.instance.getSkillObjectsSize(0);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 16:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le rugissement";
				view.VM.description="L'effet est immediat, chacune de vos cartes gagne des points d'attaque supplémentaires";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 17:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(4,4,6));
				GameController.instance.setAllSkillObjects(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="C'est encore au tour de l'adversaire qui déplace de nouveau un de ses personnage prêt de votre berseker";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 18:
			if(!isResizing)
			{
				//GameController.instance.launchSkill(4,new List<int> {2});
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="... et lui inflige des points de dégâts";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 19:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				//GameController.instance.setDestination(2,2,true);
				GameController.instance.addTileHalo(2,2,5, true);
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Mon troisième tour";
				view.VM.description="Déplaçons une autre de vos cartes et rapprochons là de vos adversaires.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(2,2);
			GOSize =GameController.instance.getTilesSize(2,2);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 20:
			if(!isResizing)
			{
				GameController.instance.hideTileHalo(2,2);
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeSingleSkillObjects(5);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Mon troisième tour";
				view.VM.description="Vous ne pouvez malheureusement pas attaquer ni déclencher d'autres compétences. On va donc passer ce tour.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(5);
			GOSize =GameController.instance.getSkillObjectsSize(5);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 21:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.moveCharacterRPC(1,4,5));
				GameController.instance.setAllSkillObjects(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="L'adversaire déplace une de ses cartes à proximité d'un de vos bersek.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 22:
			if(!isResizing)
			{
				//GameController.instance.launchSkill(4,new List<int> {1});
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'adversaire joue";
				view.VM.description="... et lui inflige des dégâts.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 23:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				//GameController.instance.setDestination(5,3,true);
				GameController.instance.addTileHalo(5,3,5, true);
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="A l'attaque";
				view.VM.description="Votre adversaire est suffisament prêt pour que vous puissiez l'attaquer. Déplacez votre bersek à proximité.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(5,3);
			GOSize =GameController.instance.getTilesSize(5,3);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 24:
			if(!isResizing)
			{
				GameController.instance.hideTileHalo(5,3);
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeSingleSkillObjects(4);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="A l'attaque";
				view.VM.description="Il est temps de lancer une attaque simple. La carte étant adjacente à la vôtre, il est désormais possible d'attaquer.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(4);
			GOSize =GameController.instance.getSkillObjectsSize(4);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 25:
			if(!isResizing)
			{
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeTargetingOnCharacter(7);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="A l'attaque";
				view.VM.description="Sélectionnez bien la carte ennemi, vous voyez qu'apparaît déjà les futurs effets de l'attaque. Ici le nombre de points de vie va chuter à 0";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(5,4);
			GOSize =GameController.instance.getTilesSize(5,4);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 26:
			if(!isResizing)
			{
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="La puissance des compétences";
				view.VM.description="Votre personnage se trouve coincé entre deux ennemis. On pourait utiliser une attaque simple, mais celle-ci ne permettrait d'atteindre qu'un seul des deux ennemis.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 27:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeSingleSkillObjects(0);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="La puissance des compétences";
				view.VM.description="Heureusement votre carte possède l'attaque circulaire est en mesure d'infliger des dégâts à toutes les unités adjacentes, cliquez dessus pour l'activer.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(0);
			GOSize =GameController.instance.getSkillObjectsSize(0);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 28:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeSingleSkillObjects(5);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="La puissance des compétences";
				view.VM.description="Bravo ! Vous venez d'éléminer deux adversaires d'un coup. Vous pouvez passer au tour suivant.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(5);
			GOSize =GameController.instance.getSkillObjectsSize(5);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 29:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeSingleSkillObjects(4);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Le coup de grâce";
				view.VM.description="Il ne reste plus qu'un adversaire. Essayons de l'éliminer maintenant. C'est le tour de votre carte située à proximité immédaite. Déclanchons une attaque.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getSkillObjectsPosition(4);
			GOSize =GameController.instance.getSkillObjectsSize(4);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 30:
			if(!isResizing)
			{
				GameController.instance.disableAllSkillObjects();
				GameController.instance.activeTargetingOnCharacter(5);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Le coup de grâce";
				view.VM.description="Comme tout à l'heure, on sélectionne la cible.";
				this.setDownArrow();
			}
			GOPosition = GameController.instance.getTilesPosition(1,4);
			GOSize =GameController.instance.getTilesSize(1,4);
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=GOPosition.x-(arrowWidth/2f);
			arrowY=Screen.height-GOPosition.y-GOSize.y/2f-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 31:
			if(!isResizing)
			{
				GameController.instance.disableAllCharacters();
				GameController.instance.disableAllSkillObjects();
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Fin du combat";
				view.VM.description="Vous avez éliminé tous vos ennemis, le combat s'arrête et vous remportez une précieuse victoire ! Félicitations";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 32:
			if(!isResizing)
			{
				StartCoroutine(GameController.instance.quitGame());
				view.VM.displayArrow=false;
				view.VM.displayNextButton=false;
				view.VM.title="Récompenses";
				view.VM.description="A chaque fin de match vous remportez diverses récompenses : des crédits pour acheter ou améliorer des cartes, ainsi que des points d'expérience.";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.6f*Screen.width;
			popUpY=0.1f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 33:
			if(!isResizing)
			{
				GameController.instance.setEndSceneControllerGUI(true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Quittons le combat";
				view.VM.description="Vous pouvez désormais quitter le mode match.";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=Screen.width/2f-(arrowWidth/2f);
			arrowY=Screen.height/(1.2f)-arrowHeight;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 1: case 2: case 3: case 4: case 5: case 9: case 10: case 14: case 15: case 19: case 20: case 21: case 23: case 24: case 25: case 27: case 28: case 29: case 30:
			this.launchSequence(this.sequenceID+1);
			break;
		case 32:
			this.setNextButtonDisplaying(true);
			break;
		case 33:
			StartCoroutine(GameController.instance.endTutorial());
			break;
		}
	}
}

