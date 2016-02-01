using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPlayPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingPlayPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisir un type de match",""}); //0
		references.Add(new string[]{"Annuler",""}); //1
		references.Add(new string[]{"Refuser",""}); //2

	}
}