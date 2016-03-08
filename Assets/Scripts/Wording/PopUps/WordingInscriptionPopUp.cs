using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingInscriptionPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingInscriptionPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Inscription rapide","Quick registration"}); //0
		references.Add(new string[]{"Pseudo","Player name"}); //1
		references.Add(new string[]{"Entre 3 et 12 caractères","Between 3 and 12 characters"}); //2
		references.Add(new string[]{"Mot de passe","Password"}); //3
		references.Add(new string[]{"5 caractères minimum","At least 5 characters"}); //4
		references.Add(new string[]{"Confirmer mot de passe","Confirm your password"}); //5
		references.Add(new string[]{"Adresse email","Email address"});//6
		references.Add(new string[]{"C'est parti","Let's start!"});//7
		references.Add(new string[]{"Ou","Or"});//8
	}
}