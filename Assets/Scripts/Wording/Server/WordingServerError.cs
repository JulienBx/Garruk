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
		references.Add(new string[]{"Erreur système :","System error :"}); //0
		references.Add(new string[]{"\n\n Veuillez réessayer ultérieurement. \n\n Si l'erreur persiste contactez notre support admin@techticalwars.com","\n\n Please try again later. \n\n If you still get this error send us a mail at admin@techticalwars.com"}); //1
		references.Add(new string[]{"Mauvais mot de passe","Wrong password"}); //2
		references.Add(new string[]{"Mauvais login ou email","Wrong email or login"}); //3
		references.Add(new string[]{"Demande déjà existante","Demand already existing"}); //4
		references.Add(new string[]{"Connexion expirée, veuillez réessayer","Your session has expired, please try again"}); //5
		references.Add(new string[]{"Demande déjà existante","Demande already existing"}); //6
		references.Add(new string[]{"Le vendeur a changé le prix de vente de l'unité","The seller has just changed the unit's price"}); //7
		references.Add(new string[]{"L'unité est déjà vendue","The unit has just been sold"}); //8
		references.Add(new string[]{"Vous n'avez pas suffisamment de crédits","You do not have enough cristals"}); //9
		references.Add(new string[]{"Vous ne pouvez pas acheter une unité qui vous appartient déjà","You can not buy one of your own units"}); //10
		references.Add(new string[]{"Vous ne pouvez pas encore acheter d'unité","you can not buy units yet"}); //11
		references.Add(new string[]{"Vous ne pouvez pas encore acheter d'unité de cette faction","You can not buy units from this faction yet"}); //12
		references.Add(new string[]{"La demande n'existe plus","Offer do not exist anymore"}); //13
		references.Add(new string[]{"La demande n'a pas pu être traitée","We could not proceed your action"}); //14
		references.Add(new string[]{"Pseudo ou email déjà existant","This login or mail already exists"}); //15
		references.Add(new string[]{"L'invitation a expirée,\n veuillez réessayer plus tard","Your invitation has expired,\n please try again later"}); //16
		references.Add(new string[]{"Votre ami est encore en mode ENTRAINEMENT, vous ne pouvez pas encore l'affronter.\n Réessayez plus tard.","Your friend is playing PRACTICE mode.\n Please retry later on"}); //17
		references.Add(new string[]{"Votre ami n'a pas encore constitué son équipe,\n vous ne pouvez pas l'affronter.\n Réessayez plus tard.","Your friend does not have a team ready to fight yet.\nPlease retry later on"}); //18
		references.Add(new string[]{"Votre ami a refusé votre invitation","Your friend has just declined your fight offer"}); //19
		references.Add(new string[]{"Vous ne pouvez pas augmenter cette stat","You can not improve this stat anymore"}); //20
		references.Add(new string[]{"Mail déjà existant","This email already exists"}); //21
		references.Add(new string[]{"Le joueur adverse s'est déconnecté au lancement du match, veuillez réessayer","Your opponent has been disconnected before entering the arena, please try again"}); //22

	}
}