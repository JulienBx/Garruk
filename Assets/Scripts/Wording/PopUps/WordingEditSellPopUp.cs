using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEditSellPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingEditSellPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Unité sur le marché pour ",""}); //0
		references.Add(new string[]{" cristaux. Modifier ?",""}); //1
		references.Add(new string[]{"Retirer",""}); //2
		references.Add(new string[]{"Modifier",""}); //3
	}
}