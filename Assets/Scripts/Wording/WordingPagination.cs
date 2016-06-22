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
		references.Add(new string[]{"carte ","card "}); //0
		references.Add(new string[]{" à "," to "}); //1
		references.Add(new string[]{" sur "," on "}); //2
		references.Add(new string[]{"aucune unité à afficher","No unit to display"}); //3
		references.Add(new string[]{"compétence ","skill "}); //4
		references.Add(new string[]{"aucune compétence à afficher","No skill to display"}); //5
		references.Add(new string[]{"pack ","pack "}); //6
		references.Add(new string[]{"aucun pack à afficher","no pack to display"}); //7
	}
}