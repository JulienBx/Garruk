using UnityEngine;
using System.Collections.Generic;

public class Lance : GameSkill
{
	public Lance(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
//	public override void launch()
//	{
//		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
//		GameView.instance.display1TileAwayOpponentsTargets();
//	}
//	
//	public override void resolve(List<int> targetsPCC)
//	{	
//		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//			GameView.instance.hideTargets();
//		}
//		
//		int target = targetsPCC[0];
//		
//		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
//		{                             
//			GameController.instance.addTarget(target,1);
//		}
//		else{
//			GameController.instance.addTarget(target,0);
//		}
//		
//		if (base.card.isGiant()){
//			Debug.Log("Je suis giant ");
//			
//			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
//				Debug.Log("Je lance giant ");
//				
//				List<Tile> opponents = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(target));
//				Debug.Log("Je trouve "+opponents.Count+" opponents");
//				
//				if(opponents.Count>=1){
//					int ran = Random.Range(0,opponents.Count);
//					target = GameView.instance.getTileCharacterID(opponents[ran].x, opponents[ran].y) ;
//					Debug.Log("Je choisis "+target);
//					
//					if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
//					{
//						GameController.instance.addTarget(target,3);
//					}
//					else{
//						GameController.instance.addTarget(target,2);
//					}
//				}
//			}
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
//		
//		for(int i = 0 ; i < base.targets.Count ; i++){
//			Debug.Log("Lance "+base.results[i]);
//						
//			target = base.targets[i];
//			targetCard = GameView.instance.getCard(target);
//			receivers.Add (targetCard);
//			if (base.results[i]==0){
//				text = "Esquive";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else if (base.results[i]==2){
//				text = "Bonus 'Géant'\nEsquive";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else{
//				amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*(50f+5f*base.skill.Level)/100f)*(1f-(targetCard.GetBouclier()/100f))));
//				
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
//				if(base.results[i]==3){
//					text = "Bonus Géant\n";
//				}
//				else{
//					text="";
//				}
//				
//				text+="-"+amount+" PV";
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
//			GameView.instance.setSkillPopUp("utilise <b>Lance</b>...", base.card, receivers, receiversTexts);
//		}
//	}
//	
//	public override string isLaunchable(){
//		return GameView.instance.canLaunch1TileAwayOpponents();
//	}
//	
//	public override string getTargetText(int id, Card targetCard){
//		
//		int currentLife = targetCard.GetLife();
//		int damageBonusPercentage = this.card.GetDamagesPercentageBonus(targetCard);
//		
//		int bouclier = targetCard.GetBouclier();
//		int amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*(50f+5f*base.skill.Level)/100f)*(1f-(targetCard.GetBouclier()/100f))));
//		
//		
//		string text = "PV : "+currentLife+"->"+(currentLife-amount)+"\n";
//		int probaEsquive = targetCard.GetEsquive();
//		int probaHit = Mathf.Max(0,100-probaEsquive) ;
//		
//		text += "HIT% : "+probaHit;
//		
//		return text ;
//	}
}
