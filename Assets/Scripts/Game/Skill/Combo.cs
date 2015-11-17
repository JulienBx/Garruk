using UnityEngine;
using System.Collections.Generic;

public class Combo : GameSkill
{
	public Combo()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
//	public override void launch()
//	{
//		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
//		GameView.instance.displayAdjacentOpponentsTargets();
//	}
//	
//	public override void resolve(List<int> targetsPCC)
//	{	
//		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//			GameView.instance.hideTargets();
//		}
//		
//		int target = targetsPCC[0];
//		int nbHitMax = Random.Range(1,5);
//		int arg = 0;
//		
//		for (int i = 0 ; i < nbHitMax ; i++){
//			if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
//			{                             
//				arg++;
//			}
//		}
//		
//		if (arg!=0){
//			GameController.instance.addTarget(target,1,arg);
//		}
//		else{
//			GameController.instance.addTarget(target,1,arg);
//		}
//		GameController.instance.play();
//	}
//	
//	public override void applyOn(){
//		Card targetCard ;
//		int target ;
//		string text ;
//		List<Card> receivers =  new List<Card>();
//		List<string> receiversTexts =  new List<string>();
//		
//		int amount ; 
//		
//		for(int i = 0 ; i < base.targets.Count ; i++){
//			target = base.targets[i];
//			targetCard = GameView.instance.getCard(target);
//			receivers.Add (targetCard);
//			if (base.results[i]==0){
//				text = "Esquive";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else{
//				amount = Mathf.Min(targetCard.GetLife(),base.values[i]*(base.card.GetAttack()*(1-(targetCard.GetBouclier()/100))));
//				if (base.card.isLache()){
//					if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//						if(GameView.instance.getPlayingCardTile(target).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
//							amount = (100+base.card.getPassiveManacost())*amount/100;
//						}
//					}
//					else{
//						if(GameView.instance.getPlayingCardTile(target).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
//							amount = (100+base.card.getPassiveManacost())*amount/100;
//						}
//					}
//				}
//				
//				text="Hits X"+base.values[i]+"\n-"+amount+" PV";
//			
//				if(targetCard.GetLife()==amount){
//					text+="\nMORT";
//				}
//				receiversTexts.Add (text);
//				
//				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//				
//				GameView.instance.displaySkillEffect(target, text, 5);
//			}	
//		}
//		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//			GameView.instance.setSkillPopUp("lance <b>Combo</b>...", base.card, receivers, receiversTexts);
//		}
//	}
//	
//	public override string isLaunchable(){
//		return GameView.instance.canLaunchAdjacentOpponents();
//	}
//	
//	public override string getTargetText(int id, Card targetCard){
//		
//		int currentLife = targetCard.GetLife();
//		int bouclier = targetCard.GetBouclier();
//		
//		int damageBonusPercentage = GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()).GetDamagesPercentageBonus(targetCard);
//		int amount = base.card.GetAttack()*this.skill.ManaCost*(100+damageBonusPercentage)/10000;
//		string text = "";
//		
//		if (base.card.isLache()){
//			if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//				if(GameView.instance.getPlayingCardTile(id).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
//					amount = (100+base.card.getPassiveManacost())*amount/100;
//					text="LACHE\n";
//				}
//			}
//			else{
//				if(GameView.instance.getPlayingCardTile(id).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
//					amount = (100+base.card.getPassiveManacost())*amount/100;
//					text="LACHE\n";
//				}
//			}
//		}
//		
//		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
//		
//		if(currentLife-Mathf.Min(currentLife,amount)==0){
//			text += "PV : "+currentLife+"->0";
//		}
//		else{
//			text += "PV : "+currentLife+"->"+(currentLife-Mathf.Min(currentLife,amount))+"-"+(currentLife-Mathf.Min(currentLife,(4*amount)));
//		}
//		
//		int probaEsquive = targetCard.GetEsquive();
//		int probaHit = Mathf.Max(0,100-probaEsquive) ;
//		
//		text += "HIT% : "+probaHit;
//		
//		return text ;
//	}
}
