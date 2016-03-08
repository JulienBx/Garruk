using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLobby
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLobby()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Statistiques","Stats"}); //0
		references.Add(new string[]{"Résultats","Results"}); //1
		references.Add(new string[]{"Récompenses","Rewards"}); //2
		references.Add(new string[]{"Victoires","Won"}); //3
		references.Add(new string[]{"Défaites","Lost"}); //4
		references.Add(new string[]{"Classement bataille","Fighter ranking"}); //5
		references.Add(new string[]{"Classement collection","Collection ranking"}); //6
		references.Add(new string[]{"OK!","OK!"}); //7
		references.Add(new string[]{"(","("}); //8
		references.Add(new string[]{" pts)"," pts)"}); //9
		references.Add(new string[]{"Victoire le ","Won, "}); //10
		references.Add(new string[]{"Défaite le ","Lost, "}); //11
		references.Add(new string[]{"Champion : ","Champion : "}); //12
		references.Add(new string[]{"\nColon : ","Settler : "}); //13
		references.Add(new string[]{" cristaux"," cristals"}); //14
		references.Add(new string[]{"Victoire : ","Win : "}); //15
		references.Add(new string[]{"Bravo, vous avez été nommé champion du satellite! Il est désormais temps de partir à la conquête du satellite suivant et de combattre des adversaires encore plus redoutables","Congratulations, you have been awarded the title of Champion by the satellite inhabitants. Now it's time to move on to the next satellite and to fight stronger challengers"}); //16
		references.Add(new string[]{"Bravo, vous avez gagné le titre de champion! Etes-vous prêt à le remettre en jeu?","Congratulations, you been awarded the title of champion. How long will you able to keep this title ?"}); //17
		references.Add(new string[]{"Bravo, cette victoire vous permet d'obtenir l'autorisation d'explorer un nouveau satellite!","Congratulations. You have been granted an authorization to explore the next Cristalian satellite"}); //18
		references.Add(new string[]{"Bravo, vous pouvez dès maintenant commencer l'exploration d'un nouveau satellite !",""}); //19
		references.Add(new string[]{"Vous pourrez prochainement explorer une nouvelle planète !","Congratulations. You have been granted an authorization to explore the next Cristalian satellite"}); //20
		references.Add(new string[]{"Bravo, le conseil de guerre Cristalien a décidé de renouveler votre accès à ce satellite sur la base de vos résultats récents","Thanks to your fighting record, Cristalian War Council agree to let you stay on this satellite"}); //21
		references.Add(new string[]{"Bravo, le conseil de guerre Cristalien a décidé de renouveler votre accès à ce satellite sur la base de vos résultats récents","Thanks to your fighting record, Cristalian War Council agree to let you stay on this satellite"}); //22
		references.Add(new string[]{"Cette victoire vous assure de garder votre place sur ce satellite!","Following this success, Cristalian War Council guarantees you that you will be authorized to stay on this satellite."}); //23
		references.Add(new string[]{"Le conseil de guerre Cristalien a décidé de vous renvoyer au satellite précédent suite à vos résultats récents.","Cristalian War Council decided that you may go back to the previous satellite due to your recent fight results"}); //24
		references.Add(new string[]{"Continuer","Next fight"}); //25
		references.Add(new string[]{"Fight","Fight"}); //26
		references.Add(new string[]{" Victoires"," Won"}); //27
		references.Add(new string[]{"Champion","Champion"}); //28
		references.Add(new string[]{"Colon","Settler"}); //29
		references.Add(new string[]{"Rester sur le satellite","Secure the access"}); //30
		references.Add(new string[]{" batailles restantes","fights left"}); //31
		references.Add(new string[]{" bataille restante","fight left"}); //32
		references.Add(new string[]{" victoires"," won"}); //33
		references.Add(new string[]{" victoire"," won"}); //34
		references.Add(new string[]{"Vous êtes champion\n","You are the champion\n"}); //35
		references.Add(new string[]{"Vous avez gagné l'accès au satellite suivant\n","You have been granted an authorization to move on to the next satellite\n"}); //36
		references.Add(new string[]{"Vous resterez sur le satellite pour la période suivante\n","You have secured the access to the satellite for the next period\n"}); //37
		references.Add(new string[]{"Essaye d'assurer son maintien sur le satellite\n","Trying to secure the access for the next period\n"}); //38
	}
}