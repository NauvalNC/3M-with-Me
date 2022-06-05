using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class CostumeManager : MonoBehaviour
{
    [SerializeField] private Button backBtn;
    [SerializeField] private GameObject costumeItemPrefab;
    [SerializeField] private GameObject costumeCont;
    [SerializeField] private Image costumeViewer;
    [SerializeField] private GameObject priceTagPanel, equipPanel;
    [SerializeField] private TMP_Text priceTxt;
    [SerializeField] private Animator fadeAC;

    private int currentIndex;

    private void Start()
    {
        backBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        backBtn.onClick.AddListener(delegate { StartCoroutine(IReturnToMainMenu()); });
        
        GenerateCostumeData();
        PreviewCostume(CostumeProvider.Instance.GetChosenCostumeID());
        HideDialoguePanelAPI();
    }

    void GenerateCostumeData()
    {
        int len = CostumeProvider.Instance.costumes.Count;
        GameObject temp;
        for (int i = 0; i < len; i++)
        {
            temp = GameObject.Instantiate(costumeItemPrefab, costumeCont.transform);
            temp.transform.localScale = Vector3.one;
            temp.transform.localPosition = Vector3.one;

            temp.transform.GetChild(0).GetComponent<Image>().sprite = CostumeProvider.Instance.costumes[i].costume;
            int index = i;

            Button btn = temp.GetComponent<Button>();
            btn.onClick.AddListener(delegate { PreviewCostume(index); });
            btn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        }
    }

    void PreviewCostume(int index)
    {
        currentIndex = index;

        Costume temp = CostumeProvider.Instance.costumes[index];
        costumeViewer.sprite = temp.costume;
        if (temp.unlocked == false)
        {
            priceTxt.text = temp.price.ToString();
            priceTagPanel.SetActive(true);
            equipPanel.SetActive(false);
        } else
        {
            priceTagPanel.SetActive(false);
            equipPanel.SetActive(true);
        }
    }

    public void BuyCostume()
    {
        if (GameManager.Instance.GetStars() >= CostumeProvider.Instance.costumes[currentIndex].price)
        {
            GameManager.Instance.SubstractStars(CostumeProvider.Instance.costumes[currentIndex].price);
            SoundManager.Instance.coinSound.Play();
            CostumeProvider.Instance.costumes[currentIndex].unlocked = true;
            CostumeProvider.Instance.SetChosenCostumeID(currentIndex);
            CostumeProvider.Instance.SaveCostumeData();
        } else
        {
            SoundManager.Instance.falseBtnSound.Play();
        }
        HideDialoguePanelAPI();
    }

    public void EquipCostume()
    {
        CostumeProvider.Instance.SetChosenCostumeID(currentIndex);
        SoundManager.Instance.equipSound.Play();
        HideDialoguePanelAPI();
    }

    public void HideDialoguePanel()
    {
        SoundManager.Instance.falseBtnSound.Play();
        equipPanel.SetActive(false);
        priceTagPanel.SetActive(false);
    }

    void HideDialoguePanelAPI()
    {
        equipPanel.SetActive(false);
        priceTagPanel.SetActive(false);
    }

    IEnumerator IReturnToMainMenu()
    {
        fadeAC.Play("fade_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Lobby");
    }
}

[System.Serializable]
public class Costume
{
    public Sprite costume;
    public int price;
    public bool unlocked = false;
}
