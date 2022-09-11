using UnityEngine;
using static UnityEngine.Mathf;

public class SurfaceController : MonoBehaviour
{
    private const float MIN_TILE_SIZE = 0.01f;
    private const int INDICES_IN_TRIANGLE = 3;
    private const int TRIANGLES_IN_TILE = 2;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Vector2 meshSize = new Vector2( 30, 20 );
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private Color defaultVertexColor = new Color( 0.851f, 0.894f, 0.984f );
    [SerializeField] private bool animate = true;
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private Material wireframeMaterial;
    [SerializeField] private Material shadedMaterial;

    private Mesh generatedMesh = null;
    private float timer = 0f;
    private int tilesX;
    private int tilesZ;

    private void Awake()
    {
        meshRenderer.sharedMaterial = wireframeMaterial;
        GenerateMesh();
    }

    private void Update()
    {
        AnimateMesh();
        HandleSwitchingMaterials();
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

        tilesX = (int)(meshSize.x / tileSize);
        tilesZ = (int)(meshSize.y / tileSize);

        var vertices = GetVerticesForGrid();
        var colors = GetColorsForGrid();
        var triangles = GetTrianglesForGrid();

        generatedMesh = new Mesh();
        generatedMesh.name = "Surface";
        generatedMesh.vertices = vertices;
        generatedMesh.colors = colors;
        generatedMesh.triangles = triangles;
        generatedMesh.RecalculateNormals();

        meshFilter.mesh = generatedMesh;
    }

    private void AnimateMesh()
    {
        if ( !animate )
            return;

        timer += Time.deltaTime;
        var vertices = generatedMesh.vertices;

        for ( int i = 0; i < vertices.Length; i++ )
        {
            var currentVertex = vertices[i];
            float newY = GetIndexPositionY( currentVertex.x, currentVertex.z, timer );
            currentVertex.y = newY;

            vertices[i] = currentVertex;
        }

        generatedMesh.vertices = vertices;
        generatedMesh.RecalculateNormals();
    }

    private void HandleSwitchingMaterials()
    {
        if ( !Input.GetKeyDown( KeyCode.S ) )
            return;

        if ( meshRenderer.sharedMaterial == wireframeMaterial )
            meshRenderer.sharedMaterial = shadedMaterial;
        else
            meshRenderer.sharedMaterial = wireframeMaterial;

        generatedMesh.colors = GetColorsForGrid();
    }

    private Vector3[] GetVerticesForGrid()
    {
        var vertices = new Vector3[(tilesX + 1) * (tilesZ + 1)];

        float positionX = 0f;
        float positionZ = 0f;

        for ( int z = 0; z < tilesZ + 1; z++ )
        {
            for ( int x = 0; x < tilesX + 1; x++ )
            {
                int currentIndex = (z * (tilesX + 1)) + x;
                float positionY = GetIndexPositionY( x, z, timer );

                vertices[currentIndex] = new Vector3( positionX, positionY, positionZ );
                positionX += tileSize;
            }

            positionX = 0f;
            positionZ += tileSize;
        }

        return vertices;
    }

    private float GetIndexPositionY( float x, float z, float t )
    {
        float length = meshSize.x;
        float width = meshSize.y;
        float y = 3 * Sin( PI * (x / length + z / width + (t * animationSpeed)) );
        return y;
    }

    private Color [] GetColorsForGrid()
    {
        var colors = new Color[(tilesX + 1) * (tilesZ + 1)];

        for ( int z = 0; z < tilesZ + 1; z++ )
        {
            for ( int x = 0; x < tilesX + 1; x++ )
            {
                int currentIndex = (z * (tilesX + 1)) + x;
                colors[currentIndex] = GetColorForVertex( x, z );
            }
        }

        return colors;
    }

    private int[] GetTrianglesForGrid()
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

    private Color GetColorForVertex( int x, int z )
    {
        if ( meshRenderer.sharedMaterial == shadedMaterial )
            return defaultVertexColor;

        var color = Color.black;

        if ( z % 3 == 0 )
            color = Color.red;
        else if ( z % 3 == 1 )
            color = Color.green;
        else
            color = Color.blue;

        int offset = x % 3;

        while ( offset > 0 )
        {
            color = GetNextColorInRow( color );
            offset--;
        }

        return color;
    }

    private Color GetNextColorInRow( Color color )
    {
        if ( color == Color.red )
            return Color.blue;
        if ( color == Color.blue )
            return Color.green;

        return Color.red;
    }
}
