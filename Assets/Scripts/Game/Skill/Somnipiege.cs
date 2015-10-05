using UnityEngine;
using System.Collections.Generic;

public class Somnipiege : GameSkill
{
	public Somnipiege(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentTileTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		GameView.instance.getGC().addTargetTile(targetsTile[0].x, targetsTile[0].y, 1);
		GameView.instance.getGC().play();
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
			
			GameView.instance.getGC().addTileModifier(target, amount, ModifierType.Type_SleepingTrap, ModifierStat.Stat_No, -1, 2, "Piège endormissant", "Endort l'adversaire. "+amount+"% de chances de se réveiller chaque tour", "Permanent. Non visible du joueur adverse");
			GameView.instance.displaySkillEffect(GameView.instance.getGC().getCurrentPlayingCard(), text, 4);
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("pose un <b>somnipiège</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override void activateTrap(int[] targets, int[] args){
		GameView.instance.getGC().addCardModifier(targets[0], args[0], ModifierType.Type_Sleeping, ModifierStat.Stat_No, -1, 12, "Endormi", "Le héros ne peut ni se déplacer ni agir", args[0]+"% réveil / tour");
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
