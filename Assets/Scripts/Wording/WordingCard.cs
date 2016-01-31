using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCard
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingCard()
	{
		references=new List<string[]>();
		references.Add(new string[]{". P : ",""}); //0
		references.Add(new string[]{"%",""}); //1
	}
}