﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEditSellPricePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEditSellPricePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Changer le prix de l'unité sur le marché","Change the unit selling price"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}