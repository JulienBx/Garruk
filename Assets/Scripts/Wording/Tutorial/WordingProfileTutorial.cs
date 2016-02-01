using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProfileTutorial
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.idLanguage];
	}
	static WordingProfileTutorial()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Mon profil",""}); //0
		helpContents.Add(new string[]{"Votre vie de colon sur Cristalia vous permettra de rencontrer d'autres colons. Ce profil est votre carte d'identité accessible de tous sur la Cristalosphère.",""}); //1
		helpContents.Add(new string[]{"Profil d'un colon",""}); //2
		helpContents.Add(new string[]{"Vous pouvez accéder ici au profil d'un colon, et rentrer en relation avec lui. Ce système de mise en relation vous permettra notamment de vous entrainer face à lui dans les arènes privatisées de Cristalia",""}); //3
		helpContents.Add(new string[]{"Trophées et challenges",""}); //4
		helpContents.Add(new string[]{"De nombreux trophées sillonneront votre présence sur Cristalia, récompensant votre aptitude au combat, vos connaissances du Cristal ou votre activité sur la planète",""}); //5
		helpContents.Add(new string[]{"Derniers combats",""}); //6
		helpContents.Add(new string[]{"Vous pourrez également accéder à la liste des derniers combats du colon, et consulter ainsi l'état de forme de ses équipes",""}); //7
		helpContents.Add(new string[]{"Rechercher un colon",""}); //8
		helpContents.Add(new string[]{"Recherchez ici un colon en inscrivant sur son nom!",""}); //9
		helpContents.Add(new string[]{"Mes amis",""}); //10
		helpContents.Add(new string[]{"Vous créer une vie sociale sur Cristalia vous permettra d'entrainer vos équipes à plusieurs, et de rompre avec la solitude du combattant, mal fréquent chez les colons fraichement débarqués.",""}); //11
		helpContents.Add(new string[]{"Les amis",""}); //12
		helpContents.Add(new string[]{"Vous pouvez ici consulter la liste des amis du colon",""}); //13
	}
}