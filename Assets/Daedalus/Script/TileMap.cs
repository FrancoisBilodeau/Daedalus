using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{

    PlayModeSingleton playMode = PlayModeSingleton.Instance;
    float tileSize;
    int wallHeight;
    int size_x; 
    int size_z;
    public Maze maze;

    //texture coord
    public const float floorA = 2f / 3000f, floorB = 998f / 3000f, floorC = 3002f / 4000f, floorD = 3998f / 4000f;
    public const float roofA = 3f / 3000f, roofB = 997f / 3000f, roofC = 1903f / 4000f, roofD = 2897f / 4000f;
    public const float wallA = 2002f / 3000f, wallB = 2998f / 3000f, wallC = 2002f / 4000f, wallD = 3998f / 4000f;
    public const float startA = 2f / 3000f, startB = 998f / 3000f, startC = 802f / 4000f, startD = 1798f / 4000f;
    public const float endA = 2002f / 3000f, endB = 2998f / 3000f, endC = 802f / 4000f, endD = 1798f / 4000f;
	
    void Start()
    {
        tileSize = playMode.TileSize;
        wallHeight = playMode.WallHeight;
        BuildMesh();
    }
    
    public void BuildMesh()
    {
        maze = new Maze(playMode.SaveFile.Maze);
        maze = AddBorder(maze);

        size_x = (int)maze.sizeX;
        size_z = (int)maze.sizeZ;
		
        int numTiles = size_x * size_z;
        int numWall = NumTrisMaze(maze);
        int numTris = (numTiles + numWall) * 2;

        int numVerts = (numTiles + numWall) * 4;
		
        // Generate the mesh data
        var vertices = new Vector3[ numVerts ];
        var normals = new Vector3[numVerts];
        var uv = new Vector2[numVerts];
		
        var triangles = new int[numTris * 3];
		
        // Add Floor 
        for (int i=0; i<size_x; ++i)
        {
            for (int j=0; j<size_z; ++j)
            {
                int squareIndex = i * size_z + j;
                int mur = 0;

                // Applique la bonne texture au carré 
                if (maze.mapData [i, j])
                {
                    mur = wallHeight;
                    uv [squareIndex * 4] = new Vector2(roofA, roofC); 
                    uv [(squareIndex * 4) + 1] = new Vector2(roofA, roofD); 
                    uv [(squareIndex * 4) + 2] = new Vector2(roofB, roofC); 
                    uv [(squareIndex * 4) + 3] = new Vector2(roofB, roofD);
                }
                else
                {
                    if (System.Math.Abs(maze.start.x - i) < 0.001 && System.Math.Abs(maze.start.y - j) < 0.001)
                    {
                        uv [squareIndex * 4] = new Vector2(startA, startC); 
                        uv [(squareIndex * 4) + 1] = new Vector2(startA, startD); 
                        uv [(squareIndex * 4) + 2] = new Vector2(startB, startC); 
                        uv [(squareIndex * 4) + 3] = new Vector2(startB, startD);
                    }
                    else if (System.Math.Abs(maze.end.x - i) < 0.001 && System.Math.Abs(maze.end.y - j) < 0.001)
                    {
                        uv [squareIndex * 4] = new Vector2(endA, endC); 
                        uv [(squareIndex * 4) + 1] = new Vector2(endA, endD); 
                        uv [(squareIndex * 4) + 2] = new Vector2(endB, endC); 
                        uv [(squareIndex * 4) + 3] = new Vector2(endB, endD);
                    }
                    else
                    {
                        uv [squareIndex * 4] = new Vector2(floorA, floorC); 
                        uv [(squareIndex * 4) + 1] = new Vector2(floorA, floorD); 
                        uv [(squareIndex * 4) + 2] = new Vector2(floorB, floorC); 
                        uv [(squareIndex * 4) + 3] = new Vector2(floorB, floorD);
                    }
                }

                vertices [squareIndex * 4] = new Vector3(j * tileSize, mur, i * tileSize);
                normals [squareIndex * 4] = Vector3.up;
				
                vertices [(squareIndex * 4) + 1] = new Vector3((j + 1) * tileSize, mur, (i) * tileSize);
                normals [(squareIndex * 4) + 1] = Vector3.up;
				
                vertices [(squareIndex * 4) + 2] = new Vector3(j * tileSize, mur, (i + 1) * tileSize);
                normals [(squareIndex * 4) + 2] = Vector3.up;
				
                vertices [(squareIndex * 4) + 3] = new Vector3((j + 1) * tileSize, mur, (i + 1) * tileSize);
                normals [(squareIndex * 4) + 3] = Vector3.up;

                int triOffset = squareIndex * 6;
				
                triangles [triOffset + 0] = squareIndex * 4;
                triangles [triOffset + 1] = squareIndex * 4 + 3;
                triangles [triOffset + 2] = squareIndex * 4 + 1;
				
                triangles [triOffset + 3] = squareIndex * 4;
                triangles [triOffset + 4] = squareIndex * 4 + 2;
                triangles [triOffset + 5] = squareIndex * 4 + 3;
            }
        }
		
        // add Wall  
        int k = numTris / 2 - numWall;
        int l = numVerts - (numWall * 4);
        for (int i = 0; i < size_x && k < numTris/2; ++i)
        {
            for (int j = 0; j < size_z && k < numTris/2; ++j)
            {
                if (i < size_x - 1 && maze.mapData [i + 1, j] != maze.mapData [i, j])
                {
                    int triOffset = k * 6;
                    k++;

                    vertices [l] = vertices [(i * size_z + j) * 4 + 2];
                    normals [l] = Vector3.up;
                    uv [l] = new Vector2(wallA, wallC);

                    vertices [l + 1] = vertices [((i + 1) * size_z + j) * 4];
                    normals [l + 1] = Vector3.up;
                    uv [l + 1] = new Vector2(wallA, wallD);

                    vertices [l + 2] = vertices [(i * size_z + j) * 4 + 3];
                    normals [l + 2] = Vector3.up;
                    uv [l + 2] = new Vector2(wallB, wallC);

                    vertices [l + 3] = vertices [((i + 1) * size_z + j) * 4 + 1];
                    normals [l + 3] = Vector3.up;
                    uv [l + 3] = new Vector2(wallB, wallD);

                    triangles [triOffset + 0] = l;
                    triangles [triOffset + 1] = l + 1;
                    triangles [triOffset + 2] = l + 3;

                    triangles [triOffset + 3] = l;
                    triangles [triOffset + 4] = l + 3;
                    triangles [triOffset + 5] = l + 2;

                    l += 4;
                }
                if (j < size_z - 1 && maze.mapData [i, j + 1] != maze.mapData [i, j])
                {
                    int triOffset = k * 6;
                    k++;

                    vertices [l] = vertices [(i * size_z + j) * 4 + 1];
                    normals [l] = Vector3.up;
                    uv [l] = new Vector2(wallA, wallC);
					
                    vertices [l + 1] = vertices [(i * size_z + j + 1) * 4 + 0];
                    normals [l + 1] = Vector3.up;
                    uv [l + 1] = new Vector2(wallA, wallD);
					
                    vertices [l + 2] = vertices [(i * size_z + j) * 4 + 3];
                    normals [l + 2] = Vector3.up;
                    uv [l + 2] = new Vector2(wallB, wallC);
					
                    vertices [l + 3] = vertices [(i * size_z + j + 1) * 4 + 2];
                    normals [l + 3] = Vector3.up;
                    uv [l + 3] = new Vector2(wallB, wallD);
					
                    triangles [triOffset + 0] = l;
                    triangles [triOffset + 1] = l + 3;
                    triangles [triOffset + 2] = l + 1;
					
                    triangles [triOffset + 3] = l;
                    triangles [triOffset + 4] = l + 2;
                    triangles [triOffset + 5] = l + 3;

                    l += 4;
                }
            }
        }
		
        // Create a new Mesh and populate with the data
        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
		
        // Assign our mesh to our filter/renderer/collider
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
		
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
	
    int NumTrisMaze(Maze theMaze)
    {
        int numTrisTemp = 0;
        bool[,] mazeData = theMaze.mapData;
		
        for (int i = 0; i < size_x; ++i)
        {
            for (int j = 0; j < size_z; ++j)
            {
                if (i < size_x - 1 && mazeData [i + 1, j] != mazeData [i, j])
                {
                    numTrisTemp++;
                }
                if (j < size_z - 1 && mazeData [i, j + 1] != mazeData [i, j])
                {
                    numTrisTemp++;
                }
            }
        }
        return numTrisTemp;
    }

    Maze AddBorder(Maze theMaze)
    {
        theMaze.sizeX += 2;
        theMaze.sizeZ += 2;

        var mazeData = new bool[theMaze.sizeX, theMaze.sizeZ];
        for (int i = 1; i < theMaze.sizeX - 1; i++)
        {
            for (int j = 1; j < theMaze.sizeZ-1; j++)
            {
                mazeData [i, j] = theMaze.mapData [i - 1, j - 1];
            }
        }
        for (int i = 0; i < theMaze.sizeX; i++)
        {
            mazeData [i, 0] = true;
            mazeData [i, theMaze.sizeZ - 1] = true;
        }
        for (int j = 0; j < theMaze.sizeZ; j++)
        {
            mazeData [0, j] = true;
            mazeData [theMaze.sizeX - 1, j] = true;
        }

        theMaze.mapData = mazeData;

        theMaze.start.x++; 
        theMaze.start.y++; 
        theMaze.end.x++; 
        theMaze.end.y++; 

        return theMaze;
    }
	
}
