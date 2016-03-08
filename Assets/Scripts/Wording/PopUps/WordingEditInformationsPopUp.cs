using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEditInformationsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEditInformationsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Prénom","First Name"}); //0
		references.Add(new string[]{"Nom","Surname"}); //1
		references.Add(new string[]{"Adresse Email","Email address"}); //2
		references.Add(new string[]{"Confirmer","Confirm"}); //3
	}
}