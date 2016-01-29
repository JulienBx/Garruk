using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNewCardTypePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingNewCardTypePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Félicications !!\n\nVous avez débloqué la faction :\n\n",""}); //0
		references.Add(new string[]{"Continuer",""}); //1
	}
}