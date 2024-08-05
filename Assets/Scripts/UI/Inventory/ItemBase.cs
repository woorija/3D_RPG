public class ItemBase
{
    public int itemId { get; protected set; } // 아이템고유ID
    public string name { get; protected set; } // 아이템 이름
    public string description { get; protected set; } // 아이템 설명
    public int sellPrice { get; protected set; } // 개당 판매 가격
    public int maxAmount { get; protected set; } // 1칸당 아이템 최대 소지 수
    public int curruntAmount { get; protected set; } // 현재 아이템 소지 수
    public ItemBase()
    {

    }
    public ItemBase(int _itemId, string _name, string _description, int _sellPrice, int _maxAmount = 1, int _curruntAmount = 1)
    {
        itemId = _itemId;
        name = _name;
        description = _description;
        sellPrice = _sellPrice;
        maxAmount = _maxAmount;
        curruntAmount = _curruntAmount;
    }
    public void LoadItem(int _amount = 1)
    {
        curruntAmount = _amount;
    }
    public void ChangeAmount(int _amount)
    {
        curruntAmount += _amount;
        if(curruntAmount <= 0 )
        {
            Reset();
        }
    }
    public virtual void Reset()
    {
        itemId = 0;
        name = string.Empty;
        description= string.Empty;
        sellPrice = 0;
        curruntAmount = 0;
        maxAmount = 0;
    }
}

public class EquipmentItem : ItemBase
{
    public int equipType { get; protected set; } // 장비 장착 위치
    public int attackPower { get; protected set; } // 장비 추가 공격력
    public int defensePower { get; protected set; } // 장비 추가 방어력
    public int hp { get; protected set; } // 장비 추가 체력
    public int mp { get; protected set; } // 장비 추가 마나
    public EquipmentItem()
    {

    }
    public EquipmentItem(int _itemId, string _name, string _description, int _sellPrice, int _attackPower, int _defensePower, int _hp, int _mp) : base(_itemId, _name, _description, _sellPrice)
    {
        equipType = _itemId / 1000;
        attackPower = _attackPower;
        defensePower = _defensePower;
        hp = _hp;
        mp = _mp;
    }
    public void DeepCopy(EquipmentItem _temp)
    {
        _temp.itemId = itemId;
        _temp.name = name;
        _temp.description = description;
        _temp.sellPrice = sellPrice;
        _temp.curruntAmount= curruntAmount;
        _temp.maxAmount= maxAmount;
        _temp.equipType = equipType;
        _temp.attackPower = attackPower;
        _temp.defensePower = defensePower;
        _temp.hp = hp;
        _temp.mp = mp;
    }
    public override void Reset()
    {
        base.Reset();
        equipType = 0;
        attackPower = 0;
        defensePower = 0;
        hp = 0;
        mp = 0;
    }
    public void SetEquipType(int _equipType)
    {
        equipType = _equipType;
    }
}
public class UseableItem : ItemBase
{
    public UseableItem()
    {

    }
    public UseableItem(int _itemId, string _name, string _description, int _sellPrice, int _maxCount, int _curruntCount = 0) : base(_itemId, _name, _description, _sellPrice, _maxCount, _curruntCount)
    {
    }
    public void DeepCopy(UseableItem _temp)
    {
        _temp.itemId = itemId;
        _temp.name = name;
        _temp.description = description;
        _temp.sellPrice = sellPrice;
        _temp.maxAmount = maxAmount;
        _temp.curruntAmount = curruntAmount;
    }
    public override void Reset()
    {
        base.Reset();
    }
}
public class MiscItem : ItemBase
{
    public MiscItem()
    {

    }
    public MiscItem(int _itemId, string _name, string _description, int _sellPrice, int _maxCount, int _curruntCount = 0) : base(_itemId, _name, _description, _sellPrice, _maxCount, _curruntCount)
    {

    }
    public void DeepCopy(MiscItem _temp)
    {
        _temp.itemId = itemId;
        _temp.name = name;
        _temp.description = description;
        _temp.sellPrice = sellPrice;
        _temp.maxAmount = maxAmount;
        _temp.curruntAmount = curruntAmount;
    }
    public override void Reset() 
    {
        base.Reset();
    }
}
