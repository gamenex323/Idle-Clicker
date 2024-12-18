using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Reward
{
    public string Type = "";
    public int Count = 0;
    public string Title = "";
    public string Description = "";
    public string LongDescription = "";
    public string ShortDescription = "";

    public static Reward GetRandomReward(bool bigBonus)
    {
        var reward = new List<Reward>();

        if (bigBonus)
        {
            reward.Add(RewardGold(5));

            var rnd = Random.Range(1, 100);
            if (rnd >= 50 && rnd <= 84)
            {
                reward.Add(RewardGold(10));
                reward.Add(RewardGold(15));
            }

            if (rnd >= 85 && rnd <= 96)
            {
                reward.Add(RewardGold(20));
                reward.Add(RewardGold(25));
            }

            if (rnd >= 97)
            {
                reward.Add(RewardGold(30));
                reward.Add(RewardGold(50));
            }
        }
        else
        {
            reward.Add(RewardGold(1));
            reward.Add(RewardGold(2));
            reward.Add(RewardGold(3));
            reward.Add(RewardTimeJump(2));
            reward.Add(RewardTimeJump(3));
            reward.Add(RewardTimeJump(4));
        }

        return reward[Random.Range(0, reward.Count)];
    }

    public static Reward RewardGold(int count)
    {
        return new Reward { Type = "Gold", Count = count, Title = "Gold", Description = "{0} gold", LongDescription = "You got {0} gold", ShortDescription = "x{0}" };
    }

    public static Reward RewardTimeJump(int count)
    {
        return new Reward { Type = "TimeJump", Count = count, Title = "Time jump", Description = "{0} hours time jump", LongDescription = "You time jumped {0} hours", ShortDescription = "{0} h" };
    }
}