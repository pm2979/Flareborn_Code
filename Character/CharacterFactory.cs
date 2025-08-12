public static class CharacterFactory
{
    private static CharacterDataLoader characterDataLoader;
    private static JobDataLoader jobDataLoader;

    public static bool IsInitialized => characterDataLoader != null && jobDataLoader != null;

    public static void Init(CharacterDataLoader charLoader, JobDataLoader jobLoader)
    {
        characterDataLoader = charLoader;
        jobDataLoader = jobLoader;
    }

    public static CharacterInstance CreateCharacter(int characterKey) // Key로 캐릭터 생성
    {
        CharacterData charData = characterDataLoader.GetByKey(characterKey);
        if (charData == null)
        {
            Logger.LogError($"[CharacterFactory] 캐릭터 데이터를 찾을 수 없습니다: key={characterKey}");
            return null;
        }

        JobData jobData = jobDataLoader.GetByKey((int)charData.JobType);
        if (jobData == null)
        {
            Logger.LogError($"[CharacterFactory] 직업 데이터를 찾을 수 없습니다: jobType={(int)charData.JobType} (CharacterKey: {characterKey})");
            return null;
        }

        return new CharacterInstance(charData, jobData);
    }
}
