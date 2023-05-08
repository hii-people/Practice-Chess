using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManagerScript : MonoBehaviour
{
    public static BoardManagerScript Instance { get; set; }
    private bool[,] AllowedMoves { get; set; }

    private int selectedX = -1, selectedY = -1;

    public bool isWhitesTurn = true;

    private const float TILE_SIZE = 1.0f;
    //private const float TILE_SIZE = 0f;
    private const float TILE_OFFSET = 0.5f;
    //private const float TILE_OFFSET = 0f;


    public ChessPieces[,] ChessPiecePosition { get; set; }
    private ChessPieces selectedPiece;

    public List<GameObject> chessPicees;
    public List<GameObject> activePieces = new();

    // Start is called before the first frame update
    void Start()
    { 
        ChessPiecePosition = new ChessPieces[8, 8];

        SpawnAllChessPieces();
    }

    // Update is called once per frame
    void Update()
    {
        DrawChessBoard();
        UpdateSelected();

        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse has been clicked");

            if(selectedX >= 0 && selectedY >= 0)
            {
                //print("Got to this point");
                if(selectedPiece == null)
                {
                    SelectChessPiece(selectedX, selectedY);
                }
                else
                {
                    MoveChessPiece(selectedX, selectedY);
                }
            }
        }
    }


    //Generates 8x8 chess board grid to simplify code
    //Allows me to line up actual board with where chess pieces will be spawned
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

        if (selectedX >= 0 && selectedY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectedY + Vector3.right * selectedX,
                (Vector3.forward * (selectedY + 1)) + (Vector3.right * (selectedX + 1)));
            Debug.DrawLine(Vector3.forward * selectedY + Vector3.right * (selectedX + 1),
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
        //TODO Issue after mouse click not being registerd is here. Something here is not right.
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, LayerMask.GetMask("ChessPlane")))
        {
            print("Got to this point, thank fuck");
            selectedX = (int)hit.point.x;
            selectedY = (int)hit.point.y;
        }
        else
        {
            print("Got to this point");
            selectedX = -1;
            selectedY = -1;
        }
    }

    private void SpawnChessPiece(int index, int x, int y)
    {
        GameObject go = Instantiate(chessPicees[index], GetTileCentre(x, y), chessPicees[index].transform.rotation);
        go.transform.SetParent(transform);
        ChessPiecePosition[x, y] = go.GetComponent<ChessPieces>();
        ChessPiecePosition[x, y].SetPostion(x, y);
        activePieces.Add(go);
    }

    private Vector3 GetTileCentre(int x, int y)
    {
        Vector3 origin = Vector3.zero;

        origin.x = (TILE_SIZE * x) + TILE_OFFSET;
        origin.z = (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void SpawnAllChessPieces()
    {
        // White Chess Pieces
        SpawnChessPiece(0, 4, 0); // King
        SpawnChessPiece(1, 3, 0); // Queen
        SpawnChessPiece(2, 0, 0); // Rook
        SpawnChessPiece(2, 7, 0); // Rook
        SpawnChessPiece(3, 2, 0); // Bishop
        SpawnChessPiece(3, 5, 0); // Bishop
        SpawnChessPiece(4, 1, 0); // Knight
        SpawnChessPiece(4, 6, 0); // Knight

        SpawnChessPiece(5, 0, 1); //Pawns
        SpawnChessPiece(5, 1, 1);
        SpawnChessPiece(5, 2, 1);
        SpawnChessPiece(5, 3, 1);
        SpawnChessPiece(5, 4, 1);
        SpawnChessPiece(5, 5, 1);
        SpawnChessPiece(5, 6, 1);
        SpawnChessPiece(5, 7, 1);

        // Black Chess Pieces
        SpawnChessPiece(6, 4, 7); // King
        SpawnChessPiece(7, 3, 7); // Queen
        SpawnChessPiece(8, 0, 7); // Rook
        SpawnChessPiece(8, 7, 7); // Rook
        SpawnChessPiece(9, 2, 7); // Bishop
        SpawnChessPiece(9, 5, 7); // Bishop
        SpawnChessPiece(10, 1, 7); // Knight
        SpawnChessPiece(10, 6, 7); // Knight

        SpawnChessPiece(11, 0, 6); //Pawns
        SpawnChessPiece(11, 1, 6);
        SpawnChessPiece(11, 2, 6);
        SpawnChessPiece(11, 3, 6);
        SpawnChessPiece(11, 4, 6);
        SpawnChessPiece(11, 5, 6);
        SpawnChessPiece(11, 6, 6);
        SpawnChessPiece(11, 7, 6);
    }

    private void SelectChessPiece(int x, int y)
    {
        if (ChessPiecePosition[x,y] == null)
        {
            return;
        }

        if (ChessPiecePosition[x,y].isWhite != isWhitesTurn)
        {
            return;
        }

        bool hasAtLeastOneMove = false;
       AllowedMoves = ChessPiecePosition[x, y].PossibleMove();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (AllowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;

                    //Breaks out of first loop
                    i = 8;

                    //Breaks out of second loop
                    break;
                }
            }
        }

        if (!hasAtLeastOneMove)
        {
            return;
        }

        selectedPiece = ChessPiecePosition[x, y];

        //TODO Implement Board Highlighting
        //BoardHighlighting.Instance.HighlightAllowedMoves(allowedMoves);

    }

    private void MoveChessPiece(int x, int y)
    {
        if (AllowedMoves[x, y])
        {
            ChessPieces c = ChessPiecePosition[x, y];

            if(c != null && c.isWhite != isWhitesTurn)
            {
                activePieces.Remove(c.gameObject);
                Destroy(c.gameObject);

                if(c.GetType() == typeof(KingScript))
                {
                    //TODO Implement EndGamne()

                    //EndGame();
                    return;
                }
            }

            ChessPiecePosition[selectedPiece.CurrentX, selectedPiece.CurrentY] = null;
            selectedPiece.transform.position = GetTileCentre(x, y);
            ChessPiecePosition[x, y] = selectedPiece;
            isWhitesTurn = !isWhitesTurn;

        }

        //BoardHighlighting.Instance.HideHighlights();
        selectedPiece = null;
    }

    private void EndGame()
    {
        if (isWhitesTurn)
        {
            print("White wins");
        }
        else
        {
            print("Black wins");
        }
    }
}
