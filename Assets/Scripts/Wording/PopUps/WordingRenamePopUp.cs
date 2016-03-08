using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingRenamePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingRenamePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Renommer l'unité pour ","Give your unit a name. This will cost you "}); //0
		references.Add(new string[]{" cristaux"," cristals"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}