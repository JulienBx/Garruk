using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFocusedCardNewSkillsPopUpViewModel
{	
	
	public IList<string> skills;
	public string title;
	
	public NewFocusedCardNewSkillsPopUpViewModel()
	{
		this.skills = new List<string> ();
		this.title = "";
	}
}


