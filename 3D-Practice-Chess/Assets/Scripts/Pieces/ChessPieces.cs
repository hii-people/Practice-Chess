using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPieces : MonoBehaviour
{
    public int currentX;
    public int currentY;
    public bool isWhite;

    public void SetPostion(int x,int y)
    {
        currentX = x;
        currentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

}
