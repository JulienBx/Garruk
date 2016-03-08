using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCard
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingCard()
	{
		references=new List<string[]>();
		references.Add(new string[]{". P : ",". P : "}); //0
		references.Add(new string[]{"%","%"}); //1
		references.Add(new string[]{"VENDU","SOLD"}); //2
	}
}