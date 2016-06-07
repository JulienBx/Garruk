using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHelp
{
	public static IList<string[]> references;
	public static IList<string[]> helpContents;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingHelp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Quitter le tutoriel",""}); //0
		references.Add(new string[]{"Quitter l'aide",""}); //1
		references.Add(new string[]{"Terminez le tutoriel \navant de pouvoir accéder à cette action !",""}); //2
		references.Add(new string[]{"Continuer",""}); //3

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"La faction de l'unité détermine les compétences auxquelles l'unité a accès, ainsi que sa mobilité sur le champ de bataille",""}); //0
		helpContents.Add(new string[]{"Les unités peuvent progresser au fil des combats en accumulant de l'expérience. L'expérience permet à l'unité de gagner des niveaux et d'améliorer ses caractéristiques et compétences",""}); //1
		helpContents.Add(new string[]{"L'attaque et les points de vie sont les deux caractéristiques les plus importantes au combat. Les points de vie déterminent la résistance de l'unité, l'attaque sa force",""}); //2
		helpContents.Add(new string[]{"L'unité dispose de 2 à 4 compétences selon son niveau, dont une compétence passive (en noir). Les compétences actives peuvent être utilisées pendant le tour de l'unité",""}); //3
		helpContents.Add(new string[]{"Cliquer sur une compétence permet d'accéder à la liste de ses effets selon le niveau de maitrise atteint",""}); //4
		helpContents.Add(new string[]{"Votre unité est montée au niveau supérieur! Choisissez la caractéristique OU la compétence à améliorer ! Aux niveaux 4 et 8, l'unité apprend une nouvelle compétence",""}); //5
	}
}