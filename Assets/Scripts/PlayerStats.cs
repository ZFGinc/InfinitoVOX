public static class PlayerStats
{
    private const float _maxMoveSpeedPlayer = 13f;
    private const float _minCoolDownForShooting = 0.1f;
    private const float _regenHpOnTick = 5;

    private static float _maxhp;
    private static float _hp;
    private static uint _exp;
    private static uint _level;
    private static float _speed;
    private static float _cooldownregenHp;
    private static float _baseCooldownForShooting;

    public static float BaseDamage;
    public static int BaseAdderCoins { get; private set; } = 1;

    public static float MaxHp 
    { 
        get => _maxhp; 
    }
    public static float Hp 
    { 
        get => _hp; 
    }
    public static uint Exp 
    { 
        get => _exp; 
        set { setExp(value); } 
    }
    public static uint Level 
    { 
        get => _level; 
        private set { _level = value; } 
    }
    public static float Speed 
    { 
        get => _speed; 
        set { _speed = (value > _maxMoveSpeedPlayer) ? _maxMoveSpeedPlayer : value; } 
    }
    public static float TickRegen 
    {
        get => _cooldownregenHp;
        set { _cooldownregenHp = valid_regen_tick(value); }
    }
    public static float BaseCoolDownForShooting 
    { 
        get => _baseCooldownForShooting; 
        set { _baseCooldownForShooting = (value < _minCoolDownForShooting) ? _minCoolDownForShooting : value; } 
    }
    public static uint NeedExpToUpLvl 
    { 
        get => (_level >= 10) ? 10 : _level + 1;
    }

    public static void Initialization(bool[] startCards)
    {
        _maxhp = 100 + (startCards[0] ? 50 : 0);
        _hp = _maxhp;
        _exp = 0;
        _level = 1;
        _speed = 7f + (startCards[1] ? 1 : 0);
        _cooldownregenHp = 3f + (startCards[3] ? -0.5f : 0);
        BaseDamage = 3 + (startCards[2] ? 20 : 0);
        _baseCooldownForShooting = .7f;
        BaseAdderCoins = (startCards[4] ? 2 : 1);

        SetDefaultUI();
    }

    public static void upExpOne()
    {
        _exp++;
        UpdateUIExperians();
    }
    public static void setExp(uint value)
    {
        _exp = value;
        UpdateUIExperians();
    }

    public static void upMaxHp(float addition)
    {
        if(_hp == _maxhp) _hp += addition;
        _maxhp += addition;
        UpdateUIHealth();
    }
    public static void regenHp()
    {
        if (_hp >= _maxhp) { _hp = MaxHp; return; }
        _hp += _regenHpOnTick;
        UpdateUIHealth();
    }
    public static void upRegenTick()
    {
        if (_cooldownregenHp < 0.2f) { _cooldownregenHp = 0.2f; return; }
        _cooldownregenHp -= 0.1f;
    }
    private static float valid_regen_tick(float value)
    {
        if (value < 0.2f) return 0.2f;

        return value;
    }

    private static void UpdateUIExperians()
    {
        UIDataPlayer.Instance.UpdateExperiansSlider(_exp);
        if (_exp == NeedExpToUpLvl) UIDataPlayer.Instance.ShowLvlupWindow();
    }
    private static void UpdateUIHealth()
    {
        UIDataPlayer.Instance.UpdateMaxHealthSlider(_maxhp);
        UIDataPlayer.Instance.UpdateHealthSlider(_hp);
    }

    public static void ResetExp()
    {
        _exp -= NeedExpToUpLvl;
        _level++;
        UIDataPlayer.Instance.UpdateExperiansSlider(_exp);
        UIDataPlayer.Instance.UpdateMaxExperiansSlider(NeedExpToUpLvl);
        UIDataPlayer.Instance.UpdateCurrentLevelText(_level);
    }
    public static void GetDamage(float value)
    {
        _hp -= value;
        UpdateUIHealth();
    }

    public static void TempBoostSpeedMove(float value) => _speed = value;
    public static void FullHP()
    {
        _hp = _maxhp;
        UIDataPlayer.Instance.UpdateHealthSlider(_hp);
    }

    private static void SetDefaultUI()
    {
        if (UIDataPlayer.Instance == null) return;

        UIDataPlayer.Instance.UpdateMaxHealthSlider(_maxhp);
        UIDataPlayer.Instance.UpdateHealthSlider(_hp);
        UIDataPlayer.Instance.UpdateExperiansSlider(_exp);
        UIDataPlayer.Instance.UpdateMaxExperiansSlider(NeedExpToUpLvl);
        UIDataPlayer.Instance.UpdateCurrentLevelText(_level);
    }
}
