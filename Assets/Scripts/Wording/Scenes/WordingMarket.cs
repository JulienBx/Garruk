using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMarket
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingMarket()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Actualiser","Refresh"}); //0
		references.Add(new string[]{"Ventes","Offers"}); //1
		references.Add(new string[]{"Mes ventes","My offers"}); //2
		references.Add(new string[]{"Ma réserve","My stock"}); //3
		references.Add(new string[]{"Marché","Market"}); //4
		references.Add(new string[]{"Bienvenue sur le marché. Vous pouvez ici acheter des unités mises en vente par d'autres colons, et mettre en vente vos propres unités depuis votre réserve. Attention les unités faisant partie d'une équipe ne pourront être mises en vente","Welcome to the market. Here you will find unit selling offers made by cristalian colonists. You can either buy units by choosing an offer or put your stock units on sale. Be careful, you won't be able to sell units that belong to a team."}); //5
		references.Add(new string[]{"Cette unité vient d'être vendue, et ne peut plus être consultée","This unit has just been sold and cannot be consulted anymore"}); //6
	}
}