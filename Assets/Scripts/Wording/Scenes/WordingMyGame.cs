using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMyGame
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingMyGame()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Mes unités",""}); //0
		references.Add(new string[]{"Mes armées",""}); //1
	}
}