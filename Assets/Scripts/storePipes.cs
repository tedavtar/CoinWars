using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class storePipes : MonoBehaviour {

	//list of all locations that have pipe segments 
	public static List<int[]> pipedCells;
	public static List<int[]> pipedCellsP2;

	public static List<SnapGrid> sgs;
	public static List<SnapGrid> sgsP2;

	public static bool teleportationInitiated;
	public static GameObject CoinThatCanTeleport;

	public static bool teleportationInitiatedP2;
	public static GameObject CoinThatCanTeleportP2;


	static public Color p1unlockedColor;
	static public Color p2unlockedColor;


	static Vector3 p1PipeStartPos;
	static Vector3 p2PipeStartPos;

	public GameObject p1PipeSeg;
	public Text p1PipeNumText;

	public GameObject p2PipeSeg;
	public Text p2PipeNumText;

	public int initNumPipeSegs;
	public int extraPipeSegsPerTurn;
	int p1PipeSegs;
	int p2PipeSegs;




	void Awake(){
		sgs = new List<SnapGrid> ();
		sgsP2 = new List<SnapGrid> ();
		pipedCells = new List<int[]> ();
		pipedCellsP2 = new List<int[]> ();
		teleportationInitiated = false;
		CoinThatCanTeleport = null;
		teleportationInitiatedP2 = false;
		CoinThatCanTeleportP2 = null;


		p1unlockedColor = new Color (231.0f / 255.0f, 207.0f / 255.0f, 15.0f / 255.0f);
		p2unlockedColor = new Color (244.0f / 255.0f, 0.0f, 0.0f);

		p1PipeStartPos = new Vector3 (-.61f, 1.7f, 7.26f);
		p2PipeStartPos = new Vector3 (20.61f, 1.7f, 7.26f);

		initNumPipeSegs = 3;
		extraPipeSegsPerTurn = 2;
		p1PipeSegs = 0;
		p2PipeSegs = 0;
		addPipeSegmentsP1(initNumPipeSegs);
		addPipeSegmentsP2(initNumPipeSegs);
	}

	public void resetP1PipeNum(int num){
		p1PipeNumText.text = "P1 Pipes: " + num.ToString ();
	}

	public void resetP2PipeNum(int num){
		p2PipeNumText.text = "P2 Pipes: " + num.ToString ();
	}

	public void addPipeSegmentsP1(int numSegments){
		for (int i = 0; i< numSegments; i++) {
			//load p1 pipe segments
			Instantiate(p1PipeSeg,p1PipeStartPos,Quaternion.identity);
			p1PipeSegs += 1;
		}
		resetP1PipeNum (p1PipeSegs);
	}

	public void addPipeSegmentsP2(int numSegments){
		for (int i = 0; i< numSegments; i++) {
			//load p1 pipe segments
			Instantiate(p2PipeSeg,p2PipeStartPos,Quaternion.identity);
			p2PipeSegs += 1;
		}
		resetP2PipeNum (p2PipeSegs);
	}

	public void removePipeSegmentP1(){
		p1PipeSegs -= 1;
		resetP1PipeNum (p1PipeSegs);
	}

	public void removePipeSegmentP2(){
		p2PipeSegs -= 1;
		resetP2PipeNum (p2PipeSegs);
	}

	public void addSegsP1Turn(){
		addPipeSegmentsP1 (extraPipeSegsPerTurn);
	}

	public void addSegsP2Turn(){
		addPipeSegmentsP2 (extraPipeSegsPerTurn);
	}

	public static void signalTeleportation(GameObject g){
		teleportationInitiated = true;
		CoinThatCanTeleport = g;
	}

	public static void signalTeleportationP2(GameObject g){
		teleportationInitiatedP2 = true;
		CoinThatCanTeleportP2 = g;
	}

	public static void clearTeleportation(){
		teleportationInitiated = false;
		CoinThatCanTeleport = null;
		foreach (SnapGrid sg in sgs) {
			sg.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
			sg.selected = false;
		}

	}

	public static void clearTeleportationP2(){
		teleportationInitiatedP2 = false;
		CoinThatCanTeleportP2 = null;
		foreach (SnapGrid sg in sgsP2) {
			sg.gameObject.GetComponent<Renderer>().material.color = Color.red;
			sg.selected = false;
		}
		
	}

	//indicates if grid cell with given location is raised
	public static bool cellRaised(int[] loc){
		bool rtn = false;
		foreach (int[] cellLoc in levelBuild.filledCells) {
			if((cellLoc[0]==loc[0])&&(cellLoc[1]==loc[1])){
				rtn = true;
			}
		}
		return rtn;
	}

	//indicates if grid cell with given location is occupied (by another grid cell)
	public static bool cellOccupied(int[] loc){
		bool rtn = false;
		foreach (int[] cellLoc in pipedCells) {
			if((cellLoc[0]==loc[0])&&(cellLoc[1]==loc[1])){
				rtn = true;
			}
		}
		return rtn;
	}

	public static bool cellOccupiedP2(int[] loc){
		bool rtn = false;
		foreach (int[] cellLoc in pipedCellsP2) {
			if((cellLoc[0]==loc[0])&&(cellLoc[1]==loc[1])){
				rtn = true;
			}
		}
		return rtn;
	}

	public static void registerSnapGrid(SnapGrid x,int player){
		if (player == 1) {
			sgs.Add (x);
		} else {
			sgsP2.Add (x);
		}
	}



	//adds this segment's location to sets. Merges connected components or creates a new one
	public static void addSegment(int[] loc,int player){
		if (player == 1) {
			pipedCells.Add (loc);
		} else {
			pipedCellsP2.Add (loc);
		}

		//extract all neighbor locs
		int[] n1 = new int[2];
		n1 [0] = loc [0] + 1;
		n1 [1] = loc [1];
		int[] n2 = new int[2];
		n2 [0] = loc [0] - 1;
		n2 [1] = loc [1];
		int[] n3 = new int[2];
		n3 [1] = loc [1] + 1;
		n3 [0] = loc [0];
		int[] n4 = new int[2];
		n4 [1] = loc [1] - 1;
		n4 [0] = loc [0];

		if (player == 1) {
			if (cellOccupied (n1)) {
				mergeCellsByLoc (n1, loc);
			}
			if (cellOccupied (n2)) {
				mergeCellsByLoc (n2, loc);
			}
			if (cellOccupied (n3)) {
				mergeCellsByLoc (n3, loc);
			}
			if (cellOccupied (n4)) {
				mergeCellsByLoc (n4, loc);
			}
		} else {
			if (cellOccupiedP2 (n1)) {
				mergeCellsByLocP2 (n1, loc);
			}
			if (cellOccupiedP2 (n2)) {
				mergeCellsByLocP2 (n2, loc);
			}
			if (cellOccupiedP2 (n3)) {
				mergeCellsByLocP2 (n3, loc);
			}
			if (cellOccupiedP2 (n4)) {
				mergeCellsByLocP2 (n4, loc);
			}
		}


	}

	static void mergeCellsByLoc(int[] a, int[] b){
		SnapGrid aSG = locToSG (a);
		SnapGrid bSG = locToSG (b);
		mergeCells (aSG, bSG);
	
	}

	static void mergeCellsByLocP2(int[] a, int[] b){
		SnapGrid aSG = locToSGP2 (a);
		SnapGrid bSG = locToSGP2 (b);
		mergeCells (aSG, bSG);
		
	}

	static SnapGrid locToSG(int[] loc){
		foreach (SnapGrid sg in sgs) {
			if((sg.finalPosition[0]==loc[0])&&(sg.finalPosition[1]==loc[1])){
				return sg;
			}
		}
		return null;
	}

	static SnapGrid locToSGP2(int[] loc){
		foreach (SnapGrid sg in sgsP2) {
			if((sg.finalPosition[0]==loc[0])&&(sg.finalPosition[1]==loc[1])){
				return sg;
			}
		}
		return null;
	}

	//links a and b to each other
	static void mergeCells(SnapGrid a, SnapGrid b){
		mergeCellsDir (a, b);
		mergeCellsDir (b, a);
	}

	//links a to b (not viceversa)
	static void mergeCellsDir(SnapGrid a, SnapGrid b){
		if (a.n1 == null) {
			a.n1 = b;
			return;
		}
		if (a.n2 == null) {
			a.n2 = b;
			return;
		}
		if (a.n3 == null) {
			a.n3 = b;
			return;
		}
		if (a.n4 == null) {
			a.n4 = b;
			return;
		}
	}
	//do DFS from s to get list of all reachable SnapGrids. Change material to green color + signal that selected (meaning specific coin can teleport there)
	public static void selectReachablePipeSegs(SnapGrid s){
		List<SnapGrid> explored = new List<SnapGrid>();
		Stack<SnapGrid> stack = new Stack<SnapGrid> ();
		stack.Push (s);
		while (stack.Count > 0) {
			SnapGrid n = stack.Pop();
			bool nWasExplored = false;
			foreach(SnapGrid sg in explored){
				if ((sg.finalPosition[0] == n.finalPosition[0]) && (sg.finalPosition[1] == n.finalPosition[1]) ){
					nWasExplored = true;
				}
			}
			if (!nWasExplored){
				explored.Add(n);
				if (n.n1!=null){
					stack.Push(n.n1);
				}
				if (n.n2!=null){
					stack.Push(n.n2);
				}
				if (n.n3!=null){
					stack.Push(n.n3);
				}
				if (n.n4!=null){
					stack.Push(n.n4);
				}
			}


		}
		//idea is that now explored is all the reachable SnapGrids--so same connected component
		foreach (SnapGrid sg in explored) {
			sg.gameObject.GetComponent<Renderer>().material.color = Color.green;
			sg.selected = true;
		}
	}

}
