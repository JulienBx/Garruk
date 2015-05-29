using UnityEngine;

public class StatModifier
{
	public int Amount;                      // de combien ça modifie        
	public int Duration;                    // -1 permanent ou bien le nombre de tour
	public int Area;                        // Nombre de cases adjacentes
	public ModifierType Type;               // AddPermanent = 0
	public ModifierStat Stat;               // Attack=0, Move=1, Energy=2, Speed=3, Damage=4, capacité à lancer des sorts
	public bool Active;

	public StatModifier()
	{

	}

	public StatModifier(int amount, ModifierType type, ModifierStat stat)
	{
		this.Amount = amount;
		this.Type = type;
		this.Stat = stat;
		this.Duration = -1;
		this.Active = true;
	}

	public StatModifier(int amount, ModifierType type, ModifierStat stat, int duration) : this(amount, type, stat)
	{
		this.Duration = duration;
		this.Active = true;
	}

	public StatModifier(int amount, ModifierType type, ModifierStat stat, int duration, bool active) : this(amount, type, stat, duration)
	{
		this.Active = active;
	}

	public int modifyAttack(int attack)
	{
		if (Stat == ModifierStat.Stat_Attack && Active)
		{
			attack += Amount;
		}
		return attack;
	}
	public int modifySpeed(int speed)
	{
		if (Stat == ModifierStat.Stat_Speed && Active)
		{
			speed += Amount;
			
		}
		return speed;
	}
	public int modifyMove(int move)
	{
		if (Stat == ModifierStat.Stat_Move && Active)
		{
			move += Amount;
		}
		return move;
	}
	
	public int modifyLife(int life)
	{
		if (Stat == ModifierStat.Stat_Life && Active)
		{
			life += Amount;
		}
		if (Stat == ModifierStat.Stat_Dommage)
		{
			life -= Amount;
		}
		return life;
	}
}
