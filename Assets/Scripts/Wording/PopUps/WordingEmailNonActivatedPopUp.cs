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
		references.Add(new string[]{"Email en attente de validation",""}); //0
		references.Add(new string[]{"Un lien d'activation vous a été envoyé précédemment dans un email  \nVous pouvez modifier votre email de contact et/ou envoyer un nouveau mail d'activation",""}); //1
		references.Add(new string[]{"Envoyer",""}); //0
		references.Add(new string[]{"Retour",""}); //0
	}
}