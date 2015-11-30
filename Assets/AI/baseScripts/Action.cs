using UnityEngine;
using System.Collections;

public class Action {

	public float coinID;		//id of coin to move
	public float ratio;			//ratio/push to launch coin
	public Vector2 direction;	//direction to launch coin.

	public Action(float id,float r,Vector2 dir){
		coinID = id;
		ratio = r;
		direction = dir.normalized; //ensure that dir is normalized
	}

	public void print(){
		Debug.Log ("move coin " + coinID + " with a push of " + ratio + " in the direction " + direction);
	}

}
