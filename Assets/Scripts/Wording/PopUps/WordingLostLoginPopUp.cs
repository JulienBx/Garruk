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
		references.Add(new string[]{"Changer son mot de passe","Changing your password"}); //0
		references.Add(new string[]{"Entrez votre pseudo ou l'adresse email avec laquelle vous vous êtes inscrit. Nous vous enverrons dans les 5 prochaines minutes un email avec un lien pour réinitialiser votre mot de passe.","Please type down your player name ou your email so that we can send you a link to change your password."}); //1
		references.Add(new string[]{"Envoyer","Send"}); //1
	}
}