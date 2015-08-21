using UnityEngine;
using System.Collections.Generic;

public class Dissipation : GameSkill
{
	public Dissipation()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllButMeModifiersTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		int successChances = base.skill.ManaCost;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.failedToCastOnSkill(target, 2);
			}
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		GameView.instance.getCard(target).emptyModifiers();
		GameView.instance.getPCC(target).show();
		GameView.instance.displaySkillEffect(target, "DISSIPATION", 4);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		if (indexFailure==1){
			GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
		}
		else if (indexFailure==2){
			GameView.instance.displaySkillEffect(target, "ECHEC DISSIPATION", 4);
		}
	}
	
	public override string isLaunchable()
	{
		return GameView.instance.canLaunchAllButMeModifiersTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = base.card.GetAttack();
		string text;
		
		text = "Dissipe les effets\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int proba ;
		text += "HIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			text+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
		}
		else{
			proba = 100;
			text+=proba+"%";
		}
		
		return text ;
	}
}
