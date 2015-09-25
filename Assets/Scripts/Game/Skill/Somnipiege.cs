using UnityEngine;
using System.Collections.Generic;

public class Somnipiege : GameSkill
{
	public Somnipiege(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentTileTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int[] targets = new int[2];
		targets[0] = targetsTile[0].x;
		targets[1] = targetsTile[0].y;
		GameController.instance.applyOn(targets);
		GameController.instance.play();
	}
	
//	public override void applyOn(int[] targets){
//		int amount = base.skill.ManaCost;
//		
//		GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_SleepingTrap, ModifierStat.Stat_No, -1, 2, "Piège endormissant", "Endort l'adversaire. "+amount+"% de chances de se réveiller chaque tour", "Permanent. Non visible du joueur adverse");
//		GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "Piège posé", 4);
//	}
	
	public override void activateTrap(int[] targets, int[] args){
		GameController.instance.addCardModifier(targets[0], args[0], ModifierType.Type_Sleeping, ModifierStat.Stat_No, -1, 12, "Endormi", "Le héros ne peut ni se déplacer ni agir", args[0]+"% réveil / tour");
		GameView.instance.displaySkillEffect(targets[0], "PIEGE\nS'endort", 5);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentTileTargets();
	}
	
	public override string getTargetText(int i, Card c){
		
		int amount = base.skill.ManaCost;
		string s = "Pose un piège\nEndort";
		
		return s ;
	}
}
