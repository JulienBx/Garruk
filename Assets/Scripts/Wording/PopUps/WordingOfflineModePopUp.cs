using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingOfflineModePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingOfflineModePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Vous n'êtes pas connecté à internet, le jeu passe en mode hors ligne","You're not online ! Offline mode is now activated"}); //0
		references.Add(new string[]{"Continuer","Continue"}); //1
	}
}