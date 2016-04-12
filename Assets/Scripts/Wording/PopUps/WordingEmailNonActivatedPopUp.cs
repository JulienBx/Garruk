using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEmailNonActivatedPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEmailNonActivatedPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Votre compte n'a pas encore été validé","We are still waiting for your account confirmation"}); //0
		references.Add(new string[]{"Un email vous a été envoyé avec un lien d'activation  \nVous pouvez modifier votre addresse mail et/ou demander qu'on vous envoie un nouveau mail d'activation","An email has been sent to your mail box.  \n You can change your mail address or ask for a new mail to be sent"}); //1
		references.Add(new string[]{"Renvoyer","Send a new mail"}); //0
		references.Add(new string[]{"OK!","OK!"}); //0
	}
}