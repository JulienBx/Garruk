using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingBuyPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingBuyPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Confirmer le recrutement de l'unité (coûte ",""}); //0
		references.Add(new string[]{" cristaux)",""}); //1
		references.Add(new string[]{"Confirmer",""}); //2
	}
}