using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingInvitationPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingInvitationPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Vous a lancé un défi",""}); //0
		references.Add(new string[]{"Accepter",""}); //1
		references.Add(new string[]{"Refuser",""}); //2

	}
}