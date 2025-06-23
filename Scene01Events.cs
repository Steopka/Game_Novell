using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene01Event : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject charKasumi;
    public GameObject charHaruka;
    public GameObject textBox;
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
        textBox.SetActive(true);
        yield return new WaitForSeconds(2);
        charHaruka.SetActive(true);
    }

}