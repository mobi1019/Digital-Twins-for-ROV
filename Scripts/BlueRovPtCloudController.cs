// This controller processes point cloud data
using System;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;
using PointCloud;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BlueRovPtCloudController : MonoBehaviour
{

    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;

    public float pointSize = 2f;

    public static List<Vector3> pcl_interpolated_list;
    public static Vector3[] pcl_interpolated;
    
    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };


    public Transform offset;

    void Start()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        meshRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        mesh = new Mesh
        {
            //  32 bit integer values used for the mesh, allows for more vertices 
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        transform.position = offset.position;
        transform.rotation = offset.rotation;
    }

    void UpdateMesh(){
        
        positions = BlueRovCloudSubscriber.pcl;


        if (positions == null)
        {
            // Debug.Log("no meshes");
            // return;
        } else {
            mesh.Clear();
            mesh.vertices = positions;
            int[] indices = new int[positions.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                indices[i] = i;
            }

            mesh.SetIndices(indices, MeshTopology.Points, 0);
            mf.mesh = mesh;

            // tests for time/ latency 
            // long secs = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            // Debug.Log("Time after drawing point clouds in milliseconds = " + secs);

        }
        


    }

    void Update()
    {
        transform.position = offset.position;
        transform.rotation = offset.rotation;
        meshRenderer.material.SetFloat("_PointSize", pointSize);       
        UpdateMesh();
    }
   
}

