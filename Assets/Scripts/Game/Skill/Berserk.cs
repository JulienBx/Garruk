using UnityEngine;
using System.Collections.Generic;

public class Berserk : GameSkill
{
	public Berserk(){
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
		
		if (base.card.isGiant()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<Tile> opponents = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(target));
				if(opponents.Count>1){
					int ran = Random.Range(0,opponents.Count);
					target = GameView.instance.getTileCharacterID(opponents[ran].x, opponents[ran].y) ;
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
					{
						GameController.instance.addTarget(target,3);
					}
					else{
						GameController.instance.addTarget(target,2);
					}
				}
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
				if(base.skill.Level<=3){
					amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*110f/100f)*(1f-(targetCard.GetBouclier()/100f))));
				}
				else if(base.skill.Level<=5){
					amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*120f/100f)*(1f-(targetCard.GetBouclier()/100f))));
				}
				else if(base.skill.Level<=7){
					amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*130f/100f)*(1f-(targetCard.GetBouclier()/100f))));
				}
				else if(base.skill.Level<=9){
					amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*140f/100f)*(1f-(targetCard.GetBouclier()/100f))));
				}
				else {
					amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*150f/100f)*(1f-(targetCard.GetBouclier()/100f))));
				}
				
				if(base.results[i]==3){
					text = "Bonus Géant\n";
				}
				else{
					text="";
				}
				
				text+="-"+amount+" PV";
				
				if(targetCard.GetLife()==amount){
					text+="\nMORT";
				}
				receiversTexts.Add (text);
				
				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		
		targetCard = GameView.instance.getCard(GameController.instance.getCurrentPlayingCard());
		
		if(base.skill.Level<=1){
			amount2 = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetLife()*40f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=2){
			amount2 = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetLife()*35f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=4){
			amount2 = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetLife()*30f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=6){
			amount2 = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetLife()*25f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=8){
			amount2 = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetLife()*20f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else {
			amount2 = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetLife()*15f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		
		receivers.Add (targetCard);
		text="S'inflige "+amount2+" dégats";
		if(targetCard.GetLife()==amount2){
			text+="\nMORT";
		}
		receiversTexts.Add (text);
		
		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), amount2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), text, 5);
		
		GameView.instance.setSkillPopUp("lance <b>Berserk</b>...", base.card, receivers, receiversTexts);
		
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int myCurrentLife = base.card.GetLife();
		int currentLife = targetCard.GetLife();
		
		int myBouclier = base.card.GetBouclier();
		int bouclier = targetCard.GetBouclier();
		int amount ;
		
		if(base.skill.Level<=3){
			amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*110f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=5){
			amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*120f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=7){
			amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*130f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else if(base.skill.Level<=9){
			amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*140f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		else {
			amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*150f/100f)*(1f-(targetCard.GetBouclier()/100f))));
		}
		
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		string text = "PV: "+currentLife+"->"+(currentLife-amount)+"\n";
		
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
