using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TextCreator : MonoBehaviour
{
    public static TMPro.TMP_Text viewText;
    public static bool runTextPrint;
    public static int charCout;
    [SerializeField] string transferText;
    [SerializeField] int internaCount;
   
  
    void Update()
    {
        internaCount = charCout;
        charCout = GetComponent<TMPro.TMP_Text>().text.Length;
        if (runTextPrint == true)
        {
            runTextPrint = false;
            viewText = GetComponent<TMPro.TMP_Text>(); 
            transferText = viewText.text;
            viewText.text = "";
            StartCoroutine(RollText());
        }
    }
    IEnumerator RollText()
    {
        foreach(char c in transferText)
        {
            viewText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
}