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
		references.Add(new string[]{"Choisir à quel prix la carte sera proposée aux enchères \n\n Prix suggéré : ","Choose your unit selling price  \n\n This unit is evaluated to "}); //0
		references.Add(new string[]{" cristaux (évaluation proposée par la chambre de commerce cristalienne)"," crystals according to Cristalia stocks exchange"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}