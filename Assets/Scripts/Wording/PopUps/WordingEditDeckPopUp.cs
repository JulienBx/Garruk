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
		references.Add(new string[]{"Choisissez un nom pour votre nouvelle équipe","Please choose a name for your team"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}