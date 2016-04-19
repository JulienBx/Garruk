using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSoundSettingsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSoundSettingsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Volume du son","Sounds volume"}); //0
		references.Add(new string[]{"Volume de la musique","Music volume"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}