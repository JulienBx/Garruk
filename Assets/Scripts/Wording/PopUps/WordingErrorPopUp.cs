using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingErrorPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingErrorPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Continuer",""}); //0
	}
}