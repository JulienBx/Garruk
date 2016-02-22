using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPagination
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPagination()
	{
		references=new List<string[]>();
		references.Add(new string[]{"carte ",""}); //0
		references.Add(new string[]{" à ",""}); //1
		references.Add(new string[]{" sur ",""}); //2
		references.Add(new string[]{"aucune carte à afficher",""}); //3
		references.Add(new string[]{"compétence ",""}); //4
		references.Add(new string[]{"aucune compétence à afficher",""}); //5
		references.Add(new string[]{"pack ",""}); //6
		references.Add(new string[]{"aucun pack à afficher",""}); //7
	}
}