using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSellPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSellPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Confirmer le bannissement de l'unité (rapporte ",""}); //0
		references.Add(new string[]{" cristaux). \n\n Attention cette action est irréversible ! \n Vous perdrez définitivement votre unité.\n",""}); //1
		references.Add(new string[]{"Confirmer",""}); //2
	}
}