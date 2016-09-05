using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDetectOfflinePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingDetectOfflinePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Perte de réseau détecté, vous êtes désormais hors ligne","Loss of connexion detected, you're now offline"}); //0
		references.Add(new string[]{"Ok","Ok"}); //1
	}
}