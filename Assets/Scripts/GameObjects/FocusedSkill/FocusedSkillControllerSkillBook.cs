using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class FocusedSkillControllerSkillBook : FocusedSkillController 
{
	public override void exit()
	{
		NewSkillBookController.instance.hideFocusedSkill ();
	}
	public override string getDescription(int idSkill, int level)
	{
		return WordingSkills.getDescription(idSkill,level);
	}
}