using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNotifications
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingNotifications()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Nouveau !",""}); //0
	}
}