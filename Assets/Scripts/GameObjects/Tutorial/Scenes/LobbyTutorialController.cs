using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LobbyTutorialController : TutorialObjectController 
{
	public static LobbyTutorialController instance;
	
	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				LobbyController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le lobby";
				view.VM.description="Bienvenue dans le lobby. C'est un écran d'avant match qui vous permet de choisir le deck avec lequel vous souhaitez commencer le combat. Vous choisirez également le type de rencontre que vous souhaitez débuter. Les matchs de coupe et de division sont classés, ils vous feront gagner davantage de crédit et auront une influence sur votre classement général";
			}
			popUpWidth=0.35f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.325f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1:
			if(!isResizing)
			{
				LobbyController.instance.setButtonGui(1,true);
				view.VM.displayArrow=true;
				view.VM.displayNextButton=false;
				view.VM.title="Prêt pour le match";
				view.VM.description="Nous alllons commencer par un petit match amical ! Go !";
				this.setDownArrow();
			}
			arrowHeight=0.1f*Screen.height;
			arrowWidth=(2f/3f)*arrowHeight;
			arrowX=1f/6f *(Screen.width - 20f)+5-arrowWidth/2f;
			arrowY=0.45f*Screen.height;
			view.VM.arrowRect= new Rect (arrowX,arrowY,arrowWidth,arrowHeight);
			this.drawDownArrow();
			popUpWidth=0.25f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=arrowX+arrowWidth/2f-popUpWidth/2f;
			popUpY=arrowY-popUpHeight-0.02f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		}
	}
	public override void actionIsDone()
	{
	}
}

