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
		references.Add(new string[]{"Mes statistiques","Stats"}); //0
		references.Add(new string[]{"Mes derniers résultats","Results"}); //1
		references.Add(new string[]{"Panneau des primes","Rewards"}); //2
		references.Add(new string[]{"Victoires","Won"}); //3
		references.Add(new string[]{"Défaites","Lost"}); //4
		references.Add(new string[]{"Classement Combattant","Fighter ranking"}); //5
		references.Add(new string[]{"Classement Collectioneur","Collection ranking"}); //6
		references.Add(new string[]{"OK!","OK!"}); //7
		references.Add(new string[]{"(","("}); //8
		references.Add(new string[]{" pts)"," pts)"}); //9
		references.Add(new string[]{"Victoire le ","Won, "}); //10
		references.Add(new string[]{"Défaite le ","Lost, "}); //11
		references.Add(new string[]{"Champion : ","Champion : "}); //12
		references.Add(new string[]{"\nStratège : ","\nSettler : "}); //13
		references.Add(new string[]{" cristaux"," cristals"}); //14
		references.Add(new string[]{"Victoire : ","Win : "}); //15
		references.Add(new string[]{"Bravo, vous avez atteint le rang de CHAMPION! En accord avec le code de guerre Cristalien, article 4d, vous êtes désormais autorisé à accéder au satellite suivant et combattre des joueurs plus redoutables.","Congratulations, you have been awarded the title of Champion by the satellite inhabitants. Now it's time to move on to the next satellite and to fight stronger challengers"}); //16
		references.Add(new string[]{"Bravo, vous avez atteint le rang de CHAMPION sur le satellite le plus difficile de Cristalia! Prêt à remettre votre titre en jeu?","Congratulations, you been awarded the title of champion. How long will you able to keep this title ?"}); //17
		references.Add(new string[]{"Bravo, vous avez atteint le rang d'EXPERT. Ce rang vous permettra, selon le code de guerre Cristalien article 4d, d'accéder au satellite suivant ou des joueurs plus forts vous attendent","Congratulations. You have been granted an authorization to explore the next Cristalian satellite"}); //18
		references.Add(new string[]{"Bravo, vous avez atteint le rang d'EXPERT. Ce rang vous permet, selon le code de guerre Cristalien article 4d, d'accéder au satellite suivant ou des joueurs plus forts vous attendent","Congratulations. You have been granted an authorization to explore the next Cristalian satellite"}); //19
		references.Add(new string[]{"Bravo, vous avez atteint le rang d'EXPERT. Ce rang vous permettra, selon le code de guerre Cristalien article 4d, d'accéder au satellite suivant ou des joueurs plus forts vous attendent","Congratulations. You have been granted an authorization to explore the next Cristalian satellite"}); //20
		references.Add(new string[]{"Bravo, vous avez atteint le rang de DISCIPLE. Ce rang vous permettra, selon le code de guerre Cristalien article 4d, de lancer une nouvelle campagne sur ce satellite.","Thanks to your fighting record, Cristalian War Council agree to let you stay on this satellite"}); //21
		references.Add(new string[]{"Bravo, vous avez atteint le rang de DISCIPLE. Ce rang vous permet, selon le code de guerre Cristalien article 4d, de lancer une nouvelle campagne sur ce satellite.","Thanks to your fighting record, Cristalian War Council agree to let you stay on this satellite"}); //22
		references.Add(new string[]{"Cette victoire vous permet d'atteindre le rang de DISCIPLE et de rester sur ce satellite pour une nouvelle campagne (article de guerre Cristalien, article 4d)!","Following this success, Cristalian War Council guarantees you that you will be authorized to stay on this satellite."}); //23
		references.Add(new string[]{"Vous n'avez pas réussi à atteindre le rang de DISCIPLE à l'issue de cette campagne, le code de guerre Cristalien vous impose de retourner au satellite précédent affronter des joueurs moins redoutables.","Cristalian War Council decided that you may go back to the previous satellite due to your recent fight results"}); //24
		references.Add(new string[]{"Continuer","Next fight"}); //25
		references.Add(new string[]{"Combattre","Fight"}); //26
		references.Add(new string[]{" victoires"," Won"}); //27
		references.Add(new string[]{"CHAMPION","Champion"}); //28
		references.Add(new string[]{"EXPERT","Settler"}); //29
		references.Add(new string[]{"DISCIPLE","Secure the access"}); //30
		references.Add(new string[]{" combats restants","fights left"}); //31
		references.Add(new string[]{" combat restant","fight left"}); //32
		references.Add(new string[]{" victoires"," won"}); //33
		references.Add(new string[]{" victoire"," won"}); //34
		references.Add(new string[]{"Vous êtes CHAMPION\n","You are the champion\n"}); //35
		references.Add(new string[]{"Votre rang vous permet d'accéder au satellite suivant\n","You have been granted an authorization to move on to the next satellite\n"}); //36
		references.Add(new string[]{"Votre rang vous permet de rester sur le satellite pour votre prochaine campagne\n","You have secured the access to the satellite for the next period\n"}); //37
		references.Add(new string[]{"Essaye d'atteindre le rang de DISCIPLE pour pouvoir rester sur ce satellite\n","Trying to secure the access for the next period\n"}); //38
		references.Add(new string[]{"Combattre","Fight"}); //39
		references.Add(new string[]{"Continuez votre compagne en affrontant un joueur présent sur ce satellite et progresser vers le rang suivant","Fight yout next opponent to strenghten your position on the satellite"}); //40
	}
}