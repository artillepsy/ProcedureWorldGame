namespace Enemy
{
    public interface IOnEnemyStateChange
    {
        public void OnStateChange(State newEnemyState);
    }
}