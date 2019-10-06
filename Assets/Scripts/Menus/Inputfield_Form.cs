using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputfield_Form : MonoBehaviour
{
    [SerializeField]
    private InputField field = null;
    [SerializeField]
    private Text errorText = null;
    [SerializeField]
    private EVerification eVerification = EVerification.unknow;

    public string Content
    {
        get
        {
            return field.text;
        }
    }

    public enum EVerification
    {
        unknow = 0,
        notEmpty = 1,
        correctMail = 2,
        correctPassword = 3,
    }


    private void Start()
    {
        ResetField();
    }
    public void ResetField()
    {
        field.text = "";
        errorText.text = "";
        errorText.enabled = false;
        errorText.raycastTarget = false;
    }

    public bool Validate()
    {
        string fieldValue = field.text;
        bool isValide = true;

        switch (eVerification)
        {
            case EVerification.notEmpty:
                isValide = IsEmpty(fieldValue);
                break;
            case EVerification.correctMail:
                isValide = IsEmpty(fieldValue);
                if (isValide)
                    isValide = IsAnEmail(fieldValue);

                break;
            case EVerification.correctPassword:
                isValide = IsEmpty(fieldValue);
                if (isValide)
                    isValide = IsLongEnough(fieldValue,6);

                break;
            default:
                isValide = false;
                Debug.Log("Verification not set");
                break;
        }

        return isValide;
    }

    //Checks
    bool IsEmpty(string fieldValue)
    {
        if (fieldValue != "")
        {
            return true;
        }
        else
        {
            DisplayError("Field required");
            return false;
        }
    }
    bool IsLongEnough(string fieldValue, int minCharacter)
    {
        if (fieldValue.Length >= minCharacter)
        {
            return true;
        }
        else
        {
            DisplayError("Minimum " + minCharacter + " caracters");
            return false;
        }
    }
    bool IsAnEmail(string fieldValue)
    {
        if(fieldValue.Contains(".") && fieldValue.Contains("@"))
        {
            return true;
        }
        else
        {
            DisplayError("Invalide email");
            return false;
        }
    }


    public void DisplayError(string error)
    {
        errorText.enabled = true;
        errorText.text = error;
    }
}
