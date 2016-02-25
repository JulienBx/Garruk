using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPermuteCardPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPermuteCardPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Vous ne pouvez pas posséder dans votre équipe 2 cartes ayant la même compétence passive, souhaitez-vous permuter les deux cartes ?",""}); //0
		references.Add(new string[]{"Permuter",""}); //1
		references.Add(new string[]{"Annuler",""}); //2
	}
}