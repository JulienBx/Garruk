using UnityEngine;
using TMPro;

public class newMyGameAvailabilityFilterController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.availabilityFilterHandler (base.getId());	
	}
}