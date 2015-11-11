//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class SkillBookTutorialController : TutorialObjectController 
//{
//	public static SkillBookTutorialController instance;
//	
//	public override IEnumerator launchSequence(int sequenceID)
//	{
//		this.sequenceID = sequenceID;
//		switch(this.sequenceID)
//		{
//		case 0:
//			if(!isResizing)
//			{
//				this.displayArrow(false);
//				this.displayPopUp(2);
//				this.displayNextButton(true);
//				this.setPopUpTitle("Cristalopedia");
//				this.setPopUpDescription("La Cristalopedia rassemble l'ensemble de vos connaissances sur les factions et compétences de Cristalia. Découvrir toutes les compétences et recruter des Cristaliens les possédant vous permettra d'enrichir votre collection.\n\nPlus la compétence que vous possédez est de niveau élevée, plus vous gagnez de points de collection.\n\nIl est dit que le premier colon à acquérir toutes les compétences au niveau 100 sera sacré empereur de Cristalia!");
//				this.displayBackground(true);
//			}
//			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
//			this.resizePopUp(new Vector3(0,0,-9.5f));
//			break;
//		case 1:
//			StartCoroutine(NewSkillBookController.instance.endTutorial(true));
//			break;
//		}
//		yield break;
//	}
//	public override void actionIsDone()
//	{
//	}
//}
//
