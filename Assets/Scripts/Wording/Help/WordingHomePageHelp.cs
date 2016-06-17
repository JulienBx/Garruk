using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHomePageHelp
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
	static WordingHomePageHelp()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Vous avez gagné votre première bataille sur Cristalia! D'autres joueurs vous attendent dans l'arène... N'oubliez pas de faire progresser vos unités ou d'en acheter de plus fortes!","You have won your first Cristalian fight! Many more players are willing to defy your team... Will you meet them in the arena?"}); //0
		tutorialContents.Add(new string[]{"Vous n'avez pas démérité mais ce premier combat est perdu. Recrutez d'autres unités ou entrainez votre équipe avant de retenter votre chance!","You have put on a good fight but you lost. Hire stronger units or practice with your actual team before you get back in the arena"}); //1
		tutorialContents.Add(new string[]{"Vous êtes ici sur votre tableau de bord depuis lequel vous pouvez accéder aux combats, aux nouveautés, ou aux dernières offres du centre de recrutement","Here is your dashboard where you will find access to all Cristalian services such as the arena, your newsfeed, or the market..."}); //2
		tutorialContents.Add(new string[]{"Commencez par engager des unités au centre de recrutement","Start by hiring new units"}); //3
		tutorialContents.Add(new string[]{"Accédez au mode conquête pour combattre les meilleurs joueurs et gagner plus de cristaux","Play the conquest mode to earn more cristals and fight the best players"}); //4
		tutorialContents.Add(new string[]{"Vous avez terminé le Mode Entrainement. Accédez désormais au Mode Conquête!","You have finished Practice Mode. Conquest Mode is now available!"}); //5


		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Les cristaux peuvent être gagnés dans l'arène en combattant d'autres joueurs. Les récompenses sont plus élevées dans le Mode Conquête","The best way to collect Cristals is to fight other players and get fight rewards. Conquest mode offers better rewards, especially to the strongest players."}); //1
		helpContents.Add(new string[]{"Les combats de Cristalia opposent des équipes de 4 unités. Vous pouvez préparer plusieurs équipes et choisir celle que vous souhaitez utiliser pour combattre. L'ordre des unités dans l'équipe détermine l'ordre dans lequel elles agiront pendant la bataille, ne l'oubliez jamais!","Cristalian fight involve 4-units teams. You can set many teams and choose the one you want to fight with before entering the arena. Don't forget that your units order in the team determines their order during the fight"}); //3
		helpContents.Add(new string[]{"Le centre de recrutement vous propose de nouvelles offres chaque semaine, affichées ici!","Cristalian store launches new offers every week. They will automatically be displayed to your dashboard"}); //5
		helpContents.Add(new string[]{"L'alliance des colons de Cristalia (la fameuse ACC) propose à tous les nouveaux arrivants un réseau social pour rester informés des nouveautés et communiquer avec les quelques colons amicaux de la planète...","The COCA (Colonizators Of Cristalia Association) provides you a newsfeed with the most recent Cristalian information. You can also use it to make friends and keep your loneliness at bay"}); //7
	}
}