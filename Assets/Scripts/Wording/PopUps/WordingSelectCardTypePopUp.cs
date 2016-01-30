using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSelectCardTypePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingSelectCardTypePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisissez la faction :",""}); //0
	}
}