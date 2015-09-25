using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewPackStoreController : NewPackController
{

	public override List<string> getCardTypesAllowed()
	{
		return NewStoreController.instance.getCardTypesAllowed ();
	}
	public override int getCardTypeId(int id)
	{
		return NewStoreController.instance.getCardTypeId (id);
	}
	public override void refreshCredits()
	{
		NewStoreController.instance.refreshCredits ();
	}
	public override void displayLoadingScreen()
	{
		NewStoreController.instance.displayLoadingScreen ();
	}
	public override void hideLoadingScreen()
	{
		NewStoreController.instance.hideLoadingScreen ();
	}
}

