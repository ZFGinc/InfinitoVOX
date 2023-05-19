namespace Boosts
{
    public class PutCDShootingBoost : IBoost
    {
        private float useTime = 6f;
        private const float _newBaseCoolDown = 0.1f;
        private float _oldBaseCoolDown;

        public float GetUseTime() => useTime;

        public void StartBoosting()
        {
            _oldBaseCoolDown = PlayerStats.BaseCoolDownForShooting;
            PlayerStats.BaseCoolDownForShooting = _newBaseCoolDown;
        }

        public void EndBoosting()
        {
            PlayerStats.BaseCoolDownForShooting = _oldBaseCoolDown;
        }
    }
}
