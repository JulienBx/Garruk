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
		references.Add(new string[]{"En vente","Offers"}); //1
		references.Add(new string[]{"Mes unités","My offers"}); //2
		references.Add(new string[]{"Vendre","My stock"}); //3
		references.Add(new string[]{"Marché","Market"}); //4
		references.Add(new string[]{"Bienvenue sur le marché. Vous pouvez ici acheter des unités mises en vente par d'autres colons, et mettre en vente vos propres unités. Attention les unités faisant partie d'une équipe ne pourront être mises en vente","Welcome to the market. Here you will find unit selling offers made by cristalian colonists. You can either buy units by choosing an offer or put your stock units on sale. Be careful, you won't be able to sell units that belong to a team."}); //5
		references.Add(new string[]{"Cette unité vient d'être vendue, et n'est plus accessible","This unit has just been sold and cannot be consulted anymore"}); //6
		references.Add(new string[]{"Acheter",""}); //7
		references.Add(new string[]{"Acheter l'unité",""}); //8
		references.Add(new string[]{"Changer le prix de vente",""}); //9
		references.Add(new string[]{"Le prix de vente peut être modifié à tout moment. N'hésitez pas à baisser le prix si votre unité ne trouve pas preneur!",""}); //10
		references.Add(new string[]{"Mettre en vente",""}); //11
		references.Add(new string[]{"Vous pourrez choisir le prix auquel vous souhaitez vendre votre unité. Celle-ci sera visible de tous sur le marché, et pourra alors être achetée par un colon.",""}); //12
	}
}