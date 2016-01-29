using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHomePage
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingHomePage()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Mon armée",""}); //0
		references.Add(new string[]{"Jouer",""}); //1
		references.Add(new string[]{"Recruter",""}); //2
		references.Add(new string[]{"Alertes",""}); //3
		references.Add(new string[]{"News",""}); //4
		references.Add(new string[]{"Amis",""}); //5

	}
}