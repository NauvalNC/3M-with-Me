using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JigsawPiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject root;
    private Transform origin;
    private Image sprite;

    private Transform matchTransform;

    public void SetSprite(Sprite sprite)
    {
        if (this.sprite == null) this.sprite = GetComponent<Image>();
        this.sprite.sprite = sprite;
    }

    public void SetRoot(GameObject root) { this.root = root; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SoundManager.Instance.tapSound.Play();

        origin = transform.parent;
        transform.SetParent(root.transform);
        transform.localScale = Vector3.one;
        transform.position = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(origin);
        transform.SetSiblingIndex(0);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        MatchPiece(matchTransform);
    }

    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        matchTransform = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string colName = collision.gameObject.name;
        matchTransform = (colName == name) ? collision.transform : null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        matchTransform = null;
    }

    void MatchPiece(Transform slot)
    {
        if (matchTransform == null)
        {
            SoundManager.Instance.tapSound.Play();
            return;
        }

        SoundManager.Instance.equipSound.Play();

        transform.SetParent(slot);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        JigsawPuzzleManager.Instance.StepSolvePuzzle();
        enabled = false;
    }
}
