using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSearchUsersPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSearchUsersPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Résultats de la recherche:","Search results"}); //0
		references.Add(new string[]{"Quitter","Quit"}); //1
		references.Add(new string[]{"Désolé, aucun résultat ne correspond à votre recherche !","Sorry ! We have not found any user corresponding to your search input"}); //2
	}
}