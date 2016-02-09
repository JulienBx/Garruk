using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAuthenticationChangePasswordPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAuthenticationChangePasswordPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Choisissez un nouveau mot de passe",""}); //0
		references.Add(new string[]{"Mot de passe",""}); //1
		references.Add(new string[]{"5 caractères minimum",""}); //2
		references.Add(new string[]{"Confirmer mot de passe",""}); //3
		references.Add(new string[]{"Continuer",""});//4
	}
}