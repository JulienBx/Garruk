using UnityEngine;

public class StatModifier
{
	public int Amount;                    // de combien ça modifie        
	public int Duration;                  // -1 permanent ou bien le nombre de tour
	public int Area;                        // Nombre de cases adjacentes
	public ModifierType Type;               // AddPermanent = 0
	public ModifierStat Stat;               // Attack=0, Move=1, Energy=2, Speed=3, Damage=4, capacité à lancer des sorts

	public StatModifier()
	{

	}

	public StatModifier(int amount, ModifierType type, ModifierStat stat)
	{
		this.Amount = amount;
		this.Type = type;
		this.Stat = stat;
	}

	public int modifyAttack(int attack)
	{
		if (Stat == ModifierStat.Stat_Attack)
		{
			attack += Amount;
		}
		return attack;
	}
	public int modifySpeed(int speed)
	{
		if (Stat == ModifierStat.Stat_Speed)
		{
			speed += Amount;
			
		}
		return speed;
	}
	public int modifyMove(int move)
	{
		if (Stat == ModifierStat.Stat_Move)
		{
			move += Amount;
		}
		return move;
	}
	
	public int modifyLife(int life)
	{
		if (Stat == ModifierStat.Stat_Life)
		{
			life += Amount;
		}
		return life;
	}
}
