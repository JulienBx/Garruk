using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCheckPasswordPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingCheckPasswordPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Saisissez votre mot de passe",""}); //0
		references.Add(new string[]{"Confirmer",""}); //1
	}
}