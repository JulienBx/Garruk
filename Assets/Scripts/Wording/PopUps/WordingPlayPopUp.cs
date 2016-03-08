using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPlayPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPlayPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisir un mode de bataille","Choose a fight mode"}); //0
		references.Add(new string[]{"Annuler","Cancel"}); //1
		references.Add(new string[]{"Refuser","Decline"}); //2

	}
}