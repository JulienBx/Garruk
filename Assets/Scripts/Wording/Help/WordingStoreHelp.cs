using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingStoreHelp
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
	static WordingStoreHelp()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Bienvenue sur le centre de recrutement. Allons acheter un premier groupe d'unités.",""}); //0
		tutorialContents.Add(new string[]{"Voici vos première recrues",""}); //1
		tutorialContents.Add(new string[]{"Examinons de plus près cette recrue",""}); //2
		tutorialContents.Add(new string[]{"Le blason en haut à gauche correspond à la faction de la recrue",""}); //3
		tutorialContents.Add(new string[]{"En haut à droite vous retrouvez l'attaque et la vie de l'unité",""}); //4
		tutorialContents.Add(new string[]{"Les compétences sont situées sur le bas de la carte, la passive apparaît en premier",""}); //5
		tutorialContents.Add(new string[]{"Appuyez maintenant sur la carte",""}); //6
		tutorialContents.Add(new string[]{"Vous accédez à des informations plus détaillées",""}); //7
		tutorialContents.Add(new string[]{"Vous avez la description complète de la compétence",""}); //8
		tutorialContents.Add(new string[]{"Maintenant que vous avez vos première recrues, allons créer une équipe",""}); //9

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Certains cristaliens s'entrainent ensemble depuis leur enfance et peuvent etre recrutés à des tarifs intéressants",""}); //1
		helpContents.Add(new string[]{"Pour investir sur de nouvelles unités et progresser plus rapidement",""}); //3
		helpContents.Add(new string[]{"Voici les unités que vous avez acquises au centre de recrutement. Elles sont directement transférées vers votre armée",""}); //5
	}
}