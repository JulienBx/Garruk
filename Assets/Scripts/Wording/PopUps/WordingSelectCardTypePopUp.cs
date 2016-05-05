using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSelectCardTypePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSelectCardTypePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Veuillez choisir une faction :","Choose your faction"}); //0
	}
}