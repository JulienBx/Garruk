using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMenu
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingMenu()
	{
		references=new List<string[]>();

		// Libellés menu desktop

		references.Add(new string[]{"Accueil",""}); //0
		references.Add(new string[]{"Mes unités",""}); //1
		references.Add(new string[]{"Recruter",""}); //2
		references.Add(new string[]{"Le marché",""}); //3
		references.Add(new string[]{"Cristalopedia",""}); //4
		references.Add(new string[]{"Jouer",""}); //5

		// Libellés menu mobile

		references.Add(new string[]{"Home",""}); //6
		references.Add(new string[]{"Units",""}); //7
		references.Add(new string[]{"Shop",""}); //8
		references.Add(new string[]{"Market",""}); //9
		references.Add(new string[]{"Wiki",""}); //10
		references.Add(new string[]{"Play",""}); //11
	}
}