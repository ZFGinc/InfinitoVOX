namespace Boosts
{
    public class AddMoveSpeedBoost : IBoost
    {
        private float useTime = 3f;
        private const float _Speed = 5f;

        public float GetUseTime() => useTime;

        public void StartBoosting()
        {
            PlayerStats.TempBoostSpeedMove(PlayerStats.Speed + _Speed);
        }
        public void EndBoosting()
        {
            PlayerStats.Speed -= _Speed;
        }
    }
}
