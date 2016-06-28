using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMenu
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingMenu()
	{
		references=new List<string[]>();

		// Libellés menu desktop

		references.Add(new string[]{"Accueil","Home"}); //0
		references.Add(new string[]{"Mes unités","My units"}); //1
		references.Add(new string[]{"Recrutement","Enlist"}); //2
		references.Add(new string[]{"Marché","Market"}); //3
		references.Add(new string[]{"Cristalopedia","Cristalopedia"}); //4
		references.Add(new string[]{"Arene","Fight"}); //5

		// Libellés menu mobile

		references.Add(new string[]{"Home","Home"}); //6
		references.Add(new string[]{"Units","Units"}); //7
		references.Add(new string[]{"Enlist","Enlist"}); //8
		references.Add(new string[]{"Market","Market"}); //9
		references.Add(new string[]{"Book","Book"}); //10
		references.Add(new string[]{"Fight","Fight"}); //11
	}
}