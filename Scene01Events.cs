using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene01Event : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject charKasumi;
    public GameObject charHaruka;
    public GameObject textBox;

    [SerializeField] AudioSource girlSingh;
    [SerializeField] AudioSource girlGasp;
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;

    private void Update()
    {
        textLength = TextCreator.charCout;
    }

    private void Start()
    {
        StartCoroutine(EventStarter());
    }
    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2); 
        fadeScreenIn.SetActive(false);
        charKasumi.SetActive(true);
        yield return new WaitForSeconds(2);
        //
        mainTextObject.SetActive(true);
        textToSpeak = "*-Пожалуйста... Только не снова... ";
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        girlSingh.Play();
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
       
        
        
        
        textBox.SetActive(true);       
        yield return new WaitForSeconds(2);
        charHaruka.SetActive(true);
        girlGasp.Play();
    }

}