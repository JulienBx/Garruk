using UnityEngine;
using System.Collections.Generic;

public class Adrenaline : GameSkill
{
	public Adrenaline()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			GameView.instance.getGC().addTarget(target,1);
		}
		else{
			GameView.instance.getGC().addTarget(target,0);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getAllys();
				if(allys.Count>1){
					allys.Remove(target);
					target = allys[Random.Range(0,allys.Count)];
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
					{
						GameView.instance.getGC().addTarget(target,3);
					}
					else{
						GameView.instance.getGC().addTarget(target,2);
					}
				}
			}
		}
		
		GameView.instance.getGC().play();
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
			else if (base.results[i]==2){
				text = "Bonus 'Généreux'\nEsquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				amount = Mathf.CeilToInt((base.skill.ManaCost)*targetCard.GetMove()/100f);
				if (base.card.isLache()){
					if(GameView.instance.getGC().getIsFirstPlayer()==GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
						if(GameView.instance.getPlayingCardTile(target).y==GameView.instance.getPlayingCardTile(GameView.instance.getGC().getCurrentPlayingCard()).y-1){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
					else{
						if(GameView.instance.getPlayingCardTile(target).y-1==GameView.instance.getPlayingCardTile(GameView.instance.getGC().getCurrentPlayingCard()).y){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
				}
				if(base.results[i]==3){
					text = "Bonus Généreux\n";
				}
				else{
					text="";
				}
				
				text+="+"+amount+" MOV";
				
				receiversTexts.Add (text);
				
				GameView.instance.getGC().addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 11, "Rapidité", "+"+amount+" MOV pour 1 tour", "Actif 1 tour");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Adrénaline</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAllysButMeTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.ManaCost;
		
		int baseD = targetCard.GetMove();
		int deplacement = Mathf.FloorToInt((amount)*baseD/100)+1;
		
		string text = "MOV: "+baseD+"->"+(baseD+deplacement)+"\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
