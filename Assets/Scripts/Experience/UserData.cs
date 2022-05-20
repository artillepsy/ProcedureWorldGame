namespace Experience
{
    [System.Serializable]
    public class UserData
    {
        public int GrenadeCount = 0;
        public int FreezersCount = 0;
        public int ExpCount = 0;

        public UserData(int grenadeCount, int freezersCount, int expCount)
        {
            GrenadeCount = grenadeCount;
            FreezersCount = freezersCount;
            ExpCount = expCount;
        }
    }
}