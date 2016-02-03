using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSoldCardPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSoldCardPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Votre carte vient d'être vendue.",""}); //0
		references.Add(new string[]{"Quitter",""}); //1
	}
}