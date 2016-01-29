using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCollectionPointsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingCollectionPointsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Collections Points : + ",""}); //0
		references.Add(new string[]{"\nClassement : ",""}); //1

	}
}