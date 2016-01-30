using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class SelectCardTypePopUpController : MonoBehaviour 
{
	public void reset(List<int> cardTypesAllowed)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSelectCardTypePopUp.getReference(0);
		for(int i=0;i<10;i++)
		{
			gameObject.transform.FindChild ("picture"+i).GetComponent<SelectCardTypePopUpPictureController> ().reset ();
			if(cardTypesAllowed.Contains(i))
			{
				gameObject.transform.FindChild ("picture"+i).GetComponent<SelectCardTypePopUpPictureController> ().setIsActive(true);
			}
			else
			{
				gameObject.transform.FindChild ("picture"+i).GetComponent<SelectCardTypePopUpPictureController> ().setIsActive(false);
				gameObject.transform.FindChild ("picture"+i).GetComponent<SelectCardTypePopUpPictureController> ().setForbiddenState();
			}
		}
	}
	public void resize()
	{
	}
	public void selectCardTypeHandler(int id)
	{
		NewStoreController.instance.buyPackWidthCardTypeHandler(id);
	}
	public void exitPopUp()
	{
		NewStoreController.instance.hideSelectCardPopUp ();
	}
}

