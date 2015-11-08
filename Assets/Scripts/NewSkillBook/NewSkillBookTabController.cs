using UnityEngine;
using TMPro;

public class NewSkillBookTabController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewSkillBookController.instance.selectATabHandler(base.getId());	
	}
}

