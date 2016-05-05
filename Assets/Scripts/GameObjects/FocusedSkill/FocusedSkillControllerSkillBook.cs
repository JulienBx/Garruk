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
        string description = WordingSkills.getDescription(idSkill,level);
        if(WordingSkills.getProba(idSkill,level)<100)
        {
            description +=WordingCard.getReference(0)+WordingSkills.getProba(idSkill,level)+WordingCard.getReference(1);
        }
        return description;
	}
}