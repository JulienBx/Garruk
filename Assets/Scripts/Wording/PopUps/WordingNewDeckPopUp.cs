using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNewDeckPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingNewDeckPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisissez le nom de votre nouvelle équipe","Choose a name for your new team"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}