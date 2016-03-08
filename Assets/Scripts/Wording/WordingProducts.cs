using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProducts
{
	public static IList<string[]> references;

	public static string getReferences(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingProducts()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Non dispo",""});
		references.Add(new string[]{"Choisissez votre bonus :",""});
	}
}