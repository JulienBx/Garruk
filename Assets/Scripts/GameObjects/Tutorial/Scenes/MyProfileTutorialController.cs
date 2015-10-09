//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class MyProfileTutorialController : TutorialObjectController 
//{
//	public static MyProfileTutorialController instance;
//	
//	public override IEnumerator launchSequence(int sequenceID)
//	{
//		this.sequenceID = sequenceID;
//		switch(this.sequenceID)
//		{
//		case 0:
//			if(!isResizing)
//			{
//				MenuController.instance.setButtonsGui(false);
//				ProfileController.instance.setButtonsGui(false);
//				view.VM.displayArrow=false;
//				view.VM.displayNextButton=true;
//				view.VM.title="Votre profil de joueur";
//				view.VM.description="Bienvenue sur votre profil. Retrouvez ici tous vos amis, vos statistiques ainsi que les trophées remportées";
//			}
//			popUpWidth=0.3f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=0.35f*Screen.width;
//			popUpY=0.35f*Screen.height;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 1 :
//			if(!isResizing)
//			{
//				view.VM.displayArrow=true;
//				view.VM.displayNextButton=true;
//				view.VM.title="Vos informations personnelles";
//				view.VM.description="Sur l'encart de gauche vous retrouvez vos informations et votre photo, à tout moment vous pouvez les modifier.";
//				this.setLeftArrow();
//			}
//			arrowHeight=(2f/3f)*0.1f*Screen.height;
//			arrowWidth=(3f/2f)*arrowHeight;
//			if(Screen.height/3>180)
//			{
//				arrowX=185;
//			}
//			else
//			{
//				arrowX = Screen.height/3+5;
//			}
//			arrowY=(0.40f*Screen.height);
//			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
//			this.drawLeftArrow();
//			popUpWidth=0.35f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=arrowX+arrowWidth+0.01f*Screen.width;
//			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 2:
//			if(!isResizing)
//			{
//				view.VM.displayArrow=true;
//				view.VM.displayNextButton=true;
//				view.VM.title="Vos amis";
//				view.VM.description="La partie centrale est dédiée à vos amis. En ajoutant un ami, vous retrouverez sur votre page d'accueil des informations sur son activité dans le jeu (compétitions, amis)";
//				this.setUpArrow();
//			}
//			arrowHeight=0.1f*Screen.height;
//			arrowWidth=(2f/3f)*arrowHeight;
//			arrowX=0.5f*Screen.width-(arrowWidth/2f);
//			arrowY=0.35f*Screen.height;
//			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
//			this.drawUpArrow();
//			popUpWidth=0.3f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
//			popUpY=arrowY+arrowHeight+0.02f*Screen.height;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 3:
//			if(!isResizing)
//			{
//				view.VM.displayArrow=true;
//				view.VM.displayNextButton=true;
//				view.VM.title="Gérer vos amis";
//				view.VM.description="Vous retrouvez les invitations envoyés";
//				this.setDownArrow();
//			}
//			arrowHeight=0.1f*Screen.height;
//			arrowWidth=(2f/3f)*arrowHeight;
//			if(Screen.height/3>180)
//			{
//				arrowX=(Screen.width/2f-5f-190f)/2f + 190f-arrowWidth/2f;
//			}
//			else
//			{
//				arrowX = (Screen.width/2f-5f-(Screen.height/3f+10f))/2f + (Screen.height/3f+10f)-arrowWidth/2f;
//			}
//			arrowY=0.45f*Screen.height;
//			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
//			this.drawDownArrow();
//			popUpWidth=0.35f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
//			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 4:
//			if(!isResizing)
//			{
//				view.VM.displayArrow=true;
//				view.VM.displayNextButton=true;
//				view.VM.title="Gérer vos amis";
//				view.VM.description="ainsi que les demandes des autres joueurs";
//				this.setDownArrow();
//			}
//			arrowHeight=0.1f*Screen.height;
//			arrowWidth=(2f/3f)*arrowHeight;
//			if(Screen.height/3>180)
//			{
//				arrowX=(Screen.width/2f-5f-190f)/2f + Screen.width/2f+5f-arrowWidth/2f;
//			}
//			else
//			{
//				arrowX =(Screen.width/2f-5f-(Screen.height/3f+10f))/2f + Screen.width/2f+5f-arrowWidth/2f;
//			}
//			arrowY=0.45f*Screen.height;
//			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
//			this.drawDownArrow();
//			popUpWidth=0.35f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
//			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 5:
//			if(!isResizing)
//			{
//				view.VM.displayArrow=true;
//				view.VM.displayNextButton=true;
//				view.VM.title="Vos statistiques";
//				view.VM.description="Vous retrouvez vos statistiques en compétition ainsi que l'état d'avancement de votre collection. En cliquant sur 'Ma collection' vous retrouverez votre niveau d'acquisition des compétences";
//				this.setRightArrow();
//			}
//			arrowHeight=(2f/3f)*0.1f*Screen.height;
//			arrowWidth=(3f/2f)*arrowHeight;
//			if(Screen.height/3>180)
//			{
//				arrowX=Screen.width-185f-arrowWidth;
//			}
//			else
//			{
//				arrowX = Screen.width-(Screen.height/3f+5f)-arrowWidth;
//			}
//			arrowY=0.3f*Screen.height-(arrowHeight/2f);
//			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
//			this.drawRightArrow();
//			popUpWidth=0.35f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
//			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 6:
//			if(!isResizing)
//			{
//				view.VM.displayArrow=true;
//				view.VM.displayNextButton=true;
//				view.VM.title="Vos trophées";
//				view.VM.description="C'est dans cette vitrine que vous retrouverez les trophées au cours du jeu";
//				this.setRightArrow();
//			}
//			arrowHeight=(2f/3f)*0.1f*Screen.height;
//			arrowWidth=(3f/2f)*arrowHeight;
//			if(Screen.height/3>180)
//			{
//				arrowX=Screen.width-185f-arrowWidth;
//			}
//			else
//			{
//				arrowX = Screen.width-(Screen.height/3f+5f)-arrowWidth;
//			}
//			arrowY=0.7f*Screen.height-(arrowHeight/2f);
//			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
//			this.drawRightArrow();
//			popUpWidth=0.35f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
//			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 7:
//			if(!isResizing)
//			{
//				view.VM.displayArrow=false;
//				view.VM.displayNextButton=true;
//				view.VM.title="A vous de jouer";
//				view.VM.description="Commencez par mettre à jour vos informations ou bien essayez d'ajouter de nouveaux amis";
//			}
//			popUpWidth=0.3f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=0.35f*Screen.width;
//			popUpY=0.35f*Screen.height;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 8:
//			StartCoroutine(ProfileController.instance.endTutorial());
//			break;
//		}
//		yield break;
//	}
//	public override void actionIsDone()
//	{
//	}
//}
//
