using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour
{
    [SerializeField] private int numLimit = 15;

    [SerializeField] private GameObject objEquation;
    [SerializeField] private GameObject objMainField;
    [SerializeField] private HistoryManager historyManager;

    private TMP_Text txtEquation;
    private TMP_Text txtMainField;

    string equation = "";
    string mainFieldNum = "0";
    string numOperator;

    bool nextInputClearEntry = false;
    bool nextInputClearIfNum = false;
    bool inProgress = false;


    // Start is called before the first frame update
    void Start()
    {
        txtEquation = objEquation.GetComponent<TMP_Text>();
        txtMainField = objMainField.GetComponent<TMP_Text>();

        txtEquation.text = " ";
        txtMainField.text = "0";
    }

    private string GetFormattedNumber()
    {
        Debug.Log(mainFieldNum);
        double number = double.Parse(mainFieldNum);

        // Check if it should include decimal
        if (!mainFieldNum.Contains(","))
        {
            // Don't include decimal
            return number.ToString("N0");
        }
        else
        {
            // Include decimal
            //int integerPlaces = mainFieldNum.Split(',')[0].Length;
            int decimalPlaces = mainFieldNum.Split(',')[1].Length;
            if (decimalPlaces == 0)
            {
                return number.ToString("N0") + ",";
            }
            return number.ToString("N" + decimalPlaces);
        }
    }

    private void Calculate()
    {
        if (equation.Length != 0)
        {
            switch(numOperator)
            {
                case "Percent":
                    mainFieldNum = (double.Parse(equation) * (double.Parse(mainFieldNum) / 100)).ToString();
                    break;

                case "1DivideX":
                    mainFieldNum = (1 / double.Parse(mainFieldNum)).ToString();
                    break;

                case "SecondPower":
                    mainFieldNum = Mathf.Pow(float.Parse(mainFieldNum), 2).ToString();
                    break;

                case "SquareRoot":
                    mainFieldNum = Mathf.Sqrt(float.Parse(mainFieldNum)).ToString();
                    break;

                case "Divide":
                    mainFieldNum = (double.Parse(equation) / double.Parse(mainFieldNum)).ToString();
                    break;
                
                case "Multiply":
                    mainFieldNum = (double.Parse(equation) * double.Parse(mainFieldNum)).ToString();
                    break;

                case "Subtract":
                    mainFieldNum = (double.Parse(equation) - double.Parse(mainFieldNum)).ToString();
                    break;
                
                case "Add":
                    mainFieldNum = (double.Parse(equation) + double.Parse(mainFieldNum)).ToString();
                    break;
            }
            txtMainField.text = GetFormattedNumber();
            equation = mainFieldNum;
            
            historyManager.AddHistoryItem(txtEquation.text, txtMainField.text);  
        }
    }

    private void Clear()
    {
        mainFieldNum = "0";
        equation = "";
        txtMainField.text = "0";
        txtEquation.text = " ";
    }

    private void ClearEntry()
    {
        mainFieldNum = "0";
        txtMainField.text = "0";
    }

    public void OnButtonClick(string button)
    {
        switch(button)
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
                if (nextInputClearEntry)
                {
                    ClearEntry();
                    nextInputClearEntry = false;
                    nextInputClearIfNum = false;
                }
                if (nextInputClearIfNum)
                {
                    Clear();
                    nextInputClearEntry = false;
                    nextInputClearIfNum = false;
                }
                string number = button;

                Debug.Log(mainFieldNum.Length);
                if (mainFieldNum.Length < numLimit || mainFieldNum.Length < numLimit + 1 && mainFieldNum.Contains(","))
                {
                    mainFieldNum += number;
                }

                txtMainField.text = GetFormattedNumber();
                break;
            
            case "ClearEntry":
                ClearEntry();
                break;
            
            case "Clear":
                Clear();
                break;
            
            case "Backspace":
                // Make sure not to remove leading zero
                if (mainFieldNum.Length > 1)
                {
                    mainFieldNum = mainFieldNum.Remove(mainFieldNum.Length - 1, 1);
                }
                txtMainField.text = GetFormattedNumber();
                break;
            
            case "1DivideX":
                if (inProgress)
                    Calculate();

                nextInputClearEntry = true;
                txtEquation.text = "1/(" + GetFormattedNumber() + ") ";
                equation = mainFieldNum;
                numOperator = button;

                Calculate();
                inProgress = false;
                break;
            
            case "SecondPower":
                if (inProgress)
                    Calculate();

                nextInputClearEntry = true;
                txtEquation.text = "sqr(" + GetFormattedNumber() + ") ";
                equation = mainFieldNum;
                numOperator = button;

                Calculate();
                inProgress = false;
                break;
            
            case "SquareRoot":
                if (inProgress)
                    Calculate(); 

                nextInputClearEntry = true;
                txtEquation.text = "√(" + GetFormattedNumber() + ") ";
                equation = mainFieldNum;
                numOperator = button;

                Calculate();
                inProgress = false;
                break;
            
            case "Percent":
                if (inProgress)
                    Calculate();

                nextInputClearEntry = true;
                txtEquation.text = GetFormattedNumber() + " * %";
                equation = mainFieldNum;
                numOperator = button;
                inProgress = true;
                break;

            case "Divide":
                if (inProgress)
                    Calculate();

                nextInputClearEntry = true;
                txtEquation.text = GetFormattedNumber() + " / ";
                equation = mainFieldNum;
                numOperator = button;
                inProgress = true;
                break;

            case "Multiply":
                if (inProgress)
                    Calculate(); 

                nextInputClearEntry = true;
                txtEquation.text = GetFormattedNumber() + " * ";
                equation = mainFieldNum;
                numOperator = button;
                inProgress = true;
                break;

            case "Subtract":
                if (inProgress)
                    Calculate(); 

                nextInputClearEntry = true;
                txtEquation.text = GetFormattedNumber() + " - ";
                equation = mainFieldNum;
                numOperator = button;
                inProgress = true;

                break;

            case "Add":
                if (inProgress)
                    Calculate();          
                nextInputClearEntry = true;
                txtEquation.text = GetFormattedNumber() + " + ";
                equation = mainFieldNum;
                numOperator = button;
                inProgress = true;
                break;

            case "SwitchPosNeg":
                mainFieldNum = (double.Parse(mainFieldNum) * -1).ToString();
                GetFormattedNumber();
                break;

            case "Decimal":
                if (!mainFieldNum.Contains(","))
                {
                    mainFieldNum += ",";
                    txtMainField.text = GetFormattedNumber();
                }
                break;

            case "Equals":
                txtEquation.text = txtEquation.text + GetFormattedNumber() + " =";

                Calculate();

                nextInputClearIfNum = true;
                equation = mainFieldNum;
                inProgress = false;
                break;
        }
    }
}
