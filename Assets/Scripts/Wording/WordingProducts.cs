using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProducts
{
	public static IList<string[]> references;
	public static IList<string[]> names;

	public static string getName(int idProduct)
	{
		return names[idProduct][ApplicationModel.player.IdLanguage];
	}
	public static string getReferences(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingProducts()
	{
		names=new List<string[]>();
		names.Add(new string[]{"Découverte",""});
		names.Add(new string[]{"Passion",""});
		names.Add(new string[]{"Aguerri",""});
		names.Add(new string[]{"Berserk",""});

		references=new List<string[]>();
		references.Add(new string[]{"Payer ",""});
	}
}