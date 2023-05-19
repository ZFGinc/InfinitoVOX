namespace Boosts
{
    public class AddDamageBoost : IBoost
    {
        private float useTime = 3f;
        private const float _damage = 5f;
        private uint _oldLevel;

        public float GetUseTime() => useTime;

        public void StartBoosting()
        {
            _oldLevel = PlayerStats.Level;
            PlayerStats.BaseDamage += _damage * _oldLevel;
        }

        public void EndBoosting()
        {
            PlayerStats.BaseDamage -= _damage * _oldLevel;
        }
    }
}
