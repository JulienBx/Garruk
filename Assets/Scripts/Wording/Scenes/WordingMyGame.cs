using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMyGame
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingMyGame()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Mes unités","My units"}); //0
		references.Add(new string[]{"Mes équipes","My teams"}); //1
	}
}