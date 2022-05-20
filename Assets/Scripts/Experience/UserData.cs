namespace Experience
{
    [System.Serializable]
    public class UserData
    {
        public int GrenadeCount = 0;
        public int ExpCount = 0;

        public UserData(int grenadeCount, int expCount)
        {
            GrenadeCount = grenadeCount;
            ExpCount = expCount;
        }
    }
}