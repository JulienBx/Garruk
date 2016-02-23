using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingStoreTutorial
{
	public static IList<string[]> tutorialContents;
	public static IList<string[]> helpContents;

	public static string getTutorialContent(int idReference)
	{
		return tutorialContents[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingStoreTutorial()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Centre de recrutement",""}); //0
		tutorialContents.Add(new string[]{"Dès leur arrivée sur Cristalia, les colons sont emmenés au centre de recrutement. Mieux vaut en effet ne pas se promener seul sur la planète... Votre planète vous a remis une somme d'argent que vous avez pu échanger contre la monnaie locale (le Cristal, objectif de votre venue ici). Ceci devrait vous permettre de recruter quelques Cristaliens prets à se battre pour vous (ou pour votre argent, c'est selon)",""}); //1
		tutorialContents.Add(new string[]{"Vos premières unités",""}); //2
		tutorialContents.Add(new string[]{"Consultez-les attentivement. Les unités possèdent chacune une compétence passive (qui détermine leur type) et 1 ou plusieurs compétences actives (qu'elles pourront déclencher pendant une bataille). Allons maintenant préparer vos troupes pour la bataille!",""}); //3

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Les groupes",""}); //0
		helpContents.Add(new string[]{"Certains cristaliens s'entrainent ensemble depuis leur enfance et peuvent etre recrutés à des tarifs intéressants",""}); //1
		helpContents.Add(new string[]{"Acheter du Cristal",""}); //2
		helpContents.Add(new string[]{"Pour investir sur de nouvelles unités et progresser plus rapidement",""}); //3
		helpContents.Add(new string[]{"Les unités",""}); //4
		helpContents.Add(new string[]{"Voici les unités que vous avez acquises au centre de recrutement. Elles sont directement transférées vers votre armée",""}); //5
	}
}