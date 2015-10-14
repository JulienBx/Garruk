//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class DivisionLobbyTutorialController : TutorialObjectController 
//{
//	public static DivisionLobbyTutorialController instance;
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
//				DivisionLobbyController.instance.setButtonsGui(false);
//				view.VM.displayArrow=false;
//				view.VM.displayNextButton=true;
//				view.VM.title="L'écran de division";
//				view.VM.description="Cet écran montre votre évolution au sein de la division. Vous y verrez votre progression pour atteindre les divisions supérieures ainsi que vos derniers résultats";
//			}
//			popUpWidth=0.3f*Screen.width;
//			popUpHeight=this.computePopUpHeight();
//			popUpX=0.35f*Screen.width;
//			popUpY=0.35f*Screen.height;
//			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
//			break;
//		case 1:
//			StartCoroutine(DivisionLobbyController.instance.endTutorial());
//			break;
//		}
//		yield break;
//	}
//	public override void actionIsDone()
//	{
//	}
//}
//
