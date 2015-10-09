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
		
		GameController.instance.addTargetTile(targetsTile[0].x, targetsTile[0].y, 1);
		GameController.instance.play();
	}
	
	public override void applyOn(){
		Tile target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		int amount ; 
		
		for(int i = 0 ; i < base.tileTargets.Count ; i++){
			target = base.tileTargets[i];	
			amount = base.skill.ManaCost;
			
			text="Piège posé";
			
			GameController.instance.addTileModifier(target, amount, ModifierType.Type_WeakeningTrap, ModifierStat.Stat_No, -1, 3, "Piège affaiblissant", "Réduit de "+amount+"% l'attaque du héros touché pendant 2 tours", "Permanent. Non visible du joueur adverse");
			GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), text, 4);
		}
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("pose un <b>piège affaiblissant</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override void activateTrap(int[] targets, int[] args){
		int amount = args[0]*GameView.instance.getCard(targets[0]).GetAttack()/100;
		GameController.instance.addCardModifier(targets[0], -1*amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 19, "AFFAIBLISSEMENT", "-"+amount+" ATK. Actif 2 tours", "Actif 2 tours");
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
