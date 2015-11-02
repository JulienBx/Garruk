using UnityEngine;
using TMPro;

public class newMyGameAvailabilityFilterController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.availabilityFilterHandler (base.getId());	
	}
}