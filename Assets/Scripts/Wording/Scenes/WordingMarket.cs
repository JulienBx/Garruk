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
		references.Add(new string[]{"Actualiser",""}); //0
		references.Add(new string[]{"Offres",""}); //1
		references.Add(new string[]{"Mes ventes",""}); //2
		references.Add(new string[]{"Mes unités",""}); //3
		references.Add(new string[]{"Le marché",""}); //4
		references.Add(new string[]{"Bienvenue sur le marché. Vous pouvez ici acheter des cartes à d'autres joueurs et vendre vos cartes. Attention seules les cartes qui ne sont pas déjà ratachées à une équipe peuvent être mises en vente",""}); //5
		references.Add(new string[]{"Cette carte a été vendue, vous ne pouvez plus la consulter",""}); //6
	}
}