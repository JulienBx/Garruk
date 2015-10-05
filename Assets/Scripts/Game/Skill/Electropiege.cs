using UnityEngine;
using System.Collections.Generic;

public class Electropiege : GameSkill
{
	public Electropiege(){
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
			
			GameView.instance.getGC().addTileModifier(target, amount, ModifierType.Type_Wolftrap, ModifierStat.Stat_No, -1, 4, "Electropiege", "Inflige "+amount+" dégats", "Permanent. Non visible du joueur adverse");
			GameView.instance.displaySkillEffect(GameView.instance.getGC().getCurrentPlayingCard(), text, 4);
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("pose un <b>électropiège</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override void activateTrap(int[] targets, int[] args){
		GameView.instance.getGC().addCardModifier(targets[0], args[0], ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		if(GameView.instance.getCard(targets[0]).GetLife()>0){
			GameView.instance.displaySkillEffect(targets[0], "HIT\n-"+args[0]+" PV", 5);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentTileTargets();
	}
	
	public override string getTargetText(int i, Card c){
		
		int amount = base.skill.ManaCost;
		string s = "Pose un piège\n-"+amount+" PV";
		
		return s ;
	}
}
