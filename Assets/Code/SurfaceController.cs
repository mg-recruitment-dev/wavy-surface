using UnityEngine;

public class SurfaceController : MonoBehaviour
{
    private const float MIN_TILE_SIZE = 0.01f;
    private const int INDICES_IN_TRIANGLE = 3;
    private const int TRIANGLES_IN_TILE = 2;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Vector2 meshSize = new Vector2( 30, 20 );
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private Color defaultVertexColor = new Color( 0.851f, 0.894f, 0.984f );

    private Mesh generatedMesh = null;

    private void Awake()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        if ( meshSize.x <= 0 || meshSize.y <= 0 )
        {
            Debug.LogError( "Mesh dimensions have to be positive value - mesh won't be generated" );
            return;
        }

        if ( tileSize < MIN_TILE_SIZE )
        {
            Debug.LogError( $"Tile size shouldn't be smaller than {MIN_TILE_SIZE} - mesh won't be generated." );
            return;
        }

        int tilesX = (int)(meshSize.x / tileSize);
        int tilesZ = (int)(meshSize.y / tileSize);

        var vertices = GetVerticesForGrid( tilesX, tilesZ, out var colors );
        var triangles = GetTrianglesForGrid( tilesX, tilesZ );

        generatedMesh = new Mesh();
        generatedMesh.name = "Surface";
        generatedMesh.vertices = vertices;
        generatedMesh.colors = colors;
        generatedMesh.triangles = triangles;
        generatedMesh.RecalculateNormals();

        meshFilter.mesh = generatedMesh;
    }

    private Vector3[] GetVerticesForGrid( int tilesX, int tilesZ, out Color[] colors )
    {
        var vertices = new Vector3[(tilesX + 1) * (tilesZ + 1)];
        colors = new Color[vertices.Length];

        float positionX = 0f;
        float positionZ = 0f;

        for ( int z = 0; z < tilesZ + 1; z++ )
        {
            for ( int x = 0; x < tilesX + 1; x++ )
            {
                int currentIndex = (z * (tilesX + 1)) + x;
                vertices[currentIndex] = new Vector3( positionX, 0, positionZ );
                colors[currentIndex] = defaultVertexColor;

                positionX += tileSize;
            }

            positionX = 0f;
            positionZ += tileSize;
        }

        return vertices;
    }

    private int[] GetTrianglesForGrid( int tilesX, int tilesZ )
    {
        var indicesPerTile = INDICES_IN_TRIANGLE * TRIANGLES_IN_TILE;
        var triangles = new int[indicesPerTile * tilesX * tilesZ];
        int ti = 0;

        for ( int z = 0; z < tilesZ; z++ )
        {
            for ( int x = 0; x < tilesX; x++ )
            {
                // First half of a tile
                triangles[ti] = z * (tilesX + 1) + x;
                triangles[ti + 1] = ((z + 1) * (tilesX + 1)) + x;
                triangles[ti + 2] = triangles[ti] + 1;

                // Second half of a tile
                triangles[ti + 3] = triangles[ti + 1];
                triangles[ti + 4] = triangles[ti + 1] + 1;
                triangles[ti + 5] = triangles[ti + 2];

                ti += indicesPerTile;
            }
        }

        return triangles;
    }
}
