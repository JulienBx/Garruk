using UnityEngine;
using TMPro;

public class NewSkillBookSkillTypeFilterController : SimpleButtonController
{	
	void OnMouseUp()
	{
		NewSkillBookController.instance.skillTypeFilterHandler (base.getId());	
	}
}

