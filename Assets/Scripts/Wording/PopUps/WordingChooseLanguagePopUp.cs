using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingChooseLanguagePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingChooseLanguagePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Sélectionnez la langue de votre choix",""}); //0
	}
}