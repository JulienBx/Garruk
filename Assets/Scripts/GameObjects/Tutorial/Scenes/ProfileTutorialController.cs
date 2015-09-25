using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileTutorialController : TutorialObjectController 
{
	public static ProfileTutorialController instance;
	
	public override IEnumerator launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				ProfileController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le profil d'un joueur";
				view.VM.description="En consultant le profil d'un autre joueur vous accédez à ces statistiques ainsi qu'à la liste de ses amis";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1:
			if(!isResizing)
			{
				view.VM.displayArrow=true;
				view.VM.displayNextButton=true;
				view.VM.title="Ajouter un ami";
				view.VM.description="Pour ajouter ou retirer le joueur de vos amis, des boutons sont disponibles en haut à droite du profil";
				this.setRightArrow();
			}
			arrowHeight=(2f/3f)*0.1f*Screen.height;
			arrowWidth=(3f/2f)*arrowHeight;
			if(Screen.height/3>180)
			{
				arrowX=Screen.width-185f-arrowWidth;
			}
			else
			{
				arrowX = Screen.width-(Screen.height/3f+5f)-arrowWidth;
			}
			arrowY=0.15f*Screen.height-(arrowHeight/2f);
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawRightArrow();
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX-0.01f*Screen.width-popUpWidth;
			popUpY=arrowY+arrowHeight/2f-popUpHeight/2f;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 2:
			if(!isResizing)
			{
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="A vous de jouer";
				view.VM.description="Consulter les profils de vos adversaires et tenez vous informé de leur progression";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 3:
			StartCoroutine(ProfileController.instance.endTutorial());
			break;
		}
		yield break;
	}
	public override void actionIsDone()
	{
	}
}

