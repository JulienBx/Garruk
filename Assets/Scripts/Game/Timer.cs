public class Timer
{
	public float timeBeforeFight = 30 ;
	public float timePerTurn = 30 ;
	
	float timer ;
	int timerSeconds ;
	
	public Timer(){
		
	}
	
	public void resetTimer(bool hasFightStarted){
		if (hasFightStarted){
			this.timer = this.timePerTurn ;
		}
		else{
			this.timer = this.timeBeforeFight ;
		}
	}
}

