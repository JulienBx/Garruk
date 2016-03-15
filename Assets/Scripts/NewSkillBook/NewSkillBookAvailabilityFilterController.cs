using UnityEngine;
using TMPro;

public class NewSkillBookAvailabilityFilterController : SimpleButtonController
{	
	new void OnMouseUp()
	{
		NewSkillBookController.instance.availabilityFilterHandler (base.getId());	
	}
}