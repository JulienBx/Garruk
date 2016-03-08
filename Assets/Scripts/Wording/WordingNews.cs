using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNews
{
	public static IList<string[]> references;
	public static IList<string[]> contents;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getContent(int idNotificationType)
	{
		return contents[idNotificationType][ApplicationModel.player.IdLanguage];
	}
	static WordingNews()
	{
		references=new List<string[]>();

		contents=new List<string[]>();
		contents.Add(new string[]{"est désormais ami avec #*user*#","is now friend with #*user*#"}); //0
		contents.Add(new string[]{"a conquis #*trophy*#"," has conquished #*trophy*#"}); //1
		contents.Add(new string[]{"a remporté #*trophy*#","has won #*trophy*#"}); //2
	}
}