using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingConnectionBonusPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingConnectionBonusPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Un nouveau jour sur Cristalia se lève. Vous recevez ","This is your first appearance on Cristalia today. You receive "}); //0
		references.Add(new string[]{" cristaux de votre terre mère"," cristals from your mother earth"}); //1
		references.Add(new string[]{"OK!","OK!"}); //2
	}
}