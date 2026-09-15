using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HighPolyWaterGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [Tooltip("Размер сетки в метрах (юнитах Unity)")]
    public float gridSize = 50f;
    
    [Tooltip("Количество полигонов на сторону. Чем выше, тем детальнее волна.")]
    public int gridResolution = 200;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Water Grid";
        
        // Включение 32-битных индексов для поддержки сеток более чем на 65535 вершин
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        int numVertices = (gridResolution + 1) * (gridResolution + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[gridResolution * gridResolution * 6];

        float step = gridSize / gridResolution;
        float offset = gridSize / 2f;

        // Генерация вершин и UV-развертки
        int v = 0;
        for (int z = 0; z <= gridResolution; z++)
        {
            for (int x = 0; x <= gridResolution; x++)
            {
                vertices[v] = new Vector3(x * step - offset, 0, z * step - offset);
                uvs[v] = new Vector2((float)x / gridResolution, (float)z / gridResolution);
                v++;
            }
        }

        // Генерация треугольников (индексов)
        int t = 0;
        int vert = 0;
        for (int z = 0; z < gridResolution; z++)
        {
            for (int x = 0; x < gridResolution; x++)
            {
                triangles[t + 0] = vert + 0;
                triangles[t + 1] = vert + gridResolution + 1;
                triangles[t + 2] = vert + 1;
                
                triangles[t + 3] = vert + 1;
                triangles[t + 4] = vert + gridResolution + 1;
                triangles[t + 5] = vert + gridResolution + 2;

                vert++;
                t += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
        // Задаем нормали, направленные строго вверх (шейдер пересчитает их сам)
        mesh.RecalculateNormals(); 

        GetComponent<MeshFilter>().mesh = mesh;
    }
}