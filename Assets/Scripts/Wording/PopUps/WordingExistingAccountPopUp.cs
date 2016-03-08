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
		references.Add(new string[]{"Merci de remplir les informations suivantes pour finaliser la création de votre compte","Please fill the following form to complete your registration"}); //0
		references.Add(new string[]{"Pseudo","Player name"}); //1
		references.Add(new string[]{"Mot de passe","Password"}); //3
		references.Add(new string[]{"Valider","Confirm"}); //4
		references.Add(new string[]{"Pseudo ou mot de passe non renseigné","Missing player name or password information"}); //5
	}
}