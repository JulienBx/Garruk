using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingWonPackPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingWonPackPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"De nouvelles cartes vous attendent au centre de recrutement!","New cards are available in the store!"}); //0
		references.Add(new string[]{"OK!","OK!"}); //1
	}
}