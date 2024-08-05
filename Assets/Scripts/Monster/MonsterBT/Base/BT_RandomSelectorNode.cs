using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_RandomSelectorNode : BT_CompositeNode
{
    [SerializeField] protected int[] randomPercentage;
    List<int> exceptNumber = new List<int>();
    public override BTResult Execute()
    {
        if (children == null || children.Count == 0) return BTResult.Failure;

        exceptNumber.Clear();

        for (int i = 0; i < children.Count; i++)
        {
            int sumRandomValue = randomCalc(); // 남은 자식노드의 랜덤값 합계
            int randomValue = Random.Range(0, sumRandomValue); // 0~합계랜덤값중 랜덤으로 값 정하기
            int executeIndex = randomIndex(randomValue);

            switch (children[executeIndex].Execute())
            {
                case BTResult.Running:
                    return BTResult.Running;
                case BTResult.Success:
                    return BTResult.Success;
                case BTResult.Failure: //실패시 해당 인덱스 예외처리
                    exceptNumber.Add(executeIndex);
                    break;
            }
        }
        return BTResult.Failure;
    }

    int randomCalc()
    {
        int sum = 0;
        for(int i=0;i<randomPercentage.Length;i++)
        {
            if (exceptNumber.Contains(i)) continue;
            sum += randomPercentage[i];
        }
        return sum;
    }
    int randomIndex(int _rand)
    {
        int sum = 0;
        for(int i = 0; i < randomPercentage.Length; i++)
        {
            if (!exceptNumber.Contains(i))
            {
                sum += randomPercentage[i];
                if(_rand < sum)
                {
                    return i;
                }
            }
        }
        return 0;
    }
}
