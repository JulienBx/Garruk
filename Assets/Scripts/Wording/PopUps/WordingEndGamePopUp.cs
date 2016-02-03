using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEndGamePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEndGamePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"BRAVO !\n\nVenez en match officiel vous mesurer aux meilleurs joueurs !",""}); //0
		references.Add(new string[]{"DOMMAGE !\n\nC'est en s'entrainant qu'on progresse ! Courage !",""}); //1
		references.Add(new string[]{"Continuer",""}); //2
	}
}