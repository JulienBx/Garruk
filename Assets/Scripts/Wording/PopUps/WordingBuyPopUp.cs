using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingBuyPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingBuyPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Recruter l'unité pour ","Enlisting this unit will cost you "}); //0
		references.Add(new string[]{" cristaux"," cristals"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}