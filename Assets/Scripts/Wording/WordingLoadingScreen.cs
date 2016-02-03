using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLoadingScreen
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLoadingScreen()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Chargement...","Loading..."}); //0
	}
}