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
		references.Add(new string[]{"OK!","OK!"}); //0
		references.Add(new string[]{"Vous avez modifié votre adresse email. Pour pouvoir vous connecter à nouveau, il vous suffira de cliquer sur le lien d'activation qui vient de vous être envoyé par mail.","You have just changed your emalil account information. You will have to click on the activation link we have sent you before logging in the next time you want to play"}); //3
	}
}