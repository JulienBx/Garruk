using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProfile
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingProfile()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Modifier","Edit"}); //0
		references.Add(new string[]{"- Vider -","- Empty -"}); //1
		references.Add(new string[]{"Victoires","Won"}); //2
		references.Add(new string[]{"Défaites","Loss"}); //3
		references.Add(new string[]{"Classement bataille","Fighter ranking"}); //4
		references.Add(new string[]{"Classement collection","Collection ranking"}); //5
		references.Add(new string[]{"Rechercher","Search"}); //6
		references.Add(new string[]{"Trouver un ami à l'aide de son pseudo","Find a friend by typing his player name"}); //7
		references.Add(new string[]{"Entrez un pseudo","Type a player name"}); //8
		references.Add(new string[]{"OK","OK"}); //9
		references.Add(new string[]{"Conquêtes","Conquest"}); //10
		references.Add(new string[]{"Défis","Challenges"}); //11
		references.Add(new string[]{"Confrontations","Fights with you"}); //12
		references.Add(new string[]{"Amis","Friends"}); //13
		references.Add(new string[]{"Invitations","Requests"}); //14
		references.Add(new string[]{"Votre ami mène avec ","Your friend leads with "}); //15
		references.Add(new string[]{" Victoire(s) contre "," wins against "}); //16
		references.Add(new string[]{" défaite(s)"," loss"}); //17
		references.Add(new string[]{"Vous menez avec ","You lead with "}); //18
		references.Add(new string[]{" Victoire(s) contre "," wins against "}); //19
		references.Add(new string[]{" défaite(s)"," loss"}); //20
		references.Add(new string[]{"Vous êtes à égalité avec ","You tie with"}); //21
		references.Add(new string[]{" victoire(s)"," win(s)"}); //22
		references.Add(new string[]{"Est devenu champion le ","Became champion on"}); //23
		references.Add(new string[]{"(","("}); //24
		references.Add(new string[]{" pts)"," pts)"}); //25
		references.Add(new string[]{"prénom : ","First name: "}); //26
		references.Add(new string[]{"nom : ","Surname: "}); //27
		references.Add(new string[]{"adresse mail : ","mail address: "}); //28
		references.Add(new string[]{"Le mot de passe doit comporter au moins 5 caractères","Password must be made of at least 3 characters"}); //29
		references.Add(new string[]{"Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .","Password cannot be made of special characters except for @ _ ."}); //30
		references.Add(new string[]{"Veuillez saisir un mot de passe","Please fill your password"}); //31
		references.Add(new string[]{"Veuillez confirmer votre mot de passe","Confirm your password"}); //32
		references.Add(new string[]{"Les deux mots de passes doivent être identiques","Passwords must be identical"}); //33
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux","You cannot use special characters"}); //34
		references.Add(new string[]{"Veuillez saisir une adresse email valide","Please type a valid mail address"}); //35
		references.Add(new string[]{"Le nom recherché doit comporter au moins 2 caractères","The name you are searching is too short (less than 2 characters)"}); //36
		references.Add(new string[]{"Les prénoms et noms doivent comporter moins de 20 caractères","First name and surname are too long (more than 20 characters)"}); //37
		references.Add(new string[]{"L'adresse mail est trop longue (plus de 40 caractères)","Email address is too long (more than 40 characters)"}); //38
		references.Add(new string[]{"Modifier le mot de passe",""}); //39
		references.Add(new string[]{"Appuyez ici pour changer votre mot de passe",""}); //40
		references.Add(new string[]{"Changer vos informations",""}); //41
		references.Add(new string[]{"Appuyez ici pour changer vos informations personnelles",""}); //42
		references.Add(new string[]{"Changer de langue",""}); //43
		references.Add(new string[]{"Appuyez ici pour changer de langue d'affichage",""}); //44
		references.Add(new string[]{"Déconnecter",""}); //45
		references.Add(new string[]{"Appuyez ici pour vous déconnectez ou bien quitter le jeu",""}); //46
		references.Add(new string[]{"Réglages du son",""}); //47
		references.Add(new string[]{"Appuyez ici pour vous ajuster la musique et les effets sonores",""}); //48
	}
}