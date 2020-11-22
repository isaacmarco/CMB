using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreadorTerreno : MonoBehaviour
{
    private GameObject terreno;

    [Header("Configuracion")]
    [SerializeField] private Material materialParaTerreno; 
    [SerializeField] private Texture2D mapaDeAltura; 
    public MeshFilter meshfilter;


    private void CrearMallaDelTerreno()
    {

        terreno = new GameObject();
        MeshFilter meshFilter = (MeshFilter)terreno.AddComponent(typeof(MeshFilter));
        terreno.AddComponent(typeof(MeshRenderer));
        Mesh m = new Mesh();
        int size = 300; 
        int widthSegments = size; 
        int lengthSegments = size; 
        float width = size; float length = size; 

        int hCount2 = widthSegments+1;
        int vCount2 = lengthSegments+1;
        int numTriangles = widthSegments * lengthSegments * 6;        
        int numVertices = hCount2 * vCount2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles];
        Vector4[] tangents = new Vector4[numVertices];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        
        int index = 0;
        float uvFactorX = 1.0f/widthSegments;
        float uvFactorY = 1.0f/lengthSegments;
        float scaleX = width/widthSegments;
        float scaleY = length/lengthSegments;
        for (float y = 0.0f; y < vCount2; y++)
        {
            for (float x = 0.0f; x < hCount2; x++)
            {                    
                vertices[index] = new Vector3(x*scaleX - width/2f, 0.0f, y*scaleY - length/2f );                
                tangents[index] = tangent;
                uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
            }
        }        

        index = 0;
        for (int y = 0; y < lengthSegments; y++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                triangles[index]   = (y     * hCount2) + x;
                triangles[index+1] = ((y+1) * hCount2) + x;
                triangles[index+2] = (y     * hCount2) + x + 1;
 
                triangles[index+3] = ((y+1) * hCount2) + x;
                triangles[index+4] = ((y+1) * hCount2) + x + 1;
                triangles[index+5] = (y     * hCount2) + x + 1;
                index += 6;
            }               
        }
 
        m.vertices = vertices;
        // m.uv = uvs;
        m.triangles = triangles;
        m.tangents = tangents;
        m.RecalculateNormals();
        meshFilter.sharedMesh = m;
        m.RecalculateBounds();
        //return terreno; 
        terreno.GetComponent<Renderer>().material = materialParaTerreno; 
    }
    
    
    void Start()
    {
        //CrearMallaDelTerreno();
        GenerarAlturaDelTerreno();
        //ColorearTerreno(); 
    }

    private void CrearRuta(GameObject terreno)
    {

    }

    private void GenerarAlturaDelTerreno()
    {
        Mesh mesh1 = meshfilter.sharedMesh; 

        Vector3[] vertices = mesh1.vertices;

        for(int i=0; i<vertices.Length; i++)
        {                       
            Matrix4x4 localToWorld = transform.localToWorldMatrix;
            Vector3 world_v = localToWorld.MultiplyPoint3x4(vertices[i]);
            int x = (int) world_v.x; 
            int y = (int) world_v.z * -1;            
            float factorAltura = 10f; 
            float altura = mapaDeAltura.GetPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y)).grayscale * factorAltura;            
            vertices[i].y = vertices[i].y + altura * factorAltura + Random.value/10f; 
        }

        mesh1.vertices = vertices; 
        mesh1.RecalculateBounds();
        mesh1.RecalculateNormals();
    }

    private Color AsignarColor(float altura)
    {
         // obtener el volumn del terreno
        Mesh mesh = terreno.GetComponent<Mesh>();
        mesh.RecalculateBounds();
        Bounds limites = mesh.bounds;
        float alturaMinima = 0;
        float alturaMaxima = limites.max.y;        
        // colorear cada vertice
        Color blanco = new Color32(255, 255, 255, 255);
        /*
        Color violeta = new Color32();
        Color verde = new Color32();
        Color amarillo = new Color32();
        Color azul = new Color32();
        */
        return new Color(1f, 1f, 1f);
    }

    private void ColorearTerreno()
    {

    }
    
    // Start is called before the first frame update
    void Start2()
    {
       

         
        if(terreno!=null && terreno.GetComponent<MeshFilter>().mesh!=null)
        {
            Mesh mesh = terreno.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

        for (var i = 0; i < vertices.Length; i++)
            vertices[i] = new Vector3(vertices[i].x, Random.value, vertices[i].z);

        mesh.vertices = vertices;


        //Process the triangles
         Vector3[] oldVerts = mesh.vertices;
         int[] triangles = mesh.triangles;
         Vector3[] vertices2 = new Vector3[triangles.Length];
         for (int i = 0; i < triangles.Length; i++) {
             vertices2[i] = oldVerts[triangles[i]];
             triangles[i] = i;
         }
         mesh.vertices = vertices2;
         mesh.triangles = triangles;
         mesh.RecalculateBounds();
         mesh.RecalculateNormals();
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
