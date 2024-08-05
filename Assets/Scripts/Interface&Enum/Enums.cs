public enum GameMode
{
    ControllMode = 0,
    UIMode,
    ForcedUIMode,
    NotControllable
}

public enum StateType
{
    Idle,
    Walk,
    Run,
    Dash,
    Jump,
    Fall,
    Land,
    Attack,
    Buff,
    ActiveSkill,
    Hit,
    Die
}

public enum DragUIType
{
    Inventory = 0,
    Equipment,
    Skill
}

public enum ItemType
{
    Equipment = 0,
    Useable,
    Misc
}

public enum CraftType
{
    Materials = 0,
    Result
}

public enum EquipmentType
{
    Weapon = 0,
    Armor,
    Pants,
    Gloves,
    Shoes,
    Ring
}

public enum StatusType
{
    Level = 0,
    AttackPoint,
    DefensePoint,
    Hp,
    Mp
}

public enum BTResult
{
    Success,
    Running,
    Failure
}

public enum QuestType
{
    Talk = 1,
    Hunt,
    Collect,
    Mix // Hunt+Collect
}

public enum QuestProgress
{
    NotStarted,
    InProgress,
    Completed
}

public enum UseType
{
    Null,
    Item,
    Skill
}
public enum BuffType
{
    Null,
    HpRecovery,
    MpRecovery,
    HpPercentRecovery,
    MpPercentRecovery,
    MaxHpIncrease,
    MaxMpIncrease,
    AttackPowerIncrease,
    DefensePowerIncrease
}