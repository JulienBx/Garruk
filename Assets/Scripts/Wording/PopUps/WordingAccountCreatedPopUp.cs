using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAccountCreatedPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAccountCreatedPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Félicitations !!  \n \nVotre compte est créé, un email comprenant un lien a été envoyé à votre adresse.  \nEn cliquant sur ce lien vous pourrez activer votre compte et commencer à jouer !",""}); //0
		references.Add(new string[]{"Continuer",""}); //1
	}
}