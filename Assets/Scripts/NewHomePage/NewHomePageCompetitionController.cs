using UnityEngine;
using TMPro;

public class NewHomePageCompetitionController : SimpleButtonController 
{	
	public override void OnMouseDown()
	{
		NewHomePageController.instance.joinGameHandler(base.getId());	
	}
}

