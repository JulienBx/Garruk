using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLoginPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLoginPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Connectez-vous",""}); //0
		references.Add(new string[]{"Login",""}); //1
		references.Add(new string[]{"Mot de passe",""}); //2
		references.Add(new string[]{"Se souvenir de moi",""}); //3
		references.Add(new string[]{"Se connecter",""}); //4
	}
}