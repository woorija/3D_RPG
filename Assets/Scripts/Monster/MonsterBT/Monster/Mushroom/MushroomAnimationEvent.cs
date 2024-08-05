using UnityEngine;

public class MushroomAnimationEvent : GenericMonsterAnimationEvent<MushroomBT, MushroomBlackBoard>
{
    public void DashSkillEvent()
    {
        blackBoard.DashSkillAttack();
    }
    public void JumpSkillEvent()
    {
        blackBoard.JumpSkillAttack();
    }
}
