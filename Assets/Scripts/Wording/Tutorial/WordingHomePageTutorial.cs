using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHomePageTutorial
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
	static WordingHomePageTutorial()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Bravo!","Congratulations!"}); //0
		tutorialContents.Add(new string[]{"Vous avez gagné votre première bataille sur Cristalia! Mais d'autres colons plus forts encore vous attendent... Essayez le mode conquête!",""}); //1
		tutorialContents.Add(new string[]{"Dommage !",""}); //2
		tutorialContents.Add(new string[]{"Vos unités n'ont pas démérité mais ont perdu ce premier combat. Peut-être faudra-t-il entrainer vos unités ou en recruter d'autres !",""}); //3
		tutorialContents.Add(new string[]{"Aidez-moi !",""}); //4
		tutorialContents.Add(new string[]{"Quelque soit l'endroit ou vous vous trouvez sur Cristalia, je reste à votre disposition si vous avez une question ou que vous vous sentez perdus ! Cliquez sur l'aide pour me convoquer",""}); //5

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Combattre",""}); //0
		helpContents.Add(new string[]{"La meilleure manière de s'enrichir reste de combattre d'autres colons! Le mode conquête vous permettra de gagner de plus en plus de cistal au fil de vos victoires",""}); //1
		helpContents.Add(new string[]{"Mon équipe",""}); //2
		helpContents.Add(new string[]{"Les combats de Cristalia opposent des équipes de 4 unités. Vous pouvez préparer plusieurs équipes et choisir celle que vous souhaitez pour combattre. L'ordre des unités dans l'équipe détermine l'ordre dans lequel elles agiront pendant la bataille, ne l'oubliez jamais!",""}); //3
		helpContents.Add(new string[]{"Recruter des unités",""}); //4
		helpContents.Add(new string[]{"Le centre de recrutement vous proposera ici ses meilleures offres. Chaque semaine de nouvelles offres sont proposées!",""}); //5
		helpContents.Add(new string[]{"Mon tableau de bord",""}); //6
		helpContents.Add(new string[]{"L'alliance des colons de Cristalia (la fameuse ACC) propose à tous les nouveaux arrivants un terminal social pour rester informés des nouveautés et communiquer avec les quelques colons amicaux de la planète...",""}); //7


	}
}