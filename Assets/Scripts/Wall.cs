using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Vector3 brickSize = new Vector3(0.25f, 0.25f, 0.5f);

    [Range(0, 1000)]
    public float verticalForce = 500.0f;
    [Range(0, 1000)]
    public float verticalTorque = 500.0f;
    [Range(0, 1000)]
    public float horizontalForce = 500.0f;
    [Range(0, 1000)]
    public float horizontalTorque = 500.0f;
    [Range(0, 1000)]
    public float transverseForce = 500.0f;
    [Range(0, 1000)]
    public float transverseTorque = 500.0f;

    public Material[] brickMaterials;
    
    private BoxCollider boxCollider;

    void Start () {
        boxCollider = GetComponent<BoxCollider>();
        // SpawnWall(boxCollider, brickSize);
        SpawnDefensiveWall(boxCollider, brickSize);
    }

    public void SpawnDefensiveWall(BoxCollider boxCollider, Vector3 brickSize) {
        Vector3[] vertices = GetBoxColliderVertices(boxCollider);
        int thickness = Mathf.FloorToInt(Vector3.Distance(vertices[0], vertices[4]) / brickSize.x);
        BrickWallData[] brickWallDatas = new BrickWallData[thickness];

        for (int x = 0; x < thickness; x++) {
            Vector3 offset = new Vector3(brickSize.x * x, 0.0f, 0.0f);
            brickWallDatas[x] = SpawnBrickWall(new Vector3[4] { vertices[0]-offset, vertices[1]-offset, vertices[2]-offset, vertices[3]-offset}, brickSize);    
        }

        for (int x = 0; x < thickness-1; x++) {
            for (int y = 0; y < brickWallDatas[x].bricksInHeight; y++) {
                for (int z = 0; z < brickWallDatas[x].bricksInWidth; z++) {
                    FixedJoint transverse_joint = brickWallDatas[x].bricks[y,z].AddComponent<FixedJoint>();
                    transverse_joint.connectedBody = brickWallDatas[x+1].bricks[y,z].GetComponent<Rigidbody>();
                    transverse_joint.breakForce = transverseForce;
                    transverse_joint.breakTorque = transverseTorque;
                }
            }
        }
    }

    private BrickWallData SpawnBrickWall (Vector3[] vertices, Vector3 brickSize) {
        
        float width = Mathf.Abs(vertices[0].z - vertices[1].z);
        float height = Mathf.Abs(vertices[0].y - vertices[2].y);

        int bricksInHeight = Mathf.FloorToInt(height / brickSize.y);
        int bricksInWidth = Mathf.FloorToInt(width / brickSize.z);

        GameObject[,] bricks = new GameObject[bricksInHeight, bricksInWidth];
        BrickWallData wallData = new BrickWallData(bricksInHeight, bricksInWidth, brickSize, bricks);

        float z_offset;

        // bricks generation
        for (int y = 0; y < wallData.bricksInHeight; y++) {

            z_offset = (y % 2 == 0) ? 0.0f : brickSize.z / 2.0f; 

            for (int z = 0; z < wallData.bricksInWidth; z++) {
                Vector3 position = new Vector3(
                    vertices[0].x - brickSize.x/2.0f, 
                    vertices[0].y + brickSize.y/2.0f + brickSize.y * y,
                    vertices[0].z + brickSize.z/2.0f + brickSize.z * z + z_offset
                );

                wallData.bricks[y,z] = SpawnBrick(position, brickSize, transform);
            }
        }

        for (int y = 0; y < wallData.bricksInHeight-1; y++) {
            for (int z = 0; z < wallData.bricksInWidth; z++) {

                FixedJoint joint_vertical = wallData.bricks[y,z].AddComponent<FixedJoint>();
                joint_vertical.connectedBody = wallData.bricks[y+1, z].GetComponent<Rigidbody>();
                joint_vertical.breakForce = verticalForce;
                joint_vertical.breakTorque = verticalTorque;

                if (z < bricksInWidth-1) {
                    FixedJoint joint_horizontal = wallData.bricks[y,z].AddComponent<FixedJoint>();
                    joint_horizontal.connectedBody = wallData.bricks[y, z+1].GetComponent<Rigidbody>();
                    joint_horizontal.breakForce = horizontalForce;
                    joint_horizontal.breakTorque = horizontalTorque;
                }
            }
        }

        return wallData;
    }

    private GameObject SpawnBrick(Vector3 position, Vector3 size, Transform parent) {
        GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
        brick.tag = "Brick";
        brick.transform.localScale = size;
        brick.transform.position = position;
        brick.transform.SetParent(parent);

        Rigidbody brickRigidbody = brick.AddComponent<Rigidbody>();
        brickRigidbody.mass = 20.0f;
        brickRigidbody.isKinematic = true;

        brick.GetComponent<Renderer>().material = brickMaterials[Random.Range(0, brickMaterials.Length)];

        return brick;
    }

    private Vector3[] GetBoxColliderVertices(BoxCollider boxCollider) {
        Vector3[] vertices = new Vector3[8];
        Vector3 center = transform.position + boxCollider.center;

        // X Axis = Front 

        // Front wall, bottom line, upper line 
        // Bottom Right Front 
        vertices[0] = center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        // Bottom Left Front 
        vertices[1] = center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        // Upper Right Front 
        vertices[2] = center + new Vector3(boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        // Upper Left Front
        vertices[3] = center + new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z) * 0.5f;

        // Back wall, bottom line, upper line 
        // Bottom Right Back
        vertices[4] = center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        // Bottom Left Back
        vertices[5] = center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        // Upper Right Back
        vertices[6] = center + new Vector3(-boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z) * 0.5f; 
        // Upper Left Back 
        vertices[7] = center + new Vector3(-boxCollider.size.x, boxCollider.size.y, boxCollider.size.z) * 0.5f;

        return vertices;
    }
}

public struct BrickWallData {
    public readonly int bricksInHeight; 
    public readonly int bricksInWidth;
    public readonly Vector3 brickSize;
    public readonly GameObject[,] bricks;

    public BrickWallData(int bricksInHeight, int bricksInWidth, Vector3 brickSize, GameObject[,] bricks) {
        this.bricksInHeight = bricksInHeight;
        this.bricksInWidth = bricksInWidth;
        this.brickSize = brickSize;
        this.bricks = bricks;
    }
}
