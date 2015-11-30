using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapGrid : MonoBehaviour
	
{
	public int owner;

	private bool locked;

	private Vector3 screenPoint;
	private Vector3 offset;

	public int[] finalPosition;
	public int fpX;
	public int fpY;

	public Vector3 initWorldPosition;

	//neighbors
	public SnapGrid n1 = null;
	public SnapGrid n2 = null;
	public SnapGrid n3 = null;
	public SnapGrid n4 = null;

	public bool selected;

	public storePipes sp;


	public List<Collider> colliderList = new List<Collider>();

	void OnTriggerEnter(Collider other){
		//if the object is not already in the list
		if(!colliderList.Contains(other))
		{
			//add the object to the list
			colliderList.Add(other);
		}
	}

	void OnTriggerExit(Collider other)
	{
		//if the object is in the list
		if(colliderList.Contains(other))
		{
			//remove it from the list
			colliderList.Remove(other);
		}
	}

	/*
	public TerrainData terrDat;
	int xRes;
	int yRes;


	void Start(){
		xRes = terrDat.heightmapWidth;
		yRes = terrDat.heightmapHeight;

	}
	*/
	void Awake(){
		sp = GameObject.Find ("PipeManager").GetComponent<storePipes> ();
	}
	void Start(){
		//means that it hasn't been dragged and released onto the grid yet--so give it impossible start entries--negatives
		finalPosition = new int[2];
		finalPosition [0] = -1;
		finalPosition [1] = -1;

		initWorldPosition = transform.position;
		
	}

	bool isMyTurn(){
		return (owner == testGame.turn);
	}
	
	void OnMouseDown() 
	{ 
		if (!isMyTurn ()) {  
			return;
		}
		if (locked) {
			return; // don't want to allow any more dragging once pipe segment is placed!
		}
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		
		//Cursor.visible = false;
		
		
	}
	
	void OnMouseDrag() 
	{ 
		if (!isMyTurn ()) {  
			return;
		}
		if (locked) {
			return; // don't want to allow any more dragging once pipe segment is placed!
		}
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		
		transform.position = SnapToGrid(curPosition);
		
	}

	void OnMouseUp() 
	{ 
		if (!isMyTurn ()) {  
			return;
		}
		//this is bugtestGame.clicks = 0;
		if (!locked) {
			testGame.clicks = 0;
		} else {
			return;//added
		}

		//record the CELL position landed on
		int x = (int)(centerRound(transform.position.x,20,1) - .5f);
		int y = (int)(centerRound(transform.position.z,15,1) - .5f);
		      
		finalPosition [0] = x;
		finalPosition [1] = y;
		//Debug.Log ("Landed on cell indexed at: (" + finalPosition [0] + ", " + finalPosition [1] + ")"); //works :)
		fpX = x;
		fpY = y;

		//check to see if we're trying to illegally place this segment
		if ((storePipes.cellRaised(finalPosition)||storePipes.cellOccupied(finalPosition)||storePipes.cellOccupiedP2(finalPosition)) && !locked) {
			transform.position = initWorldPosition;
			//Debug.Log (finalPosition[0] + " " + finalPosition[1]);
			return; //revert to starting location, still unlocked
		}
		storePipes.registerSnapGrid (gameObject.GetComponent<SnapGrid> (),owner);
		storePipes.addSegment (finalPosition,owner);

		//update the UI to indicate that we've consumed one more pipe segment
		if (owner == 1) {
			sp.removePipeSegmentP1 ();
		} else {
			sp.removePipeSegmentP2 ();
		}

		//lock the pipe seg, so can't drag it anymore--it's final
		locked = true;


	}

	
	
	Vector3 SnapToGrid(Vector3 Position)
	{
		Vector3 ohSnap = Vector3.zero;
		float x = Position.x;
		float y = Position.z;
		//ok so base (0,0) bottom left coord in world space of grid is 12.5,12.5. And rest of tiles are mults of 25 away
		//need to modify the x,y to snap to center of cells
		float xmod = 0;
		float ymod = 0;

		xmod = centerRound (x, 20, 1);
		ymod = centerRound (y, 15, 1);

		ohSnap.x = xmod;
		ohSnap.z = ymod;

		return ohSnap;
	}

	float centerRound(float x,int numSegs,float widthSeg){
		float rtn = 0;

		for (int i=0; i<numSegs; i++) {
			//float lowerBound = offset + i*widthSeg;
			//float upperBound = offset + (i+1)*widthSeg;
			float lowerBound = i*widthSeg;
			float upperBound = (i+1)*widthSeg;
			if ((x >= lowerBound)&&(x < upperBound)){
				rtn = .5f*(lowerBound + upperBound);
				//Debug.Log("lowerbound: " + lowerBound + "upperBound: " + upperBound);
			}
		}

		if (rtn == 0) {
			//Debug.Log("Overshoot");
			rtn = widthSeg/2.0f;
		}

		return rtn;
	}
}



