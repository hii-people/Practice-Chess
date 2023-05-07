using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPieces : MonoBehaviour
{
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public bool isWhite;

    public void SetPostion(int x,int y)
    {
        CurrentX = x; 
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

}
