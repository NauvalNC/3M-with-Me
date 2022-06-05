using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HandWashManager : MonoBehaviour
{
    private int step = 0;
    [SerializeField] private Button showerBtn, soapBtn, backBtn;
    [SerializeField] private Animator shower;
    [SerializeField] private Animator bubbles;
    [SerializeField] private Animator blings;
    [SerializeField] private Animator handShake;
    [SerializeField] private Animator fadeAC;
    [SerializeField] private float handShakeLength = 1f;
    [SerializeField] private AudioSource simulationSound;
    [SerializeField] private AudioClip waterSound;
    [SerializeField] private AudioClip bubbleSound;

    [Header("Score Panel")]
    [SerializeField] private Animator scorePanel;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private int defaultScore;

    [SerializeField] private Image character;
    [SerializeField] private Image dialogueChar;

    void Start()
    {
        character.sprite = dialogueChar.sprite = CostumeProvider.Instance.GetChosenCostume();
        step = 0;

        // Sound Button
        backBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });

        backBtn.gameObject.SetActive(false);
        showerBtn.gameObject.SetActive(false);
        soapBtn.gameObject.SetActive(false);

        backBtn.onClick.AddListener(delegate { StartCoroutine(IReturnToMainMenu()); });
        showerBtn.onClick.AddListener(delegate { TurnOnShower(); });
        soapBtn.onClick.AddListener(delegate { UseSoap(); });

        GameManager.Instance.isDialogueKeepActiveMode = true;

        StartCoroutine(IStart());
    }

    IEnumerator IStart()
    {
        yield return new WaitForSeconds(0.2f);

        ExecNextHandWashStep();
    }

    IEnumerator IReturnToMainMenu()
    {
        fadeAC.Play("fade_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Lobby");
    }

    public void TurnOnShower()
    {
        if (step != 1 && step != 4) return;

        showerBtn.gameObject.SetActive(false);
        StartCoroutine(IHandShaking(3.2f));
        StartCoroutine(ITurnOnShower());
    }

    IEnumerator ITurnOnShower()
    {
        simulationSound.clip = waterSound;
        simulationSound.Play();

        shower.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        shower.gameObject.SetActive(false);

        if (step == 4) bubbles.gameObject.SetActive(false);

        simulationSound.Stop();

        ExecNextHandWashStep();
    }

    public void UseSoap()
    {
        if (step != 2) return;

        simulationSound.clip = bubbleSound;
        simulationSound.Play();

        soapBtn.gameObject.SetActive(false);

        if (step == 2)
        {
            bubbles.gameObject.SetActive(true);
            ExecNextHandWashStep();
        }
    }

    IEnumerator IHandShaking(float length)
    {
        handShake.Play("handwash", 0, -1f);
        yield return new WaitForSeconds(length);

        handShake.Play("idle", 0, -1f);

        if (step == 3) ExecNextHandWashStep();
    }

    void ExecNextHandWashStep()
    {
        step++;

        switch(step)
        {
            case 1:
                showerBtn.gameObject.SetActive(true);
                GameManager.Instance.dialogues.Enqueue("Mari kita basuh tangan dengan air dahulu!");
                break;
            case 2:
                soapBtn.gameObject.SetActive(true);
                GameManager.Instance.dialogues.Enqueue("Sip! Mari gunakan sabun agar bersih dari kuman dan virus!");
                break;
            case 3:
                GameManager.Instance.dialogues.Enqueue("Setelah itu, gosok-gosokkan tangan agar sabun merata!");
                StartCoroutine(IHandShaking(handShakeLength));
                break;
            case 4:
                showerBtn.gameObject.SetActive(true);
                GameManager.Instance.dialogues.Enqueue("Sip! Basuh lagi dengan air hingga bersih!");
                break;
            case 5:
                GameOver();
                break;
        }
    }

    void GameOver()
    {
        SoundManager.Instance.winSound.Play();
        PlayerPrefs.SetInt("AFTER_PLAY", 0);
        scoreTxt.text = "+" + defaultScore;
        GameManager.Instance.AddStars(defaultScore);
        scorePanel.gameObject.SetActive(true);
    }

    public void CloseScorePanel()
    {
        StartCoroutine(ICloseScorePanel());
    }

    IEnumerator ICloseScorePanel()
    {
        scorePanel.Play("score_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);
        scorePanel.gameObject.SetActive(false);

        backBtn.gameObject.SetActive(true);
        blings.gameObject.SetActive(true);
        GameManager.Instance.dialogues.Enqueue("Wah! Sudah bersih! Tekan tombol kembali untuk melanjutkan aktivitas!");
    }
}
