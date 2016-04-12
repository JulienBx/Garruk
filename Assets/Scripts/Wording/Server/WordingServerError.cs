using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingServerError
{
	public static IList<string[]> references;

	public static string getReference(string error, bool identifiedError)
	{
		if(identifiedError)
		{
			if(error.StartsWith("0"))
			{
				return references[0][ApplicationModel.player.IdLanguage]+error.Substring(1,error.Length-1)+references[1][ApplicationModel.player.IdLanguage];
			}
			else
			{
				return references[System.Convert.ToInt32(error)][ApplicationModel.player.IdLanguage];
			}
		}
		else
		{
			return references[0][ApplicationModel.player.IdLanguage]+error;
		}
	}
	static WordingServerError()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Erreur système code ",""}); //0
		references.Add(new string[]{"\n\n Veuillez réessayer ultérieurement. \n\n Si l'erreur persiste contactez notre support admin@techticalwars.com",""}); //1
		references.Add(new string[]{"Mauvais mot de passe","Wrong password"}); //2
		references.Add(new string[]{"Mauvais login ou email","Wrong email or login"}); //3
		references.Add(new string[]{"Demande déjà existante",""}); //4
		references.Add(new string[]{"Connexion expirée, veuillez réessayer",""}); //5
		references.Add(new string[]{"Demande déjà existante",""}); //6
		references.Add(new string[]{"Le vendeur a changé le prix de vente de la carte",""}); //7
		references.Add(new string[]{"La carte est déjà vendue",""}); //8
		references.Add(new string[]{"Vous n'avez pas suffisamment de crédits",""}); //9
		references.Add(new string[]{"Vous ne pouvez pas acheter une carte qui vous appartient déjà",""}); //10
		references.Add(new string[]{"Vous ne pouvez pas encore acheter de carte",""}); //11
		references.Add(new string[]{"Vous ne pouvez pas encore acheter de carte de cette faction",""}); //12
		references.Add(new string[]{"La demander n'existe plus",""}); //13
		references.Add(new string[]{"La demande n'a pas pu être traitée",""}); //14
		references.Add(new string[]{"Pseudo ou email déjà existant",""}); //15
	}
}