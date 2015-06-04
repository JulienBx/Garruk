using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class StoreNewSkillsPopUpViewModel
{	
	
	public IList<string> skills;
	public string title;
	
	public StoreNewSkillsPopUpViewModel ()
	{
		this.skills = new List<string> ();
		this.title = "";
	}
}


