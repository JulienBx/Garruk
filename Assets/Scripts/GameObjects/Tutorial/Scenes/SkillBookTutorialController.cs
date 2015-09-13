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
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("L'encyclopédie");
				this.setPopUpDescription("L'encyclopédie est là pour vous donner une indication sur votre niveau de collection des compétences. Vous atteindrez le niveau ultime lorsque vous posséderez la totalité des compétences à leur niveau maximum, c'est à dire 100. Dans ce livre vous trouverez également pour chaque classe la totalité des compétences, y compris celle que vous ne possédez pas. Bonne lecture !");
				this.displayBackground(true);
			}
			this.resizeBackground(new Rect(0,0,0,0),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			StartCoroutine(NewSkillBookController.instance.endTutorial(true));
			break;
		}
	}
	public override void actionIsDone()
	{
	}
}

