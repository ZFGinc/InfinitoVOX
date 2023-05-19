namespace Boosts
{
    public class RegenHpBoost : IBoost
    {
        private float useTime = 4f;
        private const float _tick = .3f;
        private float _old_tick;

        public float GetUseTime() => useTime;

        public void StartBoosting()
        {
            _old_tick = PlayerStats.TickRegen;
            PlayerStats.TickRegen = _tick;
            Player.Init.PlayVFXHealing();
        }
        public void EndBoosting()
        {
            PlayerStats.TickRegen = _old_tick;
        }
    }
}
