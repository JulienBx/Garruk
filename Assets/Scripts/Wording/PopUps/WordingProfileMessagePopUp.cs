using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProfileMessagePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingProfileMessagePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Continuer",""}); //0
		references.Add(new string[]{"Vous avez changé d'adresse mail. Pour pouvoir vous connecter à nouveau, il vous suffira de cliquer sur le lien d'activation qui vient de vous être envoyé par mail.",""}); //3
	}
}