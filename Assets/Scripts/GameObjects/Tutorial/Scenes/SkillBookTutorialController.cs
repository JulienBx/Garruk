using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SkillBookTutorialController : TutorialObjectController 
{
	public static SkillBookTutorialController instance;
	
	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				SkillBookController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="Le livre des compétences";
				view.VM.description="Le livre des compétences est là pour vous donner une indication sur votre niveau de collection des compétences. Vous atteindrez le niveau ultime lorsque vous posséderez la totalité des compétences à leur niveau maximum, c'est à dire 100. Dans ce livre vous trouverez également pour chaque classe la totalité des compétences, y compris celle que vous ne possédez pas. Bonne lecture !";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1:
			StartCoroutine(SkillBookController.instance.endTutorial());
			break;
		}
	}
	public override void actionIsDone()
	{
	}
}

