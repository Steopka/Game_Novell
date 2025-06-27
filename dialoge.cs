using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // Добавлено для событий
using TMPro;
public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public string[] lines;
    public float textSpeed = 0.05f;
    public TMP_Text dialogueText;

    [Header("Configuration")]
    public bool startOnAwake = true;
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    private int index;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool dialogueActive = false;

    void Start()
    {
        // Проверка компонента Text
        if (dialogueText == null)
        {
            // Попытка автоматического поиска
            dialogueText = GetComponentInChildren<TMP_Text>();

            if (dialogueText == null)
            {
                Debug.LogError("Dialogue Text component not found! Please assign a Text component.", this);
                enabled = false;
                return;
            }
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
            Debug.LogWarning("No dialogue lines available!", this);
            return;
        }

        dialogueActive = true;
        index = 0;
        dialogueText.text = string.Empty;
        gameObject.SetActive(true);
        onDialogueStart?.Invoke();
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
        dialogueText.text = "";

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
        if (!dialogueActive) return;

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

    private void NextLine()
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

        dialogueActive = false;
        dialogueText.text = string.Empty;
        gameObject.SetActive(false);
        onDialogueEnd?.Invoke();
    }

    // Для отладки в редакторе
    void OnValidate()
    {
        if (dialogueText == null)
        {
            dialogueText = GetComponentInChildren<TMP_Text>();
        }
    }
}