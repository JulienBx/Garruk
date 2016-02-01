using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSelectPicturePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingSelectPicturePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Sélectionnez un avatar",""}); //0
		references.Add(new string[]{"Confirmer",""}); //1
	}
}