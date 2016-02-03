using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDisconnectPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingDisconnectPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Souhaitez-vous quitter le jeu ?",""}); //0
		references.Add(new string[]{"Confirmer",""}); //1
		references.Add(new string[]{"Annuler",""}); //2

	}
}