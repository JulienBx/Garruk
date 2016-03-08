using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNewCardTypePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingNewCardTypePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Félicitations !!\n\nVous avez débloqué la faction :\n\n","Congratulations!!\n\nYou unlocked a new unit faction :\n\n"}); //0
		references.Add(new string[]{"OK!","OK!"}); //1
	}
}