using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string characterName; // Имя персонажа
    [TextArea(3, 5)] public string text; // Текст реплики
    public Color nameColor = Color.white; // Цвет имени персонажа
}

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueLine[] dialogueLines; // Массив реплик с именами
    public float textSpeed = 0.05f;
    public TMP_Text dialogueText;
    public TMP_Text nameText; // Текстовый элемент для имени персонажа

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
        // Проверка компонентов
        if (dialogueText == null)
        {
            dialogueText = GetComponentInChildren<TMP_Text>();
            if (dialogueText == null)
            {
                Debug.LogError("Dialogue Text component not found!", this);
                enabled = false;
                return;
            }
        }

        if (nameText == null)
        {
            Debug.LogWarning("Name Text component not assigned. Names won't be displayed.");
        }

        dialogueText.text = string.Empty;
        if (nameText != null) nameText.text = string.Empty;

        if (startOnAwake && dialogueLines.Length > 0)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (dialogueLines.Length == 0)
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

        // Устанавливаем имя персонажа
        if (nameText != null)
        {
            nameText.text = dialogueLines[index].characterName;
            nameText.color = dialogueLines[index].nameColor;
        }

        // Постепенно выводим текст
        foreach (char c in dialogueLines[index].text.ToCharArray())
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
            dialogueText.text = dialogueLines[index].text;
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
        if (index < dialogueLines.Length - 1)
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
        if (nameText != null) nameText.text = string.Empty;
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