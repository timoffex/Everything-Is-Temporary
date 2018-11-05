using System;
using UnityEngine;

public struct PageCoordinates
{
    public enum PageSide
    {
        Left, Right
    }

    public readonly PageSide side;
    public readonly Vector2 coords;

    public PageCoordinates(PageSide side, Vector2 pageCoords)
    {
        this.side = side;
        this.coords = pageCoords;
    }
}