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
		helpContents.Add(new string[]{"Chaque joueur dispose d'un profil sur Cristalia. Ce profil est votre carte d'identité Cristalienne accessible de tous sur la Cristalosphère.",""}); //0
		helpContents.Add(new string[]{"Le département du mérite Cristalien aime décerner des trophées, récompensant votre aptitude au combat ou votre niveau de collection des unités. Votre vitrine des trophés affiche les récompenses gagnées depuis votre arrivée sur la planète.",""}); //1
		helpContents.Add(new string[]{"Recherchez ici un joueur en tapant les premières lettres de son pseudo!",""}); //2
		helpContents.Add(new string[]{"Vous pourrez défier vos amis dans l'arène, et ainsi rompre avec la solitude du combattant, mal fréquent chez les colons fraichement débarqués. Attention, les combats avec les amis ne rapportent pas de cristaux",""}); //3

		helpContents.Add(new string[]{"Vous pouvez accéder ici au profil d'un colon, et le demander en ami. Vous pourrez ensuite le défier dans l'arène",""}); //4
		helpContents.Add(new string[]{"La liste des derniers combats du colon dans l'arène, pour consulter l'état de forme de ses équipes",""}); //5
		helpContents.Add(new string[]{"Consultez ici la liste des amis du colon",""}); //6

	}
}