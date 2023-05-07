using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManagerScript : MonoBehaviour
{
    private int selectedX = -1, selectedY = -1;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    public ChessPieces[,] ChessPiecePosition;
    private ChessPieces selectedPiece;

    public List<GameObject> chessPicees;
    public List<GameObject> activePieces = new();

    // Start is called before the first frame update
    void Start()
    {
        ChessPiecePosition = new ChessPieces[8, 8];
    }

    // Update is called once per frame
    void Update()
    {
        DrawChessBoard();
        UpdateSelected();
    }

    private void SpawnChessPiece(int index, int x, int y)
    {
        GameObject go = Instantiate(chessPicees[index], GetTileCentre(x, y), chessPicees[index].transform.rotation) as GameObject;
        go.transform.SetParent(transform);
        ChessPiecePosition[x, y] = go.GetComponent<ChessPieces>();
        ChessPiecePosition[x, y].SetPostion(x, y);
        activePieces.Add(go);
    }


    //Generates 8x8 chess board grid to simplify code
    private void DrawChessBoard()
    {
        Vector3 boardWidth = Vector3.right * 8;
        Vector3 boardLength = Vector3.forward * 8;

        //Drawing the Chessboard
        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + boardWidth);

            for(int j =0; j <= 8; j++)
            {
                start = Vector3.right * i;
                Debug.DrawLine(start, start + boardLength);
            }
        }

        if(selectedX>=0 && selectedY >= 0)
        {
        Debug.DrawLine((Vector3.forward* selectedY) + (Vector3.right * selectedX),
        (Vector3.forward * (selectedY + 1)) + (Vector3.right * (selectedX + 1)));
        Debug.DrawLine((Vector3.forward * selectedY) + (Vector3.right * (selectedX + 1)),
        (Vector3.forward * (selectedY + 1)) + (Vector3.right * selectedX));
        }
    }

    private void UpdateSelected()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        float raycastDistance = 25f;
        
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, LayerMask.GetMask("ChessPlane")))
        {
            selectedX = (int)hit.point.x;
            selectedY = (int)hit.point.y;
        }
        else
        {
            selectedX = -1;
            selectedY = -1;
        }
    }

    private Vector3 GetTileCentre(int x,int y)
    {
        Vector3 origin = Vector3.zero;

        origin.x = (TILE_SIZE * x) + TILE_OFFSET;
        origin.y = (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }
}
