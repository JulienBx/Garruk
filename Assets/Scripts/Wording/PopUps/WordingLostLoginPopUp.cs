using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLostLoginPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLostLoginPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Restauration du mot de passe",""}); //0
		references.Add(new string[]{"Entrez votre pseudo ou l' adresse email avec laquelle vous vous êtes inscrit. Nous allons vous envoyer un email avec votre nom d’utilisateur et un lien pour réinitialiser votre mot de passe.",""}); //1
		references.Add(new string[]{"Envoyer",""}); //1
	}
}