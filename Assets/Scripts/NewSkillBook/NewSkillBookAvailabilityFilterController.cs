using UnityEngine;
using TMPro;

public class NewSkillBookAvailabilityFilterController : SimpleButtonController
{	
	void OnMouseUp()
	{
		NewSkillBookController.instance.availabilityFilterHandler (base.getId());	
	}
}