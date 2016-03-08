using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingFilters
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingFilters()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Filtrer","Filters"}); //0
		references.Add(new string[]{"Compétences","Skills"}); //1
		references.Add(new string[]{"Rechercher","Skill search"}); //2
		references.Add(new string[]{"Factions","Factions"}); //3
		references.Add(new string[]{"Attributs","Stats"}); //4
		references.Add(new string[]{"Rares","Uncommon"}); //5
		references.Add(new string[]{"Très rares","Rare"}); //6
		references.Add(new string[]{"Toutes","All"}); //7
		references.Add(new string[]{"Prix","Price"}); //8
		references.Add(new string[]{"Acquises","Owned"}); //9
		references.Add(new string[]{"Manquantes","Missing"}); //10
		references.Add(new string[]{"Discipline","Skill type"}); //11
		references.Add(new string[]{"Disponibilité","Availability"}); //12
	}
}