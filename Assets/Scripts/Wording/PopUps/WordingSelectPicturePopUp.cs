using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSelectPicturePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSelectPicturePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Veuillez sélectionner un avatar","Select an avatar"}); //0
		references.Add(new string[]{"Confirmer","Confirm"}); //1
	}
}