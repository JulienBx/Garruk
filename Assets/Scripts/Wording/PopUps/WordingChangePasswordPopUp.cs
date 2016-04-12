using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingChangePasswordPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingChangePasswordPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Nouveau mot de passe","Please fill a new password"}); //0
		references.Add(new string[]{"Confirmez la saisie","Confirm your new password"}); //1
		references.Add(new string[]{"Modifier","Save"}); //2
	}
}