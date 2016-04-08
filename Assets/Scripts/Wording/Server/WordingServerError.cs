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

	}
}