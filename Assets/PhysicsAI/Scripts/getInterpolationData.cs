using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class getInterpolationData : MonoBehaviour {

	public GameState curr;

	public float impactDamp;

	public int clicks;
	Vector3 clickPos;
	public LineRenderer lr;
	public LineRenderer lr2;
	public LineRenderer lr3;

	float epsilon = 0.04f;
	float slowSpeed = .4f;

	public GameObject simulateCoin;

	public Rigidbody testCoin;
	float prevXPos;
	float currXPos;

	void Awake(){
		float i = 1.0f;
		List<Coin> p1coins = new List<Coin>();
		List<Coin> p2coins = new List<Coin>();
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p1"))
		{
			Vector3 pos = c.transform.position;
			float id = 1.0f;
			id += i/10;
			i += 1;
			Coin c1 = new Coin(pos,id);
			p1coins.Add(c1);
			c.name = id.ToString();
		}
		float j = 1.0f;
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("p2"))
		{
			Vector3 pos = c.transform.position;
			float id = 2.0f;
			id += j/10;
			j += 1;
			Coin c2 = new Coin(pos,id);
			p2coins.Add(c2);
			c.name = id.ToString();
		}
		curr = new GameState (p1coins, p2coins);
		//curr.print ();
	}
	/*
	void Start () {

		prevXPos = testCoin.position.x;
		//StartCoroutine (simulateGivenForce (1.0f));
		StartCoroutine (addGivenForce (.6f)); // actually adds force to testCoin
		StartCoroutine (simulateNonConstantVel ());

	
	}*/
	
	IEnumerator drawLine(Vector3 start, Vector3 end, float dur){
		lr.enabled = true;
		lr.SetPosition (0, start);
		lr.SetPosition (1, end);
		yield return new WaitForSeconds (dur);
		lr.enabled = false;
		yield return null;
	}

	IEnumerator drawLine2(Vector3 start, Vector3 end, float dur){
		lr2.enabled = true;
		lr2.SetPosition (0, start);
		lr2.SetPosition (1, end);
		yield return new WaitForSeconds (dur);
		lr2.enabled = false;
		yield return null;
	}

	IEnumerator drawLine3(Vector3 start, Vector3 end, float dur){
		lr3.enabled = true;
		lr3.SetPosition (0, start);
		lr3.SetPosition (1, end);
		yield return new WaitForSeconds (dur);
		lr3.enabled = false;
		yield return null;
	}

	void Start(){
		lr = GameObject.Find("LineRenderer").GetComponent<LineRenderer> ();
		lr2 = GameObject.Find("LineRenderer2").GetComponent<LineRenderer> ();
		lr3 = GameObject.Find("LineRenderer3").GetComponent<LineRenderer> ();
		clicks = 0;

		impactDamp = .77f;
		/*
		float[] testInterpolate = interpolatePush(.3f);
		Debug.Log (testInterpolate [0]);
		Debug.Log (testInterpolate [1]);
		*/

		/*
		Action testA = new Action (1, 1, new Vector2 (1, 1));
		testA.print ();
		*/
	}

	//this outputs (disp, duration) pair specifying how far and how long to move coin
	float[] interpolatePush(float ratio){
		float[] rtn = new float[2]; 
		if (ratio <= .1f) {
			float t = Mathf.InverseLerp(0,.1f,ratio);
			//Debug.Log ("hi");
			rtn[0] = Mathf.Lerp(0,.098f,t);
			rtn[1] = Mathf.Lerp(0,.713f,t);
			return rtn;
		}
		if (ratio <= .2f) {
			float t = Mathf.InverseLerp(.1f,.2f,ratio);
			rtn[0] = Mathf.Lerp(.098f,.527f,t);
			rtn[1] = Mathf.Lerp(.713f,.6f,t);
			return rtn;
		}
		if (ratio <= .3f) {
			float t = Mathf.InverseLerp(.2f,.3f,ratio);
			rtn[0] = Mathf.Lerp(.527f,1.23f,t);
			rtn[1] = Mathf.Lerp(.6f,.82f,t);
			return rtn;
		}
		if (ratio <= .4f) {
			float t = Mathf.InverseLerp(.3f,.4f,ratio);
			rtn[0] = Mathf.Lerp(1.23f,2.17f,t);
			rtn[1] = Mathf.Lerp(.82f,1.21f,t);
			return rtn;
		}
		if (ratio <= .5f) {
			float t = Mathf.InverseLerp(.4f,.5f,ratio);
			rtn[0] = Mathf.Lerp(2.17f,3.29f,t);
			rtn[1] = Mathf.Lerp(1.21f,1.39f,t);
			return rtn;
		}
		if (ratio <= .6f) {
			float t = Mathf.InverseLerp(.5f,.6f,ratio);
			rtn[0] = Mathf.Lerp(3.29f,4.574f,t);
			rtn[1] = Mathf.Lerp(1.39f,1.491f,t);
			return rtn;
		}
		if (ratio <= .7f) {
			float t = Mathf.InverseLerp(.6f,.7f,ratio);
			rtn[0] = Mathf.Lerp(4.574f,6.0f,t);
			rtn[1] = Mathf.Lerp(1.491f,1.543f,t);
			return rtn;
		}
		if (ratio <= .8f) {
			float t = Mathf.InverseLerp(.7f,.8f,ratio);
			rtn[0] = Mathf.Lerp(6.0f,7.56f,t);
			rtn[1] = Mathf.Lerp(1.543f,1.577f,t);
			return rtn;
		}
		if (ratio <= .9f) {
			float t = Mathf.InverseLerp(.8f,.9f,ratio);
			rtn[0] = Mathf.Lerp(7.56f,9.234f,t);
			rtn[1] = Mathf.Lerp(1.577f,1.542f,t);
			return rtn;
		}
		if (ratio <= 1.0f) {
			float t = Mathf.InverseLerp(.9f,1.0f,ratio);
			rtn[0] = Mathf.Lerp(9.234f,11.0f,t);
			rtn[1] = Mathf.Lerp(1.542f,1.676f,t);
			return rtn;
		}
		//make this an else
		return rtn;
	} 

	void runTest () {
		prevXPos = testCoin.position.x;
		//StartCoroutine (simulateGivenForce (1.0f));
		StartCoroutine (addGivenForce (1.0f)); // actually adds force to testCoin
		StartCoroutine (simulateNonConstantVel ());
		
		
	}

	void humanLaunchCoin(Vector2 pos){
		Ray r = Camera.main.ScreenPointToRay(pos);
		Vector3 p = Vector3.zero;
		RaycastHit test;
		Transform targetCoin;
		if (Physics.Raycast (r.origin, r.direction, out test, Mathf.Infinity)) {
			p = test.point;
			GameObject gotHit = test.collider.gameObject;

			Vector3 adjust = test.normal;
			adjust.Normalize();

			if (!gotHit.name.StartsWith("Terrain")){
				p += -.1f*adjust;
			} else {
				p += .1f*adjust;
			}
		}

		if (clicks == 0) {
			clickPos = p;
			clicks += 1;
		} else {
			clicks = 0;
			Vector3 dir = p - clickPos;
			//StartCoroutine(drawLine(clickPos,p,1.0f));
			RaycastHit hit;
			float dist = dir.magnitude;
			if (Physics.Raycast(clickPos,dir,out hit, dist)){

				float distToHit = Vector3.Distance(hit.point,clickPos);
				float ratio = distToHit/dist;

				Action humanAction = new Action(float.Parse(hit.collider.gameObject.name),ratio,new Vector2(dir.x,dir.z));
				//humanAction.print();
				StartCoroutine(displayLaunch2(humanAction,1));

				//below works when ignoring collisions
				/*
				targetCoin = hit.collider.gameObject.GetComponent<Transform>();
				Debug.Log(ratio);
				//IEnumerator displayLaunch(Transform coin, Vector3 targetPos1, float stepX, float stepY, float moveDist, float moveDur, Vector3 earlyStop)
				float[] interpolate = interpolatePush(ratio);
				float moveDist = interpolate [0];
				float moveDur = interpolate [1];
				dir.Normalize();
				float stepX = dir.x;
				float stepY = dir.z;
				Vector3 targetPos = targetCoin.position + dir*moveDist;
				StartCoroutine(displayLaunch(targetCoin, targetPos, stepX, stepY, moveDist, moveDur, Vector3.zero,false));
				*/
			}
		}

	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			//runTest();
			humanLaunchCoin(Input.mousePosition);
		}
	}
	
	IEnumerator addGivenForce(float ratio){
		Vector3 dir = new Vector3 (1, 0, 0);
		float mag = ratio * 4700;
		testCoin.AddForce (dir * mag);
		yield return new WaitForSeconds (.01f);
		float startTime = Time.time;
		while (testCoin.velocity.sqrMagnitude > 0) {
			yield return null;
		}
		float endTime = Time.time;
		float elapsed = endTime - startTime;
		currXPos = testCoin.position.x;
		float disp = currXPos - prevXPos;
		//Debug.Log ("displacement corresponding to a push of " + ratio.ToString () + " is " + disp.ToString ());
		//Debug.Log (elapsed);
		yield return null;
	}

	/*
	//from cmu university
	//.5 case right now and well looks OK but in general problem is that we see constant velocity, no slowing/speeding
	IEnumerator simulateGivenForce(float ratio){
		Vector3 oldPos = simulateCoin.transform.position;
		Vector3 targetPos = oldPos;
		targetPos.x += 3.3f;
		float moveDur = 1.0f;
		float moveTime = 0.0f;
		while (moveTime < moveDur) {
			moveTime += Time.deltaTime;
			simulateCoin.transform.position = Vector3.Lerp(oldPos,targetPos,moveTime/moveDur);
			yield return null;
		}

		yield return null;
	}
	*/

	//adapt above, start by getting it to be constant vel, then try for a speedup/speed down
	IEnumerator simulateNonConstantVel(){
		Vector3 oldPos = simulateCoin.transform.position;
		Vector3 targetPos = oldPos;
		float moveDist = 11.0f;
		targetPos.x += moveDist;
		Debug.Log (targetPos);

		float moveDur = 1.676f;
		float moveTime = 0.0f;
		float halfTime = moveDur / 2;

		Vector3 halfPos = Vector3.Lerp (oldPos, targetPos, 0.5f);

		float avgVel = moveDist / moveDur;

		/*
		//constant velocity using average--OK
		while (moveTime < moveDur) {
			moveTime += Time.deltaTime;
			//simulateCoin.transform.position = Vector3.Lerp(oldPos,targetPos,moveTime/moveDur);
			Vector3 stepResult = simulateCoin.transform.position;
			stepResult.x += avgVel*Time.deltaTime;
			simulateCoin.transform.position = stepResult;
			yield return null;
		}*/
		/* was a valient attempt. but i feel on the right track, maybe interpolate similarly slowdown
		//speed up
		while (moveTime < halfTime    && Vector3.Distance(simulateCoin.transform.position,halfPos) > epsilon) {
			moveTime += Time.deltaTime;
			//simulateCoin.transform.position = Vector3.Lerp(oldPos,targetPos,moveTime/moveDur);
			Vector3 stepResult = simulateCoin.transform.position;
			float vel = Mathf.Lerp(0.0f,2*avgVel,moveTime/halfTime);
			stepResult.x += vel*Time.deltaTime;
			simulateCoin.transform.position = stepResult;
			yield return null;
		}

		//slow down
		while (moveTime < moveDur     && Vector3.Distance(simulateCoin.transform.position,targetPos) > epsilon) {
			moveTime += Time.deltaTime;
			//simulateCoin.transform.position = Vector3.Lerp(oldPos,targetPos,moveTime/moveDur);
			Vector3 stepResult = simulateCoin.transform.position;
			float vel = Mathf.Lerp(2*avgVel,0.0f,(moveTime-halfTime)/halfTime);
			stepResult.x += vel*Time.deltaTime;
			simulateCoin.transform.position = stepResult;
			yield return null;
		}
		*/
		float vel = 2.0f*avgVel;
		while (moveTime < moveDur   && Vector3.Distance(simulateCoin.transform.position,targetPos) > epsilon ) {
			moveTime += Time.deltaTime;
			//simulateCoin.transform.position = Vector3.Lerp(oldPos,targetPos,moveTime/moveDur);
			Vector3 stepResult = simulateCoin.transform.position;
			vel = Mathf.Lerp(2.1f*avgVel,0.0f,moveTime/moveDur);
			stepResult.x += vel*Time.deltaTime;
			simulateCoin.transform.position = stepResult;
			//moveTime += Time.deltaTime;
			yield return null;
		}
		while (Vector3.Distance(simulateCoin.transform.position,targetPos) > epsilon) {
			//float step = slowSpeed * Time.deltaTime;
			float step = vel * Time.deltaTime;
			simulateCoin.transform.position = Vector3.MoveTowards(simulateCoin.transform.position, targetPos, step);
			yield return null;
		}
		//Debug.Log ("here " + targetPos.ToString());

		//Guaranteed to reach the target position--all the preceding just for show!/fake physics!
		simulateCoin.transform.position = targetPos;
		//Debug.Log ("here" + simulateCoin.transform.position.ToString());
		yield return null;
	}

	//takes a coin, target position, stepX,stepY, distance (could be recomputed stepX/Y are components cosTheta, sinTheta of launch dir), 
	//duration, and (possibly null) early stop position 
	IEnumerator displayLaunch(Transform coin, Vector3 targetPos1, float stepX, float stepY, float moveDist, float moveDur, Vector3 earlyStop,bool stopEarly){

		float moveTime = 0.0f;
		float halfTime = moveDur / 2;
		Vector3 targetPos = targetPos1;
		float avgVel = moveDist / moveDur;
		float vel = 2.0f*avgVel;

		if (stopEarly) {
			//Debug.Log(targetPos);
			targetPos = earlyStop;
			//Debug.Log(targetPos);
		}
		//Debug.Log ("target pos: " + targetPos);
		//Debug.Log ("StepX: " + stepX);
		//Debug.Log ("StepY: " + stepY);
		float prevdiff; //distance between coin and target. want this to only decrease (as in come closer)
		while (moveTime < moveDur && Vector3.Distance(coin.position,targetPos) > epsilon ) {
			prevdiff = Vector3.Distance(coin.position,targetPos);
			moveTime += Time.deltaTime;
			Vector3 stepResult = coin.transform.position;
			vel = Mathf.Lerp(2.1f*avgVel,0.0f,moveTime/moveDur);
			stepResult.x += vel*Time.deltaTime*stepX;
			stepResult.z += vel*Time.deltaTime*stepY;
			coin.transform.position = stepResult;

			float diff = Vector3.Distance(coin.position,targetPos);
			if (diff > prevdiff){
				//Debug.Log ("overshot");
				break;
			}
			//moveTime += Time.deltaTime;
			yield return null;
		}
		while (Vector3.Distance(coin.transform.position,targetPos) > epsilon) {
			//float step = slowSpeed * Time.deltaTime;
			float step = vel * Time.deltaTime;
			//Debug.Log(vel);
			coin.transform.position = Vector3.MoveTowards(coin.transform.position, targetPos, step);
			yield return null;
		}
		//Debug.Log ("here " + targetPos.ToString());
		
		//Guaranteed to reach the target position--all the preceding just for show!/fake physics!
		//Debug.Log ("original target pos: " + targetPos1);
		//Debug.Log ("actual target pos: " + targetPos);
		coin.transform.position = targetPos;

		yield return null;
	}

	//new display launch which I want to deal with collisions + better form
	//this knows the current game state (the curr variable)
	//uses the coroutine "displayLaunch" to display to player coin movement (if no collosions use bool stopEarly=false, else true and recurse on this )
	//updates curr gamestate
	IEnumerator displayLaunch2(Action a, int depth){
		string coinName = a.coinID.ToString ();
		Transform coinLaunched = GameObject.Find (coinName).transform;
		float[] interpolate = interpolatePush(a.ratio);
		float moveDist = interpolate [0];
		float moveDur = interpolate [1];
		float stepX = a.direction.x;
		float stepY = a.direction.y;
		Vector3 dirInGame = new Vector3 (a.direction.x, 0.0f, a.direction.y); //basically get the direction in the game for purposes of display there's this xz plane thing
		Vector3 targetPos = coinLaunched.position + dirInGame*moveDist;
		//StartCoroutine(drawLine2(coinLaunched.position,targetPos,3.0f)); //comment this back in--super useful!

		//create a transform closestCoinCanHit and a float that is the dist to hit it and then loop thru all coins
		//keep updating till either get the coin actually hit (lowest dist) and recurse or none hit at all

		bool hitACoin = false;				//change to true if can hit a coin
		Transform coinHit = coinLaunched;	//change this to coin first hit/the coin really hit
		float bestDist = 1000000000.0f;		//obscenely large, will store dist that lcoin needs to travel to hit coinHit

		foreach (Coin c in curr.p1coins) {
			//lcoin will refer to launched coin in comments
			//test to only hit coins that are not lcoin
			if (c.id == a.coinID){
				continue;
			}
			//test to ensure that this coin is within the circle swept by the motion of lcoin else definitely not reachable
			float coinDist = Vector3.Distance(c.pos,coinLaunched.position) - 1.0f; //1.0f is the sum of the radius of the coins
			if (moveDist < coinDist){
				continue;
			}
			//test to ensure that lcoin is moving towards coin/some component is
			Vector3 atob = c.pos - coinLaunched.position;
			float distToShortestPointOnV = Vector3.Dot(atob,dirInGame);
			if ( distToShortestPointOnV <= 0){
				continue;
			}
			//test closest dist from coin to the launch line
			float atobSqrLen = Vector3.SqrMagnitude(atob);
			float closestDistSqrLen = atobSqrLen - (distToShortestPointOnV * distToShortestPointOnV);
			if (closestDistSqrLen >= 1.0f){ //1.0f is the sum of the radii squared as (.5+.5) squared = 1
				continue;
			}

			float t = 1.0f - closestDistSqrLen; //again 1 is but 1 squared
			if (t<=0){
				Debug.Log("shouldn't see this?");
				continue;
			}
			float distToColl = distToShortestPointOnV - Mathf.Sqrt(t);
			if (distToColl > moveDist){
				return false;
			} 
			//made it here, lcoin does hit this coin
			hitACoin = true;
			if (distToColl < bestDist){ //so found that we hit an even closer coin
				coinHit = GameObject.Find(c.id.ToString()).transform;
				bestDist = distToColl;
			}

			//Debug.Log("Coin " + c.id + " can potentially, or rather should, be hit");
		}

		foreach (Coin c in curr.p2coins) {
			if (c.id == a.coinID){
				continue;
			}
			float coinDist = Vector3.Distance(c.pos,coinLaunched.position) - 1.0f; //1.0f is the sum of the radius of the coins
			if (moveDist < coinDist){
				continue;
			}
			Vector3 atob = c.pos - coinLaunched.position;
			float distToShortestPointOnV = Vector3.Dot(atob,dirInGame);
			if ( distToShortestPointOnV <= 0){
				continue;
			}
			float atobSqrLen = Vector3.SqrMagnitude(atob);
			float closestDistSqrLen = atobSqrLen - (distToShortestPointOnV * distToShortestPointOnV);
			if (closestDistSqrLen >= 1.0f){ //1.0f is the sum of the radii squared as (.5+.5) squared = 1
				continue;
			}
			float t = 1.0f - closestDistSqrLen; //again 1 is but 1 squared
			if (t<=0){
				continue;
			}
			float distToColl = distToShortestPointOnV - Mathf.Sqrt(t);
			if (distToColl > moveDist){
				return false;
			} 
			hitACoin = true;
			if (distToColl < bestDist){ //so found that we hit an even closer coin
				coinHit = GameObject.Find(c.id.ToString()).transform;
				bestDist = distToColl;
			}

			//Debug.Log("Coin " + c.id + " can potentially, or rather should, be hit");

		}

		if (hitACoin) {
			Vector3 hitPos = coinLaunched.position + dirInGame * bestDist;
			//Debug.Log(hitPos);
			//StartCoroutine (drawLine3 (coinLaunched.position, hitPos, 4.5f));//longer duration as it's occluded
			yield return StartCoroutine(displayLaunch(coinLaunched, targetPos, stepX, stepY, moveDist, moveDur, hitPos,true));
			//then update curr gamestate and recurse on this coroutine
			curr.update(a.coinID,hitPos);

			//recurse
			float recurseFactor = 1.0f; //how much to damp the original ratio
			recurseFactor *= Mathf.Pow ((moveDist - bestDist)/moveDist,.8f); //so hit early in path = more push, and later in the path less--scaled linearly
			Vector2 newDir = Vector2.zero;//dir to launch coinHit in recursion
			newDir.x = coinHit.position.x - hitPos.x;
			newDir.y = coinHit.position.z - hitPos.z;
			recurseFactor *= Mathf.Pow(Vector3.Dot(coinHit.position - hitPos,dirInGame),.9f);
			//recurseFactor *= Vector3.Dot(coinHit.position - hitPos,dirInGame);
			recurseFactor *= Mathf.Pow (impactDamp,depth);
			Debug.Log(recurseFactor);

			newDir = newDir + 1.25f*a.direction;//blend these
			Action newAct = new Action(float.Parse(coinHit.gameObject.name),recurseFactor,newDir);
			yield return StartCoroutine(displayLaunch2(newAct,depth + 1));
		
		} else { //so nothing/base case: just move lcoin and update curr gamestate
			yield return StartCoroutine(displayLaunch(coinLaunched, targetPos, stepX, stepY, moveDist, moveDur, Vector3.zero,false));
			curr.update(a.coinID,targetPos);
		}

		yield return null;
	}


}
