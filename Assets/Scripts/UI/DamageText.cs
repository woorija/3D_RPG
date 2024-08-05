using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using DG.Tweening;

public class DamageText : PoolBehaviour<DamageText>
{
    [SerializeField] TMP_Text text;
    
    private Sequence mySequence;

    float moveYpos;

    [SerializeField] bool isPlayerDamage;
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
    private void Start()
    {
        mySequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(transform.DOMoveY(moveYpos, 0.3f).SetEase(Ease.OutCubic))
            .Join(transform.DOScale(1f, 0.3f).SetEase(Ease.OutCubic))
            .Join(text.DOFade(1f,0.3f).SetEase(Ease.OutCubic))
            .Append(transform.DOScale(0.2f, 0.6f).SetEase(Ease.InQuad))
            .Join(text.DOFade(0.2f, 0.6f).SetEase(Ease.InQuad))
            .OnComplete(ReturnPool);
    }
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    public void SetPos(Vector3 _pos)
    {
        _pos.y += 1f;
        transform.position = _pos;
        moveYpos = _pos.y + 0.6f;
    }
    public void SetText(int _damage)
    {
        text.text = _damage.ToString();
        SetText();
    }
    public void SetText(int _damage,string _addText)
    {
        text.text = string.Format("{0}{1}", _damage.ToString(), _addText);
        SetText();
    }
    void SetText()
    {
        text.ForceMeshUpdate();
        text.alpha = 0;
        mySequence.Restart();
    }
    void ReturnPool()
    {
        Release(this);
    }
}
