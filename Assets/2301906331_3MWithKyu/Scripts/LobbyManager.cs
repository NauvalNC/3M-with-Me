using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button exitBtn, costumeBtn, playBtn;
    [SerializeField] private Button maskBtn, handWashBtn;
    [SerializeField] private GameObject mask;
    [SerializeField] private Animator charAC;
    [SerializeField] private Animator fadeAC;

    [Header("Info Panel")]
    [SerializeField] private Animator infoPanel;
    [SerializeField] private Button infoBtn;

    [Header("Exit Panel")]
    [SerializeField] private Animator exitPanel;
    [SerializeField] private Button confirmExitBtn, cancelExitBtn;

    [SerializeField] private bool isWearMask = false;
    [SerializeField] private bool isAfterPlay = false;

    [SerializeField] private Image catSprite;

    private void Start()
    {
        catSprite.sprite = CostumeProvider.Instance.GetChosenCostume();
        
        // Button Sound
        costumeBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        infoBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        confirmExitBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        cancelExitBtn.onClick.AddListener(delegate { SoundManager.Instance.falseBtnSound.Play(); });
        handWashBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        exitBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });


        isWearMask = PlayerPrefs.GetInt("MASKED", 0) == 0 ? false : true;
        isAfterPlay = PlayerPrefs.GetInt("AFTER_PLAY", 0) == 0 ? false : true;

        infoBtn.onClick.AddListener(delegate { infoPanel.gameObject.SetActive(true); });
        maskBtn.onClick.AddListener(delegate { WearMask(); });
        playBtn.onClick.AddListener(delegate { OpenPlayMenu(); });
        costumeBtn.onClick.AddListener(delegate { OpenCostumeMenu(); });
        handWashBtn.onClick.AddListener(delegate { StartCoroutine(IOpenScene("HandWash", 0f)); });

        exitBtn.onClick.AddListener(delegate { exitPanel.gameObject.SetActive(true); });
        confirmExitBtn.onClick.AddListener(delegate { ExitGame(true); });
        cancelExitBtn.onClick.AddListener(delegate { ExitGame(false); });

        StartCoroutine(IStart());
    }

    IEnumerator IStart()
    {
        yield return new WaitForSeconds(0.2f);

        if (isAfterPlay)
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Setelah bermain di luar, yuk cuci tangan kita terlebih dahulu!");
        }
    }

    private void Update()
    {
        mask.SetActive(isWearMask);
    }

    public void ExitGame(bool confirmExit)
    {
        if (confirmExit) Application.Quit();
        else StartCoroutine(ICloseExitPanel());
    }

    IEnumerator ICloseExitPanel()
    {
        exitPanel.Play("score_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);
        exitPanel.gameObject.SetActive(false);
    }

    public void OpenCostumeMenu() 
    {
        if (isAfterPlay == false)
        {
            StartCoroutine(IOpenScene("CostumeSelection", 0f));
        } else
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Habis bermain, lebih baik kita cuci tangan terlebih dahulu!");
            GameManager.Instance.dialogues.Enqueue("Ini adalah bagian usaha dari mencegah infeksi virus COVID-19.");
        }
    }

    public void OpenPlayMenu()
    {
        if (isWearMask && isAfterPlay == false)
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Masker siap! Yuk mari bermain di luar!");
            StartCoroutine(IOpenScene("JigsawPuzzle", 1f));
        }
        else if (isAfterPlay)
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Habis bermain, lebih baik kita cuci tangan terlebih dahulu!");
            GameManager.Instance.dialogues.Enqueue("Ini adalah bagian usaha dari mencegah infeksi virus COVID-19.");
        }
        else if (isWearMask == false)
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Sebelum bermain di luar, yuk kita pakai masker dulu!");
        } 
    }

    public void WearMask()
    {
        isWearMask = !isWearMask;
        PlayerPrefs.SetInt("MASKED", isWearMask ? 1 : 0);

        if (isWearMask)
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Pastikan masker menutupi mulut dan hidung ya...!");
        }
        else
        {
            charAC.Play("char_bounce", 0, -1f);
            GameManager.Instance.dialogues.Enqueue("Jangan lupa memakai masker ketika mau keluar rumah :)");
        }
    }

    IEnumerator IOpenScene(string sceneName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        fadeAC.Play("fade_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneName);
    }

    public void CloseInfoPanel()
    {
        StartCoroutine(ICloseInfoPanel());
    }

    IEnumerator ICloseInfoPanel()
    {
        infoPanel.Play("score_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);
        infoPanel.gameObject.SetActive(false);
    }
}
