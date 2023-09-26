using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public Movement movement;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index = 0 ;
    public Canvas pauseCanvas;
    public GameObject YesOrNoCanvas;

    void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
        textComponent.text = string.Empty;
        startDialogue();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void startDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (index < 0 || index >= lines.Length)
        {
            yield break;
        }
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;

            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if(textComponent.text == lines[lines.Length - 1])
        {
            gameObject.SetActive(false);

            //matiin canvas yes or no
            YesOrNoCanvas.SetActive(true);
            Debug.Log("DONGGG");
        }
        else if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    public void Clicked()
    {
        YesOrNoCanvas.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
        CursorLockManager.instance.DisableCursor();
        movement.isTalking = false;
    }
}


