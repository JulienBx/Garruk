using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingConnectionBonusPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingConnectionBonusPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Première connection de la journée, vous gagnez ",""}); //0
		references.Add(new string[]{" cristaux",""}); //1
		references.Add(new string[]{"Confirmer",""}); //2
	}
}