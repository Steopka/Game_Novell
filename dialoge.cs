using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Используйте для TextMeshPro

public class DialogueSystem : MonoBehaviour // Более понятное имя класса
{
    [Header("Dialogue Settings")]
    public string[] lines;                  // Массив реплик
    public float textSpeed = 0.05f;         // Скорость появления текста
    public TMP_Text dialogueText;               // Для стандартного UI Text
                                            // public TMP_Text dialogueText;        // Раскомментировать для TextMeshPro

    [Header("Configuration")]
    public bool startOnAwake = true;        // Запускать диалог автоматически

    private int index;                      // Текущая позиция в диалоге
    private Coroutine typingCoroutine;      // Ссылка на корутину
    private bool isTyping = false;          // Флаг процесса печати

    void Start()
    {
        if (dialogueText == null)
        {
            Debug.LogError("Dialogue Text component not assigned!");
            enabled = false;
            return;
        }

        dialogueText.text = string.Empty;

        if (startOnAwake && lines.Length > 0)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (lines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines available!");
            return;
        }

        index = 0;
        dialogueText.text = string.Empty;
        gameObject.SetActive(true);
        StartTypingLine();
    }

    private void StartTypingLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = ""; // Очищаем предыдущий текст

        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    public void SkipOrContinue()
    {
        if (isTyping)
        {
            // Пропустить анимацию печати
            StopCoroutine(typingCoroutine);
            dialogueText.text = lines[index];
            isTyping = false;
            typingCoroutine = null;
        }
        else
        {
            // Перейти к следующей реплике
            NextLine();
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartTypingLine();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        gameObject.SetActive(false);
        // Здесь можно добавить вызов события окончания диалога
        // OnDialogueEnd?.Invoke();
    }
}