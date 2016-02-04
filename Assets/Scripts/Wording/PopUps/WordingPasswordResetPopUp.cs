using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPasswordResetPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPasswordResetPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Un lien permettant de réinitialiser votre mot de passe vient de vous être envoyé sur votre adresse mail.",""}); //0
		references.Add(new string[]{"Continuer",""}); //1
	}
}