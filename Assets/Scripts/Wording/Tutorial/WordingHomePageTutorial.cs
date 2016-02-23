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
		tutorialContents.Add(new string[]{"Vous avez gagné votre première bataille sur Cristalia. D'autres colons vous attendent  ",""}); //1
		tutorialContents.Add(new string[]{"Défaite !",""}); //2
		tutorialContents.Add(new string[]{"Cettte première bataille  ! Continuez à entrainer vos troupes pour les préparer aux combats qui les attendent !",""}); //3
		tutorialContents.Add(new string[]{"Aidez-moi !",""}); //4
		tutorialContents.Add(new string[]{"Quelque soit l'endroit ou vous vous trouvez sur Cristalia, je reste à votre disposition si vous avez une question ou que vous vous sentez perdus ! Cliquez sur l'aide pour me convoquer",""}); //5


		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Combattre",""}); //0
		helpContents.Add(new string[]{"La meilleure manière de s'enrichir reste de combattre d'autres colons ! Choisissez soigneusement votre combat et vos unités pour affronter d'autres colons et piller leur cristal",""}); //1
		helpContents.Add(new string[]{"Mon équipe",""}); //2
		helpContents.Add(new string[]{"Les combats de Cristalia opposent des équipes de 4 joueurs. Vous pouvez ici constituer une ou plusieurs équipes. L'ordre des unités dans l'équipe détermine l'ordre dans lequel elle agiront en combat, donc ne mettez pas vos meilleures unités en dernier !",""}); //3
		helpContents.Add(new string[]{"Recruter des unités",""}); //4
		helpContents.Add(new string[]{"Vous pourrez trouver ici les dernières promotions du centre de recrutement pour renforcer vos équipes. De nouvelles offres apparaissent régulièrement !",""}); //5
		helpContents.Add(new string[]{"Mon tableau de bord",""}); //6
		helpContents.Add(new string[]{"Ce tableau de bord offert à tous les colons fraichement débarqués sur Cristalia permet d'accéder aux actualités de la planète, et de communiquer avec d'autres colons",""}); //7


	}
}