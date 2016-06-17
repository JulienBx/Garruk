using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAddCreditsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAddCreditsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisissez le nombre de cristaux à ajouter à votre portefeuille","How many cristals do you want to buy?"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}