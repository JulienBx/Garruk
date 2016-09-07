using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingGame
{
	public static IList<string[]> texts;

	public static string getText(int idReference)
	{
		return texts[idReference][ApplicationModel.player.IdLanguage];
	}

	public static string getText(int id, List<int> args){
		string text = texts[id][ApplicationModel.player.IdLanguage];
		for(int i = 0 ; i < args.Count ; i++){
			text = text.Replace("ARG"+(i+1), ""+args[i]);
		}

		return text ;
	}

	static WordingGame()
	{
		texts = new List<string[]>();

		texts.Add(new string[]{"Bienvenue dans le simulateur de combat Alpha-B49 ! Mon nom est Mudo et je serai votre guide pour cette première bataille.","Welcome to Alpha Fight Simulator AB49! My name is Mudo and I will guide you throught this first cristalian battle."});
		texts.Add(new string[]{"Leader","Leader"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"Renforce les alliés","Strengthens your allies"});
		texts.Add(new string[]{"Protecteur","Protector"});
		texts.Add(new string[]{"Protège les alliés adjacents","Protects adjacent allies"});
		texts.Add(new string[]{"Déplacement en cours","A unit is currently moving"});
		texts.Add(new string[]{"Golem","Golem"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Poison","Poison"});

		texts.Add(new string[]{"Soin sans effet","Healing\nNo effects"});
		texts.Add(new string[]{"+ARG1 PV","+ARG1 HP"});
		texts.Add(new string[]{"Caserne","Barracks"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		texts.Add(new string[]{"Infirmière","Nurse"});
		texts.Add(new string[]{"Frénétique","BloodThirsty"});
		texts.Add(new string[]{"Fatalité","Fatality"});
		texts.Add(new string[]{"Météorites!","Meteors!"});
		texts.Add(new string[]{"A votre tour de jouer!","Your turn begins!"});
		texts.Add(new string[]{"Tour de votre adversaire!","Your enemy plays!"});

		texts.Add(new string[]{"Purifié","Purified!"});
		texts.Add(new string[]{"Echec purification","Purify failed!"});
		texts.Add(new string[]{"Mutant","Mutant"});
		texts.Add(new string[]{"Se transforme!","Transforms"});
		texts.Add(new string[]{"Attaque","Attack"});
		texts.Add(new string[]{"Aucun ennemi à portée de lance","You can not attack any unit with the spear"});
		texts.Add(new string[]{"Aucun ennemi à proximité","There is no unit next to you"});
		texts.Add(new string[]{"Aucun ennemi à proximité de cristoides alliés","There is no unit next to your cristoids"});
		texts.Add(new string[]{"Aucun ennemi à proximité de droides alliés","There is no unit next to your droids"});
		texts.Add(new string[]{"Aucun cristal à proximité","There is no cristal next to your unit"});

		texts.Add(new string[]{"Aucun terrain adjacent ne peut être ciblé","There is no free tile next to your unit"});
		texts.Add(new string[]{"Aucun allié à proximité","There is no ally next to your unit"});
		texts.Add(new string[]{"Aucun ennemi ne peut être ciblé","No enemy can be targeted"});
		texts.Add(new string[]{"Aucune unité ne peut être ciblée","No unit can be targeted"});
		texts.Add(new string[]{"Aucun allié ne peut être ciblé","No ally can be targeted"});
		texts.Add(new string[]{"Aucune allié n'est blessé","No allys are wounded"});
		texts.Add(new string[]{"Aucun allié n'est mort","No allys are dead"});
		texts.Add(new string[]{"Perd les bonus leaders","Loses leader bonus"});
		texts.Add(new string[]{"Sanguinaire","BloodThirsty"});
		texts.Add(new string[]{"Dégats à distance +ARG1%","+ARG1% to distant damages"});

		texts.Add(new string[]{"Pas d'ennemis à proximité de cristaux","No enemies next to cristals"});
		texts.Add(new string[]{"Compétence en cours","A skill is running"});
		texts.Add(new string[]{"Lancer","Launch"});
		texts.Add(new string[]{"Météorites\n-ARG1 PV","Meteors\n-ARG1 HP"});
		texts.Add(new string[]{"Cristomaitre","Cristomaster"});
		texts.Add(new string[]{"Le terrain est constitué de cases sur lesquelles les unités s'affrontent!","The battlefield is made of tiles. Units can move on these tiles to hit their opponents."});
		texts.Add(new string[]{"Certaines cases sont spéciales, par exemple les cristaux. Cliquez sur un cristal pour en apprendre plus.","There are some special tiles such as cristals. Click on a cristal to learn more about it."});
		texts.Add(new string[]{"Voici vos 4 unités!. A droite les points de vie (PV) déterminent leur résistance. Une unité n'ayant plus de PV quitte le combat!","Here are your 4 units. To the right, health points (HP) determines the unit's strength. When HP reach 0, the unit quits the fight."});
		texts.Add(new string[]{"Avant le début de la bataille, vous pouvez positionner vos unités sur les deux premières rangées du terrain. Déplacez une unité pour continuer!","Before the fight begins, you can move your units on the battlefield. Now move 1 of your units!"});
		texts.Add(new string[]{"Bravo ! Positionnez le reste de vos unités et démarrez le combat en cliquant sur le bouton sous le terrain.","Congratulations! Now you can move your other units and start the fight by clicking on the button below the battlefield."});

		texts.Add(new string[]{"Votre ennemi a positionné ses unités, la bataille peut démarrer. Vous êtes le premier joueur à avoir placé ses troupes, vous êtes donc le premier à jouer!","Your enemy has also moved his units. You have been the first to click on the START button, therefore you will be the first one to play!"});
		texts.Add(new string[]{"L'ennemi possède une unité de type LEADER, vérifions en touchant son personnage.","The enemy owns a LEADER type unit. Let's touch the character to verify the unit's identity."});
		texts.Add(new string[]{"Il s'agit bien d'un LEADER! Vous pouvez consulter ses compétences sur ici.","Indeed the unit is a LEADER. You can read more about his skills!"});
		texts.Add(new string[]{"Votre unité clignote, c'est à son tour de jouer. Les cases bleues représentent sa zone de mouvement. Commencez par la déplacer près de l'ennemi. ","Your unit is flashing. This means that it's its turn to play. Blue tiles determines your unit's movement range. Move it next to your enemy."});
		texts.Add(new string[]{"Les unités peuvent attaquer les ennemis adjacents (mais pas en diagonale). Attaquez le LEADER en déplacant le bouton d'attaque sur lui.","Units can attack adjacent enemies (diagonal attack is not allowed). Move the attack button on the LEADER to hit him."});
		texts.Add(new string[]{"Les unités peuvent attaquer les ennemis adjacents (mais pas en diagonale). Cliquez sur le bouton ATTAQUE puis ciblez le LEADER ennemi","Units can attack adjacent enemies (diagonal attack is not allowed). Click on the ATTACK button then target the enemy LEADER"});
		texts.Add(new string[]{"Les effets de la compétence s'affichent sur le terrain. Anéantir le LEADER a permis d'affaiblir les unités adverses!","Skill effects are displayed on the battlefield. Destroying the leader has weakened the enemy units."});
		texts.Add(new string[]{"A chaque tour une unité se déplace et utilise une compétence. Les unités jouent chacune à leur tour, selon l'ordre établi dans la timeline.","Unit can move once and use one skill per turn. Units play after each other, play order is displayed in the timeline."});
		texts.Add(new string[]{"C'est maintenant le tour de votre ennemi! A gauche sur la carte les points d'attaque (ATK) déterminent le nombre de points enlevés à chaque attaque.", "Now begins your enemy's turn. to the left of the card, attack power (ATK) determines how many health points are removed to the target when it's attacked."});
		texts.Add(new string[]{"Apprenons maintenant à utiliser les compétences de vos unités. Votre unité active, un MEDIC, peut soigner les unités blessées.", "Let's learn how to use your unit's skills. Your active unit, a MEDIC, can use a skill to heal wounded allys."});

		texts.Add(new string[]{"Choisissez la compétence PISTOSOIN et soignez votre unité blessée!. Chaque unité peut utiliser une compétence par tour ou attaquer.", "Choose HEALSHOT skill and heal your wounded unit. Each unit can use 1 skill or attack during its turn."});
		texts.Add(new string[]{"Félicitations, Votre unité est soignée ! Vous pouvez déplacer votre unité ou terminer directement votre tour en cliquant sur le bouton TERMINER.", "Congratulations, you have healed your unit. You can still move your unit or end your turn by clicking on END OF TURN."});
		texts.Add(new string[]{"Pour éviter que les combats ne durent trop longtemps, des météorites s'abattent fréquemment sur les côtés du champ de bataille.", "To avoid long fights, meteors often fall on the sides of the battlefield."});
		texts.Add(new string[]{"La timeline affiche les météorites, ainsi que le nombre de rangées qu'elles toucheront de chaque coté du terrain.", "The timeline displays meteor events. The number above the picture determines how many lines will be hit at each side of the battlefield by the meteors."});
		texts.Add(new string[]{"Vous possédez maintenant toutes les armes pour terminer ce combat. Bonne chance et à bientôt!", "Now you hold the keys to win this fight. Good luck and see you later!"});
		texts.Add(new string[]{"Passer","End my turn"});
		texts.Add(new string[]{"Connexion internet perdue... Essaye de se reconnecter","Lost internet connnection... Trying to reconnect..."});
		texts.Add(new string[]{"Démarrer le combat!","Start the fight!"});
		texts.Add(new string[]{"Case hors de portée!","You can not move on this tile!"});
		texts.Add(new string[]{"La case est occupée","Tile is already taken!"});

		texts.Add(new string[]{"En attente de votre adversaire!","Your enemy is moving its troops!"});
		texts.Add(new string[]{"L'unité a déjà utilisé une compétence à ce tour","The unit has already used a skill during this turn"});
		texts.Add(new string[]{"Pas d'adversaire adjacent!","No adjacent opponent unit!"});
		texts.Add(new string[]{"Cette case ne peut être ciblée","This tile can't be targeted"});
		texts.Add(new string[]{"Cette unité ne peut être ciblée","This unit can't be targeted"});
		texts.Add(new string[]{"échoue!","fails"});
		texts.Add(new string[]{"Esquive","Dodges"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2","HP : ARG1 -> ARG2"});
		texts.Add(new string[]{"Victoire!","You won!"});

		texts.Add(new string[]{"Défaite!","You lost!"});
		texts.Add(new string[]{"BONUS ATK","ATK BONUS"});
	}
}