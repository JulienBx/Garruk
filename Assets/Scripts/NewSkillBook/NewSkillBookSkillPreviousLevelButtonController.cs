using UnityEngine;
using TMPro;

public class NewSkillBookSkillPreviousLevelButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		gameObject.transform.parent.GetComponent<NewSkillBookSkillController> ().previousLevelHandler ();
	}
}

