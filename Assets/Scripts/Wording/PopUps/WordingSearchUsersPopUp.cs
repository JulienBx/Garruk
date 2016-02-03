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
		references.Add(new string[]{"Résultats",""}); //0
		references.Add(new string[]{"Quitter",""}); //1
		references.Add(new string[]{"Désolé, aucun résultat ne correspond à votre recherche !",""}); //2
	}
}