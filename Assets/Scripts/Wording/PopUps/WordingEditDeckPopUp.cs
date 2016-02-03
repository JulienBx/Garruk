using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEditDeckPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEditDeckPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisissez le nom de votre nouvelle équipe",""}); //0
		references.Add(new string[]{"Confirmer",""}); //1
	}
}