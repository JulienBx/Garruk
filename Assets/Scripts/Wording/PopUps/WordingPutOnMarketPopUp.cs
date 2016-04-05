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
		references.Add(new string[]{"Choisir le prix de vente de la carte \n\n Cette carte est aujourd'hui évaluée à ","Choose your unit selling price  \n\n This unit is evaluated to "}); //0
		references.Add(new string[]{" cristaux selon la bourse de Cristalia."," crystals according to Cristalia stocks exchange"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}