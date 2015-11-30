using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class levelBuild : MonoBehaviour {

	//a list of pre-defined positions that will be raised in the terrain
	public static List<int[]> filledCells;

	public TerrainData terrData;
	int xRes;
	int yRes;

	float raisedHeight = .65f;

	// Use this for initialization
	void Awake() {
		//get Terrain heightmap width and height
		xRes = terrData.heightmapWidth;
		yRes = terrData.heightmapHeight;

		populateFilledCells ();
		/*
		foreach (int[] c in filledCells) {
			Debug.Log(c[0] + ", " + c[1]);
		}
		*/
		flattenTerr ();
		buildTerr ();
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	//so takes in heights. And raises a cell in it. Also x,y are "switched"
	float [,] raiseCell(int[] cellLoc,float[,] heights){
		float xStep = xRes/15.0f; //the switch
		float yStep = yRes/20.0f; //"        "
		int xStart = (int)(cellLoc [0]*xStep);
		int yStart = (int)(cellLoc [1]*yStep);
		//Debug.Log ("xStart: " + xStart + " xStep: " + xStep);
		//Debug.Log ("yStart: " + yStart + " yStep: " + yStep);
		
		for (int i = xStart; i < xStart + xStep; i++) {
			for (int j = yStart; j < yStart + yStep; j++) {
				heights[i,j] = raisedHeight;
			}	
		}
		
		return heights;
	}

	void buildTerr(){
		float[,] heights = terrData.GetHeights (0,0,xRes,yRes);

		foreach (int[] cellLoc in filledCells) {
			int[] correctedSwappedCellLoc = new int[2];
			correctedSwappedCellLoc[0] = cellLoc[1];
			correctedSwappedCellLoc[1] = cellLoc[0];
			heights = raiseCell(correctedSwappedCellLoc, heights);
		}
		
		
		
		terrData.SetHeights(0,0,heights);
	}



	//reset/flatten terrain at beginning
	void flattenTerr(){

		float[,] initHeights = new float[xRes,yRes];
		for (int i = 0; i < xRes; i++) {
			for (int j = 0; j < yRes; j++) {
				initHeights[i,j] = 0;
			}	
		}
		terrData.SetHeights(0,0,initHeights);
	}

	//loads filledCells with list of cells to raise
	void populateFilledCells(){
		filledCells = new List<int[]>();

		for (int i = 6; i<9; i++) {
			for (int j = 5; j<7; j++) {//j from 3
				int[] add1 = new int[2];
				add1[0] = j;
				add1[1] = i;
				filledCells.Add(add1);
				int[] add2 = new int[2];
				add2[0] = j+8;//j + 10
				add2[1] = i;
				filledCells.Add(add2);
			}
		}

		for (int i = 2; i<4; i++) {
			for (int j = 3; j<6; j++) {
				int[] add1 = new int[2];
				add1[0] = j;
				add1[1] = i;
				filledCells.Add(add1);
				int[] add2 = new int[2];
				add2[0] = j;
				add2[1] = i+9;
				filledCells.Add(add2);

				int[] add3 = new int[2];
				add3[0] = j+11;
				add3[1] = i+9;
				filledCells.Add(add3);

				int[] add4 = new int[2];
				add4[0] = j+11;
				add4[1] = i;
				filledCells.Add(add4);

			}
		}
	}

	void OnDestroy(){
		flattenTerr ();
	}
}
