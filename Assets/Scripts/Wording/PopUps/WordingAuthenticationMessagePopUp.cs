﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAuthenticationMessagePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAuthenticationMessagePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Continuer",""}); //0
		references.Add(new string[]{"Félicitations !!  \n \nVotre compte est créé, un email comprenant un lien a été envoyé à votre adresse.  \nEn cliquant sur ce lien vous pourrez activer votre compte et commencer à jouer !",""}); //1
		references.Add(new string[]{"Un lien permettant de réinitialiser votre mot de passe vient de vous être envoyé sur votre adresse mail.",""}); //2
		references.Add(new string[]{"Un lien d'activation a été envoyé sur votre adresse mail, cliquez dessus pour valider votre email",""}); //3
	}
}