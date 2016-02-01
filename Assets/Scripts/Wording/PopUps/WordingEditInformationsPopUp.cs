using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEditInformationsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingEditInformationsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Prénom",""}); //0
		references.Add(new string[]{"Nom",""}); //1
		references.Add(new string[]{"Mail",""}); //2
		references.Add(new string[]{"Confirmer",""}); //3
	}
}