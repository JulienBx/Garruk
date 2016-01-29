using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDeck
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingDeck()
	{
		references=new List<string[]>();
		references.Add(new string[]{"CAPITAINE \n1er à jouer",""}); //0
		references.Add(new string[]{"LIEUTENANT \n2ème à jouer",""}); //1
		references.Add(new string[]{"SERGENT \n3ème à jouer",""}); //2
		references.Add(new string[]{"SOLDAT \n4ème à jouer",""}); //3
		references.Add(new string[]{"Aucune armée formée",""}); //4
		references.Add(new string[]{"Aucune armée",""});//5 <-- libellé nécessairement court
		references.Add(new string[]{"Armée sélectionnée",""}); //6
	}
}