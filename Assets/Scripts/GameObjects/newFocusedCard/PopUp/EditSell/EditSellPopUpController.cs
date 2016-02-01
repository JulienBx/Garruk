using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class EditSellPopUpController : MonoBehaviour 
{
	public void reset(int price)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEditSellPopUp.getReference(0)+ price+WordingEditSellPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEditSellPopUp.getReference(2);
		gameObject.transform.FindChild ("Button").GetComponent<EditSellPopUpUnsellButtonController> ().reset ();
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingEditSellPopUp.getReference(3);
		gameObject.transform.FindChild ("Button2").GetComponent<EditSellPopUpEditPriceButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<EditSellPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void unsellHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().unsellCardHandler ();
	}
	public void editPriceHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().displayEditSellPriceCardPopUp();
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideEditSellPopUp ();
	}
	
}

