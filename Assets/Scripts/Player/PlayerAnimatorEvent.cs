using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
    [SerializeField] PlayerEffect playerEffectList;
    [SerializeField] Player_Land stateLand;
    [SerializeField] PlayerController controller;
    [SerializeField] HitUtility hitUtility;

    public void SetSkillDamage(int _index)
    {
        hitUtility.SetSkillDamage(_index);
    }
    public void EffectPlay(int _index)
    {
        playerEffectList.PlayEffect(_index);
    }
    public void IsNormalAttackHit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.25f, 120f, 3f, 2, 1f);
    }
    public void IsS100001Hit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.75f, 120f, 3f, 2, 1f);
    }
    public void IsS100002Hit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.75f, 120f, 3f, 4, 1f);
    }
    public void IsS210001Hit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        centerPos += controller.transform.forward * 3;
        hitUtility.BoxHit(centerPos, new Vector3(0.8f, 1f, 1.5f), controller.transform.localRotation, 3, 1f);
    }
    public void IsS210003_1Hit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularHit(centerPos, 1.25f, 3f, 5, 1f);
    }
    public void IsS210003_2Hit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.25f, 120f, 3f, 1, 1f);
    }
    public void IsS210005Hit()
    {
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        centerPos += controller.transform.forward * 3;
        hitUtility.BoxHit(centerPos, new Vector3(0.8f, 1f, 2f), controller.transform.localRotation, 1, 1f);
    }
    public void LandMotionSkip()
    {
        stateLand.OnMotionSkip();
    }
    public void AnimationEnd()
    {
        controller.AnimationEnd();
    }
}
