using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewSkillsPopUpViewModel
{	
	
	public IList<string> skills;
	public string title;
	
	public NewSkillsPopUpViewModel()
	{
		this.skills = new List<string> ();
		this.title = "";
	}
}


