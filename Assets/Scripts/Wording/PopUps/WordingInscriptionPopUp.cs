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
		references.Add(new string[]{"Inscription rapide",""}); //0
		references.Add(new string[]{"Pseudo",""}); //1
		references.Add(new string[]{"3 caractères minimum",""}); //2
		references.Add(new string[]{"Mot de passe",""}); //3
		references.Add(new string[]{"5 caractères minimum",""}); //4
		references.Add(new string[]{"Confirmer mot de passe",""}); //5
		references.Add(new string[]{"Adresse email",""});//6
		references.Add(new string[]{"C'est parti",""});//7
	}
}