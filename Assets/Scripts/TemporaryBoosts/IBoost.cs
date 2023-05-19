namespace Boosts
{
    public interface IBoost
    {
        public float GetUseTime();
        public void StartBoosting();
        public void EndBoosting();
    }
}
