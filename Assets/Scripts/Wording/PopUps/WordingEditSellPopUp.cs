using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEditSellPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEditSellPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"L'unité a été mise en vente sur le marché pour ","This unit has been offered for sale. Price has been set at "}); //0
		references.Add(new string[]{" cristaux. Voulez-vous modifier le prix ?","cristals. Do you want to change the selling price?"}); //1
		references.Add(new string[]{"Retirer du marché","Withdraw"}); //2
		references.Add(new string[]{"Modifier le prix","Change the price"}); //3
	}
}