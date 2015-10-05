using UnityEngine;
using System.Collections.Generic;

public class Cannibale : GameSkill
{
	public Cannibale(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentAllyTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			GameView.instance.getGC().addTarget(target,1);
		}
		else{
			GameView.instance.getGC().addTarget(target,0);
		}
		GameView.instance.getGC().play();
	}
	
	public override void applyOn(){
		Card targetCard ;
		int target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		int currentLife ; 
		int amount ;
		int amount2 ; 
		
		for(int i = 0 ; i < base.targets.Count ; i++){
			target = base.targets[i];
			targetCard = GameView.instance.getCard(target);
			receivers.Add (targetCard);
			if (base.results[i]==0){
				text = "Esquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else if (base.results[i]==2){
				text = "Bonus 'Géant'\nEsquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				currentLife = targetCard.GetLife();
				amount = currentLife*base.skill.ManaCost/100;
				amount2 = targetCard.GetAttack()*base.skill.ManaCost/100;
				
				text="Sacrifié";
				receiversTexts.Add (text);
				GameView.instance.getGC().addCardModifier(target, currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
				
				GameView.instance.displaySkillEffect(target, text, 5);
				
				targetCard = GameView.instance.getCard(GameView.instance.getGC().getCurrentPlayingCard());
				GameView.instance.getGC().addCardModifier(GameView.instance.getGC().getCurrentPlayingCard(), amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life, -1, -1, "", "", "");
				GameView.instance.getGC().addCardModifier(GameView.instance.getGC().getCurrentPlayingCard(), amount2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, -1, "", "", "");
				receivers.Add (targetCard);
				text="+"+amount+" PV\n+"+amount2+" ATK";
				receiversTexts.Add (text);
				GameView.instance.displaySkillEffect(GameView.instance.getGC().getCurrentPlayingCard(), text, 4);
			}	
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Cannibale</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentAllys();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		string text = "SACRIFICE\n";
		
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
