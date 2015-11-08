using UnityEngine;
using TMPro;

public class NewSkillBookAvailabilityFilterController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewSkillBookController.instance.availabilityFilterHandler (base.getId());	
	}
}