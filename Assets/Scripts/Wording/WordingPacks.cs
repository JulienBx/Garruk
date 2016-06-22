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
		names.Add(new string[]{"Solo pack aléatoire","Solo random pack"});
		names.Add(new string[]{"Multi-5 pack aléatoire","Multi-5 random pack"});
		names.Add(new string[]{"Solo pack - choix de la faction","Solo pack - choose your faction"});
		names.Add(new string[]{"Multi-5 pack - choix de la faction","Multi-5 pack - choose your faction"});

		references=new List<string[]>();
		references.Add(new string[]{"Payer ","Buy "});
		references.Add(new string[]{"Achat d'un pack","Buy a pack"});
		references.Add(new string[]{"achat d'un pack ...","Buy a pack..."});
	}
}