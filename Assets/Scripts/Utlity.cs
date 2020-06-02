﻿using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public class CustomVector3IntComparer : IComparer<Vector3Int>
{
    public int Compare(Vector3Int x, Vector3Int y)
    {
        if (x == null || y == null)
            return 0;

        return y.x.CompareTo(x.x);
    }
}