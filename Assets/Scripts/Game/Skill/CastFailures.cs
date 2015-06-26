public class CastFailures
{
	string[] failures ;
	public static CastFailures instance;
	
	public CastFailures(){
		this.failures = new string[10];
		this.failures[0] = "Esquive";
		this.failures[1] = "Résiste";
		this.failures[2] = "Echec";
	}
	
	public string getFailure(int i){
		return this.failures[i];
	}
}