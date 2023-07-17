using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Data;

public class CalcManager : MonoBehaviour
{
    string[] modes = { "INFIX", "POSTFIX", "BEDMAS" };
    string[] operators = { "+", "-", "/", "x" };
    string[] digits = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    string currMode = null;

    public CalcMemory myMemory, memoryStore;
    public GameObject prevModeButton, nextModeButton;
    public GameObject displayTextObject;
    public bool POSReady, POSRMem;
    string bedmasString, bSMem;
    TextMeshPro displayText;
    // Start is called before the first frame update
    void Start()
    {
        POSReady = false;
        bedmasString = "";
        myMemory = new CalcMemory();
        //Debug.Log("SANITY CHECK! MY CALCULATOR MEMORY IS COOL BECAUSE OF THIS: " + myMemory.ans);
        currMode = modes[0];
        displayText = displayTextObject.GetComponent<TextMeshPro>();
        displayText.text = myMemory.display;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float Apply(float a, float b, string c)
    {
        if (c == "+") return a + b;
        if (c == "-") return a - b;
        if (c == "x") return a * b;
        if (c == "/") return a / b;
        else return b;
    }

    void HandleInNumberClick(float e)
    {
        
        myMemory.key = e.ToString();
        if (myMemory.gotDEC)
        {
            displayText.text += e.ToString();
            myMemory.num = float.Parse(displayText.text);
            //Debug.LogError("The Number is: " + myMemory.num);
        }
        else
        {
            myMemory.num = myMemory.num * 10 + (e);
        }
        displayText.text = myMemory.num.ToString();
        myMemory.display = displayText.text;
        myMemory.gotNUM = true;
    }
    void HandleInOperatorClick(string e)
    {
        myMemory.key = e;
        if (myMemory.gotEQ && myMemory.gotNUM)
        {
            myMemory.ans = Apply(myMemory.ans, myMemory.num, myMemory.opr);
            Debug.LogError(myMemory.ans);
            displayText.text = myMemory.ans.ToString();
            myMemory.display = displayText.text;
            myMemory.num = 0;
            myMemory.gotNUM = myMemory.gotDEC = false;
            if(e != "=")
            {
                myMemory.opr = e;
            }
            return;
        }
        if ( e != "=" && !myMemory.gotNUM)
        {
            myMemory.opr = e;
            myMemory.gotEQ = true;
            myMemory.gotNUM = myMemory.gotDEC = false;
            displayText.text = e;
        }
        if (e != "=" && myMemory.gotNUM) {
            //Debug.LogError(e);
            myMemory.opr = e;
            myMemory.ans = myMemory.num;
            myMemory.gotEQ = true;
            myMemory.num = 0;
            myMemory.gotNUM = myMemory.gotDEC = false;
            displayText.text = e;
        }
    }

    void HandlePosOperatorClick(string e)
    {
        myMemory.key = e;
        if (e == "=" && myMemory.gotNUM && !myMemory.gotEQ) 
        {
            //This means we're entering a number in for later use
            myMemory.ans = myMemory.num;
            myMemory.gotEQ = true;
            myMemory.num = 0;
            myMemory.gotNUM = myMemory.gotDEC = false;
            displayText.text = myMemory.num.ToString();
            myMemory.display = displayText.text;
            Debug.LogError("FIRST NUMBER ENTERED: " + myMemory.ans);
        } 
        else if (e == "=" && myMemory.gotNUM && myMemory.gotEQ)
        {
            //This means we have BOTH numbers, and we actually do nothing here other than pull the static flag
            POSReady = true;
            displayText.text = "0";
            myMemory.display = displayText.text;
            Debug.LogError("SECOND NUMBER ENTERED: " + myMemory.num);
        }
        else if (e != "=" && POSReady)
        {
            //This means we have an operator that we're ready to use, so let's use it
            myMemory.ans = Apply(myMemory.ans, myMemory.num, e);
            Debug.LogError(myMemory.ans);
            displayText.text = myMemory.ans.ToString();
            myMemory.display = displayText.text;
            myMemory.num = 0;
            myMemory.gotNUM = myMemory.gotDEC = false;
        }
    }

    void HandleDecClick()
    {
        if (!myMemory.gotDEC && myMemory.gotNUM)
        {
            displayText.text += ".";
            myMemory.display = displayText.text;
            myMemory.gotDEC = true;
        }
    }

    void ClearEverything(CalcMemory theMemory)
    {
        theMemory.gotNUM = false;
        theMemory.gotEQ = false;
        theMemory.gotDEC = false;
        theMemory.ans = 0;
        theMemory.num = 0;
        theMemory.opr = "";
        theMemory.key = "";
        theMemory.display = "0";
        displayText.text = "0";
        myMemory.display = displayText.text;
        if (currMode == "BEDMAS") { bedmasString = ""; }
        if (currMode == "POSTFIX") { POSReady = false; }
    }

    void Clear()
    {
        if(displayText.text == myMemory.num.ToString())
        {
            myMemory.num = 0;
        }
        if(displayText.text == myMemory.ans.ToString())
        {
            myMemory.ans = 0;
        }
        displayText.text = "0";
        if(currMode == "BEDMAS") { bedmasString = bedmasString.Remove(bedmasString.Length - 1, 1); }
        myMemory.display = displayText.text;
    }

    void MemoryStore()
    {
        memoryStore = new CalcMemory();
        memoryStore.num = myMemory.num;
        memoryStore.ans = myMemory.ans;
        memoryStore.gotNUM = myMemory.gotNUM;
        memoryStore.gotDEC = myMemory.gotDEC;
        memoryStore.gotEQ = myMemory.gotEQ;
        memoryStore.opr = myMemory.opr;
        memoryStore.key = myMemory.key;
        memoryStore.display = displayText.text;
        if( currMode == "POSTFIX" ) { POSRMem = POSReady; }
        if( currMode == "BEDMAS" ) { bSMem = bedmasString; }
    }

    void MemoryReset()
    {
        myMemory.num = memoryStore.num;
        myMemory.ans = memoryStore.ans;
        myMemory.gotNUM = memoryStore.gotNUM;
        myMemory.gotDEC = memoryStore.gotDEC;
        myMemory.gotEQ = memoryStore.gotEQ;
        myMemory.opr = memoryStore.opr;
        myMemory.key = memoryStore.key;
        myMemory.display = memoryStore.display;
        displayText.text = myMemory.display;
        if (currMode == "POSTFIX") { POSReady = POSRMem; }
        if (currMode == "BEDMAS") { bedmasString = bSMem; }
    }

    public void ButtonPressed(string buttonText)
    {
        //First, check if we're changing modes
        if(buttonText == "BEDMAS" || buttonText == "POSTFIX" || buttonText == "INFIX")
        {
            SwitchMode(buttonText);
        }
        else
        {
            switch (currMode)
            {
                case "INFIX":
                    InfixOp(buttonText);
                    break;
                case "POSTFIX":
                    PostfixOp(buttonText);
                    break;
                case "BEDMAS":
                    BedmasOp(buttonText);
                    break;
            }

        }
    }

    void SwitchMode(string buttonText)
    {
        //Change current mode
        currMode = buttonText;
        //Change button texts to reflect current mode and clear the memory and calculator to start fresh
        switch (currMode)
        {
            case "INFIX":
                //Debug.LogError(currMode);
                prevModeButton.GetComponentInChildren<TextMeshPro>().text = "BEDMAS";
                nextModeButton.GetComponentInChildren<TextMeshPro>().text = "POSTFIX";
                if (memoryStore != null) { ClearEverything(memoryStore); }
                ClearEverything(myMemory);
                break;
            case "POSTFIX":
                //Debug.LogError(currMode);
                prevModeButton.GetComponentInChildren<TextMeshPro>().text = "INFIX";
                nextModeButton.GetComponentInChildren<TextMeshPro>().text = "BEDMAS";
                if (memoryStore != null) { ClearEverything(memoryStore); }
                ClearEverything(myMemory);
                break;
            case "BEDMAS":
                //Debug.LogError(currMode);
                prevModeButton.GetComponentInChildren<TextMeshPro>().text = "POSTFIX";
                nextModeButton.GetComponentInChildren<TextMeshPro>().text = "INFIX";
                if (memoryStore != null) { ClearEverything(memoryStore); }
                ClearEverything(myMemory);
                break;
        }
        //Reset memories and everything

    }

    void InfixOp(string buttonText)
    {
        switch (buttonText)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                HandleInNumberClick(float.Parse(buttonText));
                break;
            case "+":
            case "-":
            case "/":
            case "x":
            case "=":
                HandleInOperatorClick(buttonText);
                break;
            case ".":
                HandleDecClick();
                break;
            case "CE":
                ClearEverything(myMemory);
                break;
            case "C":
                Clear();
                break;
            case "M":
                MemoryStore();
                break;
            case "MR":
                MemoryReset();
                break;
            default:
                break;

        }
    }

