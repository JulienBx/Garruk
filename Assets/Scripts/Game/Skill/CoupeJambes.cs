using UnityEngine;
using System.Collections.Generic;

public class CoupeJambes : GameSkill
{
	public CoupeJambes(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			GameController.instance.addTarget(target,1);
		}
		else{
			GameController.instance.addTarget(target,0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(){
		Card targetCard ;
		int target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		int amount, amount2 ; 
		
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
				amount = (base.card.GetAttack()/2)*(100+base.card.GetDamagesPercentageBonus(targetCard))/100;
				int deplacement = targetCard.GetMove();	
				amount2 = Mathf.Min (deplacement-1, Mathf.CeilToInt(base.skill.ManaCost*deplacement/100));
				if (base.card.isLache()){
					if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
						if(GameView.instance.getPlayingCardTile(target).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
					else{
						if(GameView.instance.getPlayingCardTile(target).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
				}
				
				text="-"+amount+" PV";
				if(targetCard.GetLife()==amount){
					text+="\nMORT";
				}
				else{
					text+="\n-"+amount2+" MOV";
				}
				receiversTexts.Add (text);
				
				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
				GameController.instance.addCardModifier(target, -1*amount2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 8, "Lenteur", "-"+amount2+" MOV pour 1 tour", "Actif 1 tour");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Coupe-jambes</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		int manacost = base.skill.ManaCost;
		int deplacement = targetCard.GetMove();
		int bonusDeplacement = Mathf.FloorToInt(manacost*deplacement/100)+1;
		if (bonusDeplacement>=deplacement){
			bonusDeplacement = deplacement - 1 ;
		}
		int attack = base.card.GetAttack()/2;
		int amount = attack*(100+damageBonusPercentage)/100;
		string text = "";
		
		if (base.card.isLache()){
			if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
				if(GameView.instance.getPlayingCardTile(id).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
					amount = (100+base.card.getPassiveManacost())*amount/100;
					text="LACHE\n";
				}
			}
			else{
				if(GameView.instance.getPlayingCardTile(id).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
					amount = (100+base.card.getPassiveManacost())*amount/100;
					text="LACHE\n";
				}
			}
		}
		
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		text += "PV : "+currentLife+"->"+(currentLife-amount)+"\n";
		text += "MOV : "+deplacement+"->"+Mathf.Max(1,deplacement-bonusDeplacement)+"\n";
		
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
