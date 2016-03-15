using UnityEngine;
using TMPro;

public class NewSkillBookSkillTypeFilterController : SimpleButtonController
{	
	new void OnMouseUp()
	{
		NewSkillBookController.instance.skillTypeFilterHandler (base.getId());	
	}
}

