using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSoldCardPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSoldCardPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Cette carte vient d'être vendue","This unit has just been sold."}); //0
		references.Add(new string[]{"OK!","OK!"}); //1
	}
}