using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAddCreditsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingAddCreditsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Combien de crédits souhaitez-vous ajouter ?",""}); //0
		references.Add(new string[]{"Confirmer",""}); //1
	}
}