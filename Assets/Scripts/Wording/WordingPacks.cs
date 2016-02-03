using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPacks
{
	public static IList<string[]> references;
	public static IList<string[]> names;

	public static string getName(int idPack)
	{
		return names[idPack][ApplicationModel.player.IdLanguage];
	}
	public static string getReferences(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPacks()
	{
		names=new List<string[]>();
		names.Add(new string[]{"1 Cristalien au hasard",""});
		names.Add(new string[]{"5 Cristaliens au hasard",""});
		names.Add(new string[]{"1 Cristalien de faction",""});
		names.Add(new string[]{"5 Cristaliens de faction",""});

		references=new List<string[]>();
		references.Add(new string[]{"Payer ",""});
	}
}