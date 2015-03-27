using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMapCreator : MonoBehaviour {

	private CreatorModeSingleton creatorMode = CreatorModeSingleton.Instance;
	private float tileSize;
	private int wallHeight;
	private int size_x; 
	private int size_z;
	public Maze maze;

  //texture coord
	public const float floorA = 2f/3000f, floorB =998f/3000f, floorC = 3002f/4000f, floorD = 3998f/4000f;
	public const float roofA = 20f/3000f, roofB = 998f/3000f, roofC = 1902f/4000f, roofD = 2898f/4000f;
	public const float wallA = 2002f/3000f, wallB = 2998f/3000f, wallC = 2002f/4000f, wallD = 3998f/4000f;
	public const float startA = 2f/3000f, startB = 998f/3000f, startC = 802f/4000f, startD = 1798f/4000f;
	public const float endA = 2002f/3000f, endB = 2998f/3000f,  endC = 802f/4000f, endD = 1798f/4000f;
	// Use this for initialization
	void Start () {
		tileSize = creatorMode.getTileSize();
		wallHeight = creatorMode.getWallHeight();
		BuildMesh();
	}
    
	public void BuildMesh() {
		maze = new Maze(creatorMode.getMaze());
		maze = addBorder(maze);

		
		size_x = (int)maze.sizeX;
		size_z = (int)maze.sizeZ;
		
		int numTiles = size_x * size_z;
		int numWall = numTrisMaze(maze);
		int numTris = (numTiles + numWall) * 2;

		int numVerts = (numTiles + numWall) * 4;
		
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3];
		
		
		// Add Floor 
		for(int i=0; i<size_x; ++i){
			for(int j=0; j<size_z; ++j){
				int squareIndex = i * size_z + j;
				int mur = 0;

				//Applique la Bonne texture au carrée 
				if(maze.mapData[i,j]){
					mur = wallHeight;
					uv [squareIndex*4] = new Vector2( roofA, roofC ); 
					uv[(squareIndex*4)+1] = new Vector2( roofA, roofD ); 
					uv[(squareIndex*4)+2] = new Vector2( roofB, roofC ); 
					uv[(squareIndex*4)+3] = new Vector2( roofB, roofD );
				}else{
					if(maze.start.x == i && maze.start.y == j){
						uv [squareIndex*4] = new Vector2( startA, startC ); 
						uv[(squareIndex*4)+1] = new Vector2( startA, startD ); 
						uv[(squareIndex*4)+2] = new Vector2( startB, startC ); 
						uv[(squareIndex*4)+3] = new Vector2( startB, startD );
					}else if (maze.end.x == i && maze.end.y == j){
						uv [squareIndex*4] = new Vector2( endA, endC ); 
						uv[(squareIndex*4)+1] = new Vector2( endA, endD ); 
						uv[(squareIndex*4)+2] = new Vector2( endB, endC ); 
						uv[(squareIndex*4)+3] = new Vector2( endB, endD );
					}else{
						uv [squareIndex*4] = new Vector2( floorA, floorC ); 
						uv[(squareIndex*4)+1] = new Vector2( floorA, floorD ); 
						uv[(squareIndex*4)+2] = new Vector2( floorB, floorC ); 
						uv[(squareIndex*4)+3] = new Vector2( floorB, floorD );
					}
				}

				vertices[squareIndex*4] = new Vector3(j*tileSize, mur, i*tileSize);
				normals[squareIndex*4] = Vector3.up;
				
				vertices[(squareIndex*4)+1] = new Vector3((j+1)*tileSize, mur, (i)*tileSize);
				normals[(squareIndex*4)+1] = Vector3.up;
				
				vertices[(squareIndex*4)+2] = new Vector3(j*tileSize, mur, (i+1)*tileSize);
				normals[(squareIndex*4)+2] = Vector3.up;
				
				vertices[(squareIndex*4)+3] = new Vector3((j+1)*tileSize, mur, (i+1)*tileSize);
				normals[(squareIndex*4)+3] = Vector3.up;

				
				
				int triOffset = squareIndex * 6;
				
				triangles[triOffset + 0] = squareIndex*4  ;
				triangles[triOffset + 1] = squareIndex*4 +3;
				triangles[triOffset + 2] = squareIndex*4 +1;
				
				triangles[triOffset + 3] = squareIndex*4;
				triangles[triOffset + 4] = squareIndex*4 + 2;
				triangles[triOffset + 5] = squareIndex*4 + 3;
			}
		}
		
		
		// add Wall  
		int k = numTris/2 - numWall;
		int l = numVerts - (numWall*4);
		for(int i = 0; i < size_x && k < numTris/2; ++i){
			for(int j = 0; j < size_z && k < numTris/2; ++j){
				if( i < size_x-1 && maze.mapData[i+1,j] != maze.mapData[i,j] ){
					int triOffset = k * 6;
					k++;

					vertices[l] = vertices[(i * size_z + j)*4 + 2];
					normals[l] = Vector3.up;
					uv [l] = new Vector2( wallA, wallC );

					vertices[l+1] = vertices[((i+1) * size_z + j)*4 ];
					normals[l+1] = Vector3.up;
					uv [l+1] = new Vector2( wallA, wallD );

					vertices[l+2] = vertices[(i * size_z + j)*4 +3];
					normals[l+2] = Vector3.up;
					uv [l+2] = new Vector2( wallB, wallC );

					vertices[l+3] = vertices[((i+1) * size_z + j)*4 + 1];
					normals[l+3] = Vector3.up;
					uv [l+3] = new Vector2( wallB, wallD );

					triangles[triOffset + 0] = l;
					triangles[triOffset + 1] = l+1;
					triangles[triOffset + 2] = l+3;

					triangles[triOffset + 3] = l;
					triangles[triOffset + 4] = l+3;
					triangles[triOffset + 5] = l+2;

					l+=4;
				}
				if( j < size_z-1 && maze.mapData[i,j+1] != maze.mapData[i,j] ){
					int triOffset = k * 6;
					k++;

					vertices[l] = vertices[(i * size_z + j)*4 + 1];
					normals[l] = Vector3.up;
					uv [l] = new Vector2( wallA, wallC );
					
					vertices[l+1] = vertices[(i * size_z + j+1)*4 +0];
					normals[l+1] = Vector3.up;
					uv [l+1] = new Vector2( wallA, wallD );
					
					vertices[l+2] = vertices[(i * size_z + j)*4 + 3 ];
					normals[l+2] = Vector3.up;
					uv [l+2] = new Vector2( wallB, wallC );
					
					vertices[l+3] = vertices[(i * size_z + j+1)*4 + 2];
					normals[l+3] = Vector3.up;
					uv [l+3] = new Vector2( wallB, wallD );
					
					triangles[triOffset + 0] = l;
					triangles[triOffset + 1] = l+3;
					triangles[triOffset + 2] = l+1;
					
					triangles[triOffset + 3] = l;
					triangles[triOffset + 4] = l+2;
					triangles[triOffset + 5] = l+3;

					l+=4;
				}
			}
		}

		// Create a new Mesh and populate with the data
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		Mesh mesh = mesh_filter.sharedMesh;
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		mesh_filter.sharedMesh = mesh;
		
	}
	
	private int numTrisMaze(Maze maze){
		int numTrisTemp = 0;
		bool[,] mazeData = maze.mapData;
		
		for(int i = 0; i < size_x; ++i){
			for(int j = 0; j<size_z; ++j){
				if( i < size_x-1 && mazeData[i+1,j] != mazeData[i,j] ){
					numTrisTemp++;
				}
				if( j < size_z-1 && mazeData[i,j+1] != mazeData[i,j] ){
					numTrisTemp++;
				}
			}
		}
		return numTrisTemp;
	}

	private Maze addBorder(Maze maze){
		maze.sizeX += 2;
		maze.sizeZ += 2;

		bool[,] mazeData = new bool[maze.sizeX,maze.sizeZ];
		for(int i = 1; i < maze.sizeX - 1; i++){
			for(int j = 1; j < maze.sizeZ-1; j++){
				mazeData[i,j] = maze.mapData[i-1,j-1];
			}
		}
		for(int i = 0; i < maze.sizeX; i++){
			mazeData[i,0] = true;
			mazeData[i,maze.sizeZ -1] = true;
		}
		for(int j = 0; j < maze.sizeZ; j++){
			mazeData[0,j] = true;
			mazeData[maze.sizeX-1,j] = true;
		}

		maze.mapData = mazeData;

		maze.start.x++; 
		maze.start.y++; 
		maze.end.x++; 
		maze.end.y++; 

		return maze;

	}
	
}
