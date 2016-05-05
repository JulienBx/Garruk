using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCollectionPointsPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingCollectionPointsPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Points de collection + ","Collection Score : "}); //0
		references.Add(new string[]{"\nClassement collectionneur : ","\nRanking : "}); //1

	}
}