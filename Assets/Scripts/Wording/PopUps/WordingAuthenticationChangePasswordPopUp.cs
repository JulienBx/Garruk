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
		references.Add(new string[]{"Choisissez un nouveau mot de passe","Choose a new password"}); //0
		references.Add(new string[]{"Mot de passe","Password"}); //1
		references.Add(new string[]{"5 caractères minimum","At least 5 characters"}); //2
		references.Add(new string[]{"Confirmez votre mot de passe","Confirm your password"}); //3
		references.Add(new string[]{"Continuer","Save"});//4
	}
}