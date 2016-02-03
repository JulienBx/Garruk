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
		references.Add(new string[]{"Entrez votre nouveau mot de passe",""}); //0
		references.Add(new string[]{"Confirmez la saisie",""}); //1
		references.Add(new string[]{"Confirmer",""}); //2
	}
}