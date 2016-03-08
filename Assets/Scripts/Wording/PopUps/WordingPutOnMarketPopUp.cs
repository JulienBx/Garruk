using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPutOnMarketPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPutOnMarketPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisir le prix de vente de la carte","Choose your unit selling price"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}