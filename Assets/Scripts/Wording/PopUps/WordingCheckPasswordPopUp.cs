using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCheckPasswordPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingCheckPasswordPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Saisissez votre mot de passe","Fill a password to secure your account"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}