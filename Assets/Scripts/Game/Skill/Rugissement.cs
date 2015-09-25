using UnityEngine;
using System.Collections.Generic;

public class Rugissement : GameSkill
{
	public Rugissement()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		List<int> targets = GameView.instance.getAllys();
		 
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) > GameView.instance.getCard(targets[i]).GetMagicalEsquive())
			{
				GameController.instance.addTarget(targets[i],1);
			}
			else{
				GameController.instance.addTarget(targets[i],0);
			}
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(){
		Card targetCard ;
		int target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		int amount ; 
		
		for(int i = 0 ; i < base.targets.Count ; i++){
			target = base.targets[i];
			targetCard = GameView.instance.getCard(target);
			receivers.Add (targetCard);
			if (base.results[i]==0){
				text = "Esquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				amount = base.skill.ManaCost;
				
				text="+"+amount+" ATK";
				receiversTexts.Add (text);
				
				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 9, "Renforcé", "+"+amount+" ATK pour 1 tour", "Actif 1 tour");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Rugissement</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAllysButMeTargets();
	}
}
