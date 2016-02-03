using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSocial
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSocial()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Défier",""}); //0
		references.Add(new string[]{"Oui",""}); //1
		references.Add(new string[]{"Non",""}); //2
		references.Add(new string[]{"n'est pas en ligne",""}); //3
		references.Add(new string[]{"est disponible pour un défi",""}); //4
		references.Add(new string[]{"est entrain de jouer",""}); //5
		references.Add(new string[]{"En attente de la réponse de votre ami ...",""}); //6
		references.Add(new string[]{"Votre ami a annulé le défi",""});//7
		references.Add(new string[]{"vous a invité à devenir son ami, accepter ?",""});//8
		references.Add(new string[]{"n'a pas encore répondu à votre invitation, annuler ?",""});//9
		references.Add(new string[]{"Retirer",""});//10
		references.Add(new string[]{"Vous êtes amis",""});//11
		references.Add(new string[]{"Accepter",""});//12
		references.Add(new string[]{"Refuser",""});//13
		references.Add(new string[]{"Souhaite faire parti de vos amis",""});//14
		references.Add(new string[]{"Annuler",""});//15
		references.Add(new string[]{"n'a pas encore répondu à votre invitation",""});//16
		references.Add(new string[]{"Ajouter",""});//17
		references.Add(new string[]{"ne fait pas partie de vos amis",""});//18
	}
}