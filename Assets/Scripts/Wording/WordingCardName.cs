using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCardName
{
	public static IList<string[]> names;
	private static IList<int> idSkills;

	public static string getName(int idSkill)
	{
		return names[idSkills.IndexOf(idSkill)][ApplicationModel.player.IdLanguage];
	}
	static WordingCardName()
	{
		names = new List<string[]>();
		idSkills=new List<int>();


		//2 - PistoSoin
		idSkills.Add(2);
		names.Add(new string[]{"PistoSoin",""}); 

		//3 - Fortifiant
		idSkills.Add(3);
		names.Add(new string[]{"Fortifiant",""}); 

		//4 - Relaxant
		idSkills.Add(4);
		names.Add(new string[]{"Relaxant",""}); 

		//5 - PistoLest
		idSkills.Add(5);
		names.Add(new string[]{"PistoLest",""}); 

		//6 - Adrénaline
		idSkills.Add(6);
		names.Add(new string[]{"Adrénaline",""}); 

		//7 - Antibiotique
		idSkills.Add(7);
		names.Add(new string[]{"Antibiotique",""}); 
		 
		//8 - Pistolero
		idSkills.Add(8);
		names.Add(new string[]{"Pistolero",""}); 

		//9 - Furtivité
		idSkills.Add(9);
		names.Add(new string[]{"Furtivité",""}); 

		//10 - Assassinat
		idSkills.Add(10);
		names.Add(new string[]{"Assassinat",""}); 

		//11 - Estoc
		idSkills.Add(11);
		names.Add(new string[]{"Estoc",""}); 

		//12 - Combo
		idSkills.Add(12);
		names.Add(new string[]{"Combo",""}); 

		//13 - Electropiège
		idSkills.Add(13);
		names.Add(new string[]{"Electropiège",""}); 

		//14 - Agilite
		idSkills.Add(14);
		names.Add(new string[]{"Agilite",""}); 

		//15 - Coupejambe
		idSkills.Add(15);
		names.Add(new string[]{"Coupejambe",""}); 

		//16 - Berserk
		idSkills.Add(16);
		names.Add(new string[]{"Berserk",""}); 

		//17 - Attaque 360
		idSkills.Add(17);
		names.Add(new string[]{"Attaque 360",""}); 

		//18 - Frenesie
		idSkills.Add(18);
		names.Add(new string[]{"Frenesie",""}); 

		//19 - Cri de rage
		idSkills.Add(19);
		names.Add(new string[]{"Cri de rage",""}); 

		//20 - Terreur
		idSkills.Add(20);
		names.Add(new string[]{"Terreur",""}); 

		//21 - Cannibale
		idSkills.Add(21);
		names.Add(new string[]{"Cannibale",""}); 

		//22 - Laser
		idSkills.Add(22);
		names.Add(new string[]{"Laser",""}); 
		 
		//23 - Grenade
		idSkills.Add(23);
		names.Add(new string[]{"Grenade",""}); 

		//24 - Bombardier
		idSkills.Add(24);
		names.Add(new string[]{"Bombardier",""}); 

		//25 - Visée
		idSkills.Add(25);
		names.Add(new string[]{"Visée",""}); 

		//26 - Gros calibre
		idSkills.Add(26);
		names.Add(new string[]{"Gros calibre",""}); 

		//27 - Lanceflammes
		idSkills.Add(27);
		names.Add(new string[]{"Lanceflammes",""}); 

		//28 - Implosion
		idSkills.Add(28);
		names.Add(new string[]{"Implosion",""}); 

		//29 - Protection
		idSkills.Add(29);
		names.Add(new string[]{"Protection",""}); 

		//30 - Mitraillette
		idSkills.Add(30);
		names.Add(new string[]{"Mitraillette",""}); 

		//31 - PerfoTir
		idSkills.Add(31);
		names.Add(new string[]{"PerfoTir",""}); 

		//32 - Embusqué
		idSkills.Add(32);
		names.Add(new string[]{"Embusqué",""}); 

		//33 - Fou
		idSkills.Add(33);
		names.Add(new string[]{"Fou",""}); 

		//34 - Sanguinaire
		idSkills.Add(34);
		names.Add(new string[]{"Sanguinaire",""}); 

		//35 - Sniper
		idSkills.Add(35);
		names.Add(new string[]{"Sniper",""}); 

		//36 - Réparation
		idSkills.Add(36);
		names.Add(new string[]{"Réparation",""}); 

		//37 - Virus
		idSkills.Add(37);
		names.Add(new string[]{"Virus",""}); 

		//38 - Tourelle
		idSkills.Add(38);
		names.Add(new string[]{"Tourelle",""}); 

		//39 - Renfoderme
		idSkills.Add(39);
		names.Add(new string[]{"Renfoderme",""}); 

		//40 - Temple sacre
		idSkills.Add(40);
		names.Add(new string[]{"Temple sacre",""}); 

		//41 - Forestier
		idSkills.Add(41);
		names.Add(new string[]{"Forestier",""}); 

		//42 - Sable maudit
		idSkills.Add(42);
		names.Add(new string[]{"Sable maudit",""}); 

		//43 - Caméléon
		idSkills.Add(43);
		names.Add(new string[]{"Caméléon",""}); 

		//44 - Fontaine
		idSkills.Add(44);
		names.Add(new string[]{"Fontaine",""}); 

		//45 - Loup sauvage
		idSkills.Add(45);
		names.Add(new string[]{"Loup sauvage",""}); 
		 
		//46 - Resurrection
		idSkills.Add(46);
		names.Add(new string[]{"Resurrection",""}); 

		//47 - Blindé
		idSkills.Add(47);
		names.Add(new string[]{"Blindé",""}); 

		//48 - Drone
		idSkills.Add(48);
		names.Add(new string[]{"Drone",""}); 

		//49 - Pacifista
		idSkills.Add(49);
		names.Add(new string[]{"Pacifista",""}); 

		//50 - Trouble
		idSkills.Add(50);
		names.Add(new string[]{"Trouble",""}); 

		//51 - Etouffement
		idSkills.Add(51);
		names.Add(new string[]{"Etouffement",""}); 

		//52 - Illusion
		idSkills.Add(52);
		names.Add(new string[]{"Illusion",""}); 

		//53 - Hacking
		idSkills.Add(53);
		names.Add(new string[]{"Hacking",""}); 

		//54 - Manipulation
		idSkills.Add(54);
		names.Add(new string[]{"Manipulation",""}); 

		//55 - Confusion
		idSkills.Add(55);
		names.Add(new string[]{"Confusion",""}); 

		//56 - Stéroide
		idSkills.Add(56);
		names.Add(new string[]{"Stéroide",""}); 

		//57 - Sénilité
		idSkills.Add(57);
		names.Add(new string[]{"Sénilité",""}); 

		//58 - Poisonpiège
		idSkills.Add(58);
		names.Add(new string[]{"Poisonpiège",""}); 

		//59 - Coup précis
		idSkills.Add(59);
		names.Add(new string[]{"Coup précis",""}); 
		 
		//60 - Copiage
		idSkills.Add(60);
		names.Add(new string[]{"Copiage",""}); 
		 
		//61 - Hypnose
		idSkills.Add(61);
		names.Add(new string[]{"Hypnose",""}); 

		//63 - Massue
		idSkills.Add(63);
		names.Add(new string[]{"Massue",""}); 

		//64 - Piégeur
		idSkills.Add(64);
		names.Add(new string[]{"Piégeur",""}); 
		 
		//65 - Lâche
		idSkills.Add(65);
		names.Add(new string[]{"Lâche",""}); 

		//66 - Agile
		idSkills.Add(66);
		names.Add(new string[]{"Agile",""}); 
		 
		//67 - Ninja
		idSkills.Add(67);
		names.Add(new string[]{"Ninja",""}); 
		 
		//68 - Aguerri
		idSkills.Add(68);
		names.Add(new string[]{"Monstrueux",""}); 
		  
		//69 - Frénétique
		idSkills.Add(69);
		names.Add(new string[]{"Frénétique",""}); 
		  
		//70 - Cuirassé
		idSkills.Add(70);
		names.Add(new string[]{"Cuirassé",""}); 
		 
		//71 - Rapide
		idSkills.Add(71);
		names.Add(new string[]{"Rapide",""}); 

		//72 - Virologue
		idSkills.Add(72);
		names.Add(new string[]{"Virologue",""}); 

		//73 - Paladin
		idSkills.Add(73);
		names.Add(new string[]{"Paladin",""}); 

		//74 - Maladroit
		idSkills.Add(74);
		names.Add(new string[]{"Maladroit",""}); 

		//75 - Infirmier
		idSkills.Add(75);
		names.Add(new string[]{"Infirmier",""}); 

		//76 - Leader
		idSkills.Add(76);
		names.Add(new string[]{"Leader",""}); 

		//77 - Fou
		idSkills.Add(77);
		names.Add(new string[]{"Fou",""}); 

		//78 - Joueur
		idSkills.Add(78);
		names.Add(new string[]{"Joueur",""}); 
		 
		//79 - Copycat
		idSkills.Add(79);
		names.Add(new string[]{"Copycat",""}); 

		//80 - Vampire
		idSkills.Add(80);
		names.Add(new string[]{"Vampire",""}); 

		//81 - Gundam
		idSkills.Add(81);
		names.Add(new string[]{"Gundam",""}); 

		//82 - Labyrinthe
		idSkills.Add(82);
		names.Add(new string[]{"Labyrinthe",""}); 

		//83 - Cristo-Blast
		idSkills.Add(83);
		names.Add(new string[]{"Cristo-Blast",""}); 
		 
		//84 - Séisme
		idSkills.Add(84);
		names.Add(new string[]{"Séisme",""}); 
		 
		//85 - Tectonique
		idSkills.Add(85);
		names.Add(new string[]{"Tectonique",""}); 
		 
		//86 - Tunnel
		idSkills.Add(86);
		names.Add(new string[]{"Tunnel",""}); 
		 
		//87 - Blitz
		idSkills.Add(87);
		names.Add(new string[]{"Blitz",""}); 
		  
		//88 - Terraformeur
		idSkills.Add(88);
		names.Add(new string[]{"Terraformeur",""}); 

		//89 - Enraciner
		idSkills.Add(89);
		names.Add(new string[]{"Enraciner",""}); 
		 
		//91 - Lance
		idSkills.Add(91);
		names.Add(new string[]{"Lance",""}); 

		//92 - Deséquilibre
		idSkills.Add(92);
		names.Add(new string[]{"Deséquilibre",""}); 

		//93 - Furie
		idSkills.Add(93);
		names.Add(new string[]{"Furie",""}); 
		  
		//94 - Poison
		idSkills.Add(94);
		names.Add(new string[]{"Poison",""}); 
		 
		//95 - Division
		idSkills.Add(95);
		names.Add(new string[]{"Division",""}); 

		//96 - Cristalien
		idSkills.Add(96);
		names.Add(new string[]{"Cristalien",""}); 

		//97 - Tacticien
		idSkills.Add(97);
		names.Add(new string[]{"Tacticien",""}); 

		//98 - Cristophile
		idSkills.Add(98);
		names.Add(new string[]{"Cristophile",""}); 
		  
		//99 - Furtif
		idSkills.Add(99);
		names.Add(new string[]{"Furtif",""}); 

		//100 - Renaissance
		idSkills.Add(100);
		names.Add(new string[]{"Renaissance",""}); 

		//101 - Fatalité
		idSkills.Add(101);
		names.Add(new string[]{"Fatalité",""}); 

		//102 - Sermon
		idSkills.Add(102);
		names.Add(new string[]{"Sermon",""}); 
		 
		//103 - Bénédiction
		idSkills.Add(103);
		names.Add(new string[]{"Bénédiction",""}); 

		//105 - Sacrifice
		idSkills.Add(105);
		names.Add(new string[]{"Sacrifice",""}); 

		//106 - Malédiction
		idSkills.Add(106);
		names.Add(new string[]{"Malédiction",""}); 
		  
		//107 - Miracle
		idSkills.Add(107);
		names.Add(new string[]{"Miracle",""}); 

		//110 - Cristomaitre
		idSkills.Add(110);
		names.Add(new string[]{"Cristomaitre",""}); 
		 
		//111 - Apotre
		idSkills.Add(111);
		names.Add(new string[]{"Apotre",""}); 

		//112 - Fanatique
		idSkills.Add(112);
		names.Add(new string[]{"Fanatique",""}); 

		//113 - Purificateur
		idSkills.Add(113);
		names.Add(new string[]{"Purificateur",""}); 

		//114 - Géant
		idSkills.Add(114);
		names.Add(new string[]{"Géant",""}); 

		//115 - Loup
		idSkills.Add(115);
		names.Add(new string[]{"Loup",""}); 

		//116 - Infirmerie
		idSkills.Add(116);
		names.Add(new string[]{"Infirmerie",""}); 
		 
		//117 - Arène
		idSkills.Add(117);
		names.Add(new string[]{"Arène",""}); 
		  
		//118 - Abri
		idSkills.Add(118);
		names.Add(new string[]{"Abri",""}); 

		//119 - Armure
		idSkills.Add(119);
		names.Add(new string[]{"Armure",""}); 

		//124 - Sculpteur
		idSkills.Add(124);
		names.Add(new string[]{"Sculpteur",""}); 
		 
		//125 - Génie
		idSkills.Add(125);
		names.Add(new string[]{"Génie",""}); 

		//126 - Roboticien
		idSkills.Add(126);
		names.Add(new string[]{"Roboticien",""}); 
		  
		//127 - Armurier
		idSkills.Add(127);
		names.Add(new string[]{"Armurier",""}); 

		//128 - Cristo Power
		idSkills.Add(128);
		names.Add(new string[]{"Cristo Power",""}); 

		//129 - Cristo Life
		idSkills.Add(129);
		names.Add(new string[]{"Cristo Life",""}); 

		//130 - Pluie Bleue
		idSkills.Add(130);
		names.Add(new string[]{"Pluie Bleue",""}); 

		//131 - Chasseur
		idSkills.Add(131);
		names.Add(new string[]{"Chasseur",""}); 

		//132 - Choc bleu
		idSkills.Add(132);
		names.Add(new string[]{"Choc bleu",""}); 

		//133 - Synergie
		idSkills.Add(133);
		names.Add(new string[]{"Synergie",""}); 

		//138 - Mutant
		idSkills.Add(138);
		names.Add(new string[]{"Mutant",""}); 

		//139 - Symbiote
		idSkills.Add(139);
		names.Add(new string[]{"Symbiote",""}); 

		//140 - Alchimiste
		idSkills.Add(140);
		names.Add(new string[]{"Alchimiste",""}); 
		 
		//141 - Human Killer
		idSkills.Add(141);
		names.Add(new string[]{"Human Killer",""}); 

		//143 - Arme
		idSkills.Add(143);
		names.Add(new string[]{"Arme",""}); 

	}
}