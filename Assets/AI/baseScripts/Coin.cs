using UnityEngine;
using System.Collections;

public class Coin{
	//public float health; //from 0 to 1. Later
	public Vector3 pos; //position
	public float id; //1.1 1.2 2.1 2.2 X.Y means player X coin # Y

	public Coin(Vector3 p, float label){
		//health = h;
		pos = p;
		id = label;
	}

}
