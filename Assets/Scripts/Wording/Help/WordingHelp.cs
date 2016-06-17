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
		references.Add(new string[]{"Quitter le tutoriel","Quit tutorial"}); //0
		references.Add(new string[]{"Quitter l'aide","Quit help"}); //1
		references.Add(new string[]{"Terminez le tutoriel \navant de pouvoir réaliser cette action!","Please finish tutorial before doing this!"}); //2
		references.Add(new string[]{"Continuer","Next"}); //3

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"La faction détermine les compétences qui peuvent être utilisées par l'unité, ainsi que sa capacité de mouvement sur le champ de bataille","Faction determines which skills can be used by the unit. Movement range also depends on the faction"}); //0
		helpContents.Add(new string[]{"Les unités gagnent de l'expérience au fil des combats. Chaque niveau d'expérience vous permet d'améliorer une stat (ATK ou PV) ou une compétence de votre unité","Your units can earn experience points during fights. Then you will be able to strenghten their skills or unlock new ones"}); //1
		helpContents.Add(new string[]{"L'attaque (ATK) et les points de vie (PV) sont les deux stats de chaque unité. Si votre unité attaque, elle inflige un nombre de dégats égal à son ATK sur les PV de l'unité ciblée","Attack (ATK) and Health Points (HP) are the two most important stats of your unit. When your unit attacks, it inflicts basic damages equal to its ATK to the ennemy HP"}); //2
		helpContents.Add(new string[]{"Une unité possède de 2 à 4 compétences, La compétence passive (plus sombre) donne des bonus permanents pendant le combat. Vous pourrez déclencher les compétences actives pendant le tour de l'unité","Depending on their experience level, units can use up to 4 skills. Every unit has a unique passive skill (permanent bonus during fights), and 1 to 3 active skills that you can use during fights"}); //3
		helpContents.Add(new string[]{"Cliquer sur une compétence vous permet d'accéder au détail de ses effets. Plus le niveau d'une compétence est élevé, plus les effets sont grands","Skill details can be accessed by clicking on it. You will find full list of effects depending on the skill level"}); //4
		helpContents.Add(new string[]{"Votre unité est montée au niveau supérieur! Choisissez la stat OU la compétence à améliorer ! Aux niveaux 4 et 8, l'unité apprend une nouvelle compétence","Your unit has just reached a new experience level. Choose the stat or skill to increase! Reaching level 4 or 8 will unlock a new skill."}); //5
	}
}