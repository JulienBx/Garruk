using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAuthentication
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAuthentication()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Connexion à Techtical Wars ...","Connecting to Techtical Wars..."}); //0
		references.Add(new string[]{"Veuillez saisir votre pseudo","Enter your player name"}); //1
		references.Add(new string[]{"Votre pseudo doit comporter au moins 3 caractères","Your player name must be made of more than 3 characters"}); //2
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux","You can not use special characters"}); //3
		references.Add(new string[]{"Veuillez saisir un mot de passe","Enter your password"}); //4
		references.Add(new string[]{"Veuillez confirmer votre mot de passe","Confirm your password"}); //5
		references.Add(new string[]{"Les deux mots de passes doivent être identiques","Both passwords are not identical"});//6
		references.Add(new string[]{"Le mot de passe doit comporter au moins 5 caractères","Your password must be made of more than 4 characters"});//7
		references.Add(new string[]{"Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .","Your password can not be made of special characters "});//8
		references.Add(new string[]{"Veuillez saisir une adresse email valide","Please enter a valid email adress"});//9
		references.Add(new string[]{"Connexion avec Facebook","Facebook connect"});//10
		references.Add(new string[]{"Inscription avec Facebook","Register with your facebook account"});//11
		references.Add(new string[]{"Le pseudo ne doit pas comporter plus de 12 caractères","Player name can not exceed 12 characters"}); //12
		references.Add(new string[]{"Le jeu n'est pas accessible actuellement, veuillez réessayer plus tard","Techtical Wars is down and will be back soon, please retry in a few minuts"}); //13
	}
}