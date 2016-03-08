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
		references.Add(new string[]{"Souhaitez-vous quitter Cristalia ?","Do you want to quit Cristalia ?"}); //0
		references.Add(new string[]{"Oui!","Yes!"}); //1
		references.Add(new string[]{"Annuler","Cancel"}); //2

	}
}