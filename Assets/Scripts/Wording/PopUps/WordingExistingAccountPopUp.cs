using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingExistingAccountPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingExistingAccountPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Renseignez vos identifiants habituels pour lier votre compte Facebok",""}); //0
		references.Add(new string[]{"Pseudo",""}); //1
		references.Add(new string[]{"Mot de passe",""}); //3
		references.Add(new string[]{"Valider",""}); //4
		references.Add(new string[]{"Login ou mot de passe oublié",""}); //5
	}
}