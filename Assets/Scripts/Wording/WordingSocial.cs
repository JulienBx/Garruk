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
		references.Add(new string[]{"Défier","Challenge"}); //0
		references.Add(new string[]{"Oui","Yes"}); //1
		references.Add(new string[]{"Non","No"}); //2
		references.Add(new string[]{"n'est pas en ligne","is offline"}); //3
		references.Add(new string[]{"est disponible pour un défi","is available for a challenge"}); //4
		references.Add(new string[]{"est en train de jouer","is fighting"}); //5
		references.Add(new string[]{"En attente de la réponse de votre ami...","Waiting for your friend answer..."}); //6
		references.Add(new string[]{"Votre ami a refusé le défi","Your friend declined the challenge"});//7
		references.Add(new string[]{"vous a invité à devenir son ami, accepter?","requested you to become his friend, do you accept?"});//8
		references.Add(new string[]{"n'a pas encore répondu à votre invitation, annuler?","hasn't answered your friend request yet, do you want to cancel?"});//9
		references.Add(new string[]{"Retirer","Withdraw"});//10
		references.Add(new string[]{"Vous êtes amis","You are friends"});//11
		references.Add(new string[]{"Accepter","Accept"});//12
		references.Add(new string[]{"Refuser","Decline"});//13
		references.Add(new string[]{"Souhaite faire partie de vos amis","Wants to become your friend"});//14
		references.Add(new string[]{"Annuler","Cancel"});//15
		references.Add(new string[]{"n'a pas encore répondu à votre invitation","hasn't answered your request yet"});//16
		references.Add(new string[]{"Ajouter","Add"});//17
		references.Add(new string[]{"ne fait pas partie de vos amis","does not belong to your friends"});//18
	}
}