using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMyGameTutorial
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
	static WordingMyGameTutorial()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Mon armée",""}); //0
		tutorialContents.Add(new string[]{"La meilleure manière de s'enrichir à Cristalia est de combattre d'autres colons ! Vous constituer une armée puissante sera donc cruciale pour survivre sur la planète. Vous pouvez ici consulter vos unités, et les organiser en équipes pretes à combattre pour acquérir de nouvelles ressources",""}); //1
		tutorialContents.Add(new string[]{"Créer une équipe",""}); //2
		tutorialContents.Add(new string[]{"A cause de massacres fréquents menaçant la survie de l'espèce humaine sur Cristalia, les combats sont depuis peu règlementés et opposent des équipes de 4 unités. Créer votre équipe en choisissant vos meilleures unités!",""}); //3

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Mes équipes",""}); //0
		helpContents.Add(new string[]{"Organisez vos unités en équipes de 4 pretes à combattre. N'oubliez jamais que l'ordre des unités dans l'équipe se retrouve également dans l'ordre de jeu en combat.",""}); //1
		helpContents.Add(new string[]{"Mes unités",""}); //2
		helpContents.Add(new string[]{"Accédez à l'ensemble de vos unités, et n'hésitez pas à cliquer sur une unité pour accéder au détail de ses compétences",""}); //3
		helpContents.Add(new string[]{"Les filtres",""}); //4
		helpContents.Add(new string[]{"Plus vous posséderez d'unités, plus il sera difficile de bien toutes les connaitre. Les filtres vous permettront de trouver rapidement des unités répondant à des critères spécifiques",""}); //5
	}
}