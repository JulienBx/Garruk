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
		references.Add(new string[]{"vous a lancé un défi","wants to challenge you"}); //0
		references.Add(new string[]{"Accepter","Accept"}); //1
		references.Add(new string[]{"Refuser","Decline"}); //2

	}
}