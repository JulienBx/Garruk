using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class FocusedSkillControllerFocusedCard : FocusedSkillController 
{
	public override void exit()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController>().hideSkillFocused();
	}
	public override string getDescription(int idSkill, int level)
	{
		return gameObject.transform.parent.GetComponent<NewFocusedCardController>().getSkillFocusedDescription(idSkill,level);
	}
}