using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingInscriptionFacebookPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingInscriptionFacebookPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Inscription rapide",""}); //0
		references.Add(new string[]{"Pseudo",""}); //1
		references.Add(new string[]{"Entre 3 et 12 caractères",""}); //2
		references.Add(new string[]{"Mot de passe",""}); //3
		references.Add(new string[]{"5 caractères minimum",""}); //4
		references.Add(new string[]{"Confirmer mot de passe",""}); //5
		references.Add(new string[]{"Email",""});//6
		references.Add(new string[]{"Par défaut votre email facebook",""});//7
		references.Add(new string[]{"C'est parti",""});//8
		references.Add(new string[]{"Si vous avez déjà un compte, cliquez ici",""});//9
	}
}