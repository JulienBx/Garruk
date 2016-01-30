using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingFilters
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingFilters()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Filtrer",""}); //0
		references.Add(new string[]{"Compétence",""}); //1
		references.Add(new string[]{"Rechercher",""}); //2
		references.Add(new string[]{"Faction",""}); //3
		references.Add(new string[]{"Attribut",""}); //4
		references.Add(new string[]{"Rares",""}); //5
		references.Add(new string[]{"Très rares",""}); //6
		references.Add(new string[]{"Toutes",""}); //7
		references.Add(new string[]{"Prix",""}); //8
		references.Add(new string[]{"Acquises",""}); //9
		references.Add(new string[]{"Manquantes",""}); //10
		references.Add(new string[]{"Discipline",""}); //11
		references.Add(new string[]{"Disponibilité",""}); //12
	}
}