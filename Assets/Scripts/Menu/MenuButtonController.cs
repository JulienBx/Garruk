using UnityEngine;
using TMPro;

public class MenuButtonController : SimpleButtonController
{
	public override void OnMouseDown()
	{
		MenuController.instance.changePage (base.getId());	
	}
}