    void BedmasExecute(string expression)
    {
        DataTable dt = new DataTable();
        string forSubmission = bedmasString.Replace('x', '*');
        object response = dt.Compute(forSubmission, "");
        Debug.LogError("HERE'S THE OBJECT: " + response);
        displayText.text = response.ToString();
        bedmasString = displayText.text;

    }

    void BedmasInput(string key)
    {
        if(bedmasString == "" && !operators.Contains(key) && key != ")") { bedmasString += key; displayText.text = bedmasString; myMemory.display = displayText.text; }
        else
        {
            switch (bedmasString.Last().ToString())
            {
                //After numbers, I'll accept numbers, operators, a decimal point, or a closing bracket
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (operators.Contains(key) || digits.Contains(key) || ( key == "." && !myMemory.gotDEC )|| key == ")") { bedmasString += key; displayText.text = bedmasString; myMemory.display = displayText.text; }
                    //If we enter a decimal, use the decimal flag to make sure we don't enter multiple decimals in one number
                    if (key == ".") { myMemory.gotDEC = true; }
                    break;
                case ".":
                case "+":
                case "-":
                case "/":
                case "x":
                case "(":
                    //After operators, a decimal, or an opening bracket, I'll only accept numbers or more opening brackets
                    if (digits.Contains(key) || key == "(") { bedmasString += key; myMemory.gotDEC = false; displayText.text = bedmasString; myMemory.display = displayText.text; }
                    break;
                case ")":
                    //And after a closing bracket, I need an operator (no implicit multiplication in my calculator)
                    if (operators.Contains(key)) { bedmasString += key; myMemory.gotDEC = false; displayText.text = bedmasString; myMemory.display = displayText.text; }
                    break;
            }
        }
    }

    void PostfixOp(string buttonText)
    {
        switch (buttonText)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                HandleInNumberClick(int.Parse(buttonText));
                break;
            case "+":
            case "-":
            case "/":
            case "x":
            case "=":
                HandlePosOperatorClick(buttonText);
                break;
            case ".":
                HandleDecClick();
                break;
            case "CE":
                ClearEverything(myMemory);
                break;
            case "C":
                Clear();
                break;
            case "M":
                MemoryStore();
                break;
            case "MR":
                MemoryReset();
                break;
            default:
                break;

        }
    }

    void BedmasOp(string buttonText)
    {
        switch (buttonText)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case ".":
            case "+":
            case "-":
            case "/":
            case "x":
            case "(":
            case ")":
                BedmasInput(buttonText);
                break;
            case "=":
                BedmasExecute(bedmasString);
                break;
            case "CE":
                ClearEverything(myMemory);
                break;
            case "C":
                Clear();
                break;
            case "M":
                MemoryStore();
                break;
            case "MR":
                MemoryReset();
                break;
            default:
                break;

        }
    }

    void dummyFunction(string buttonText)
    {
        displayText.text = buttonText;
    }
}
