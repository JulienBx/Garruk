using UnityEngine;
using TMPro;

public class NewHomePageTabController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		NewHomePageController.instance.selectATabHandler(base.getId());	
	}
}

