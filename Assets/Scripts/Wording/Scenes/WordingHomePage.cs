using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHomePage
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingHomePage()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Mon équipe","My team"}); //0
		references.Add(new string[]{"Arène","Fight"}); //1
		references.Add(new string[]{"Centre de recrutement","Store"}); //2
		references.Add(new string[]{"Alertes","Alerts"}); //3
		references.Add(new string[]{"Actualités","Newsfeed"}); //4
		references.Add(new string[]{"Mes amis","Friends"}); //5
	}
}