using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAuthentication
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAuthentication()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Inscription",""}); //0
	}
}