using UnityEngine;

public class Modifyer
{
	public int amount;                      // de combien ça modifie        
	public int duration;                    // -1 permanent ou bien le nombre de tour
	public int type;
	
	public string title ;
	public string description ;
	
	public Modifyer(){
		this.amount = -1;
		this.duration = -1;
		this.type = -1;
		this.title = "";
		this.description = "";
	}
	
	public Modifyer(int a, int du, int t, string s, string de)
	{
		this.amount = a;
		this.duration = du;
		this.type = t;
		this.title = s;
		this.description = de;
	}
}
