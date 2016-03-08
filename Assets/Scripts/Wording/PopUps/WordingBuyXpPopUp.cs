using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingBuyXpPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingBuyXpPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Entrainer l'unité pour ","Training this unit will cost you "}); //0
		references.Add(new string[]{" cristaux"," cristals"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}