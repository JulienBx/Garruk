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
		return references[idReference][ApplicationModel.idLanguage];
	}
	public static string getContent(int idNotificationType)
	{
		return contents[idNotificationType][ApplicationModel.idLanguage];
	}
	static WordingNews()
	{
		references=new List<string[]>();

		contents=new List<string[]>();
		contents.Add(new string[]{"est désormais ami avec #*user*#","#*user*#"}); //0
		contents.Add(new string[]{"a conquis #*trophy*#","#*trophy*#"}); //1
		contents.Add(new string[]{"a remporté #*trophy*#","#*trophy*#"}); //2
	}
}