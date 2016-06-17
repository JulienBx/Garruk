using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProfileHelp
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingProfileHelp()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Chaque joueur dispose d'un profil sur Cristalia. Ce profil est votre carte d'identité Cristalienne accessible de tous sur la Cristalosphère.","Every player has a public profile on Cristalia"}); //0
		helpContents.Add(new string[]{"Le département du mérite Cristalien aime décerner des trophées, récompensant votre aptitude au combat ou votre niveau de collection des unités. Votre vitrine des trophées affiche les récompenses gagnées depuis votre arrivée sur la planète.","Cristalian Honor Department likes to award trophies, rewards to the best fighters or collectors. Here you will find all the trophies earned since your arrival on Cristalia"}); //1
		helpContents.Add(new string[]{"Recherchez ici un joueur en tapant les premières lettres de son pseudo!","Look for a player by typing its name first letters"}); //2
		helpContents.Add(new string[]{"Vous pourrez défier vos amis dans l'arène, et ainsi rompre avec la solitude du combattant, mal fréquent chez les colons fraichement débarqués. Attention, les combats avec les amis ne rapportent pas de cristaux","You can defy your friends anytime during your stay on Cristalia. Both of you need to be on the planet simultaneously to do so. You won't get any rewards durong friend fights!"}); //3
		helpContents.Add(new string[]{"Vous pouvez accéder ici au profil d'un joueur, et le demander en ami. Vous pourrez ensuite le défier dans l'arène",""}); //4
		helpContents.Add(new string[]{"Vous pouvez consulter les derniers résultats du joueur dans l'arène","You can access to the player's last results in the arena"}); //5
		helpContents.Add(new string[]{"Consultez ici la liste des amis du joueur","Find out about the player's friends on Cristalia"}); //6
	}
}