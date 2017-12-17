using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {

    public GameObject[] pipes;
    private Vector3 startPoint;
    public static GameObject[,] currentGridMatrix;
    private Color flow = Color.green, end = Color.blue;

    private const int ROWS = 12, COLS = 10;
    private const float SPACING = 0.4f;

    void Start()
    {
        float startX = (COLS / 2) * SPACING;
        float startY = (ROWS / 2) * SPACING;
        currentGridMatrix = new GameObject[ROWS, COLS];
        startPoint = new Vector3(transform.position.x - startX, transform.position.y + startY, transform.position.z);
        BuildPipes();
    }


    private void BuildPipes()
    {
        float startX = startPoint.x, startY = startPoint.y, startZ = startPoint.z - 0.8f;
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLS; j++)
            {
                if (i == 0 || i == ROWS - 1)
                {
                    if (j == Mathf.Abs(COLS / 2))
                    {
                        currentGridMatrix[i, j] =
                            Instantiate(pipes[0], new Vector3(startX, startY, startZ), Quaternion.identity);
                        currentGridMatrix[i, j].GetComponent<Pipe>().colIndex = j;
                        currentGridMatrix[i, j].GetComponent<Pipe>().rowIndex = i;
                        if (i == 0)
                        {
                            currentGridMatrix[i, j].GetComponent<Pipe>().pipeType =
                                ConstantController.PIPE_PIECES.Start;
                            currentGridMatrix[i, j].GetComponent<Pipe>().connectedSupply = true;
                            currentGridMatrix[i, j].GetComponent<Pipe>().SetColour(flow);
                        }
                        else
                        {
                            currentGridMatrix[i, j].GetComponent<Pipe>().pipeType =
                                ConstantController.PIPE_PIECES.Finish;
                            currentGridMatrix[i, j].GetComponent<Pipe>().connectedSupply = false;
                            currentGridMatrix[i, j].GetComponent<Pipe>().SetColour(end);
                        }
                        currentGridMatrix[i, j].transform.parent = gameObject.transform;
                        j = COLS + 1;
                    }
                    startX += SPACING;
                }
                else
                {
                    currentGridMatrix[i, j] =
                        Instantiate(pipes[Random.Range(0, pipes.Length)], new Vector3(startX, startY, startZ), Quaternion.identity);
                    currentGridMatrix[i, j].GetComponent<Pipe>().connectedSupply = false;
                    currentGridMatrix[i, j].GetComponent<Pipe>().pipeType = ConstantController.PIPE_PIECES.Game;
                    currentGridMatrix[i, j].transform.parent = gameObject.transform;
                    startX += SPACING;
                    int rotLotto = Random.Range(0, 4);
                    for (int k = 0; k < rotLotto; k++)
                    {
                        currentGridMatrix[i,j].GetComponent<Pipe>().InitialRotation();
                    }
                    
                }
            }
            startX = startPoint.x;
            startY -= SPACING;
        }
    }

    public static void DropUnusedPipes()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Pipe");
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetComponent<Pipe>().GetPipeType() == ConstantController.PIPE_PIECES.Game)
            {
                if (!objs[i].GetComponent<Pipe>().CheckConnectedSupply())
                {
                    objs[i].GetComponent<Pipe>().AddGravity();
                }
            }
        }
    }
}