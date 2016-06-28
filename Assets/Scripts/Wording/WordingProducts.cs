using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProducts
{
	public static IList<string[]> references;

	public static string getReferences(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingProducts()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Non dispo","Unavailable"}); //0
		references.Add(new string[]{"Choisissez votre offre :","Choose an offer :"}); //1
        references.Add(new string[]{" cristaux"," cristals"}); //2
        references.Add(new string[]{"Acheter","Buy"}); //3
		references.Add(new string[]{"La boutique n'est pas disponible, revenez plus tard !\nCode erreur : ","Shop is closed, please come back later.\nError code :"}); //4
	}
}