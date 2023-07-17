using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcMemory
{
    public bool gotNUM;
    public bool gotEQ;
    public bool gotDEC;
    public float ans;
    public float num;
    public string opr;
    public string key;
    public string display;

    public CalcMemory()
    {
        gotNUM = false;
        gotEQ = false;
        gotDEC = false;
        ans = 0;
        num = 0;
        opr = "";
        key = "";
        display = "0";
    }
}
