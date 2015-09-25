using UnityEngine;
using System.Collections.Generic;

public class PiegeAffaiblissant : GameSkill
{
	public PiegeAffaiblissant(){
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
//	
//	public override void applyOn(int[] targets){
//		int amount = base.skill.ManaCost;
//		
//		GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_WeakeningTrap, ModifierStat.Stat_No, -1, 3, "Piège affaiblissant", "Réduit de "+amount+"% l'attaque du héros touché pendant 2 tours", "Permanent. Non visible du joueur adverse");
//		GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "Piège posé", 4);
//	}
	
	public override void activateTrap(int[] targets, int[] args){
		int amount = args[0]*GameView.instance.getCard(targets[0]).GetAttack()/100;
		GameController.instance.addCardModifier(targets[0], -1*amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 5, "Parapiège", "Attaque diminuée de "+amount, "Actif 2 tours");
		GameView.instance.displaySkillEffect(targets[0], "PIEGE\n-"+amount+" ATK", 5);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentTileTargets();
	}
	
	public override string getTargetText(int i, Card c){
		
		int amount = base.skill.ManaCost;
		string s = "Pose un piège\n-"+amount+"% ATK";
		
		return s ;
	}
}
