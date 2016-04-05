using UnityEngine;
using TMPro;

public class NewStoreBuyCreditsButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		if(ApplicationDesignRules.isMobileDevice)
		{
			NewStoreController.instance.displayProductsPopUp ();
		}
		else
		{
			NewStoreController.instance.desktopPurchasingHandler();
		}
	}
	public override void setIsActive(bool value)
	{
		base.setIsActive (value);
		if(value)
		{
			base.setInitialState();
		}
		else
		{
			base.setForbiddenState();
		}
	}
}