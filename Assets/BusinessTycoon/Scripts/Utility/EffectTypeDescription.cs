public class EffectTypeDescription
{
    public static string GetDescription(string effectType, double effect, string businessName, string pluralName)
    {
        var description = "";

        switch (effectType)
        {
            case "Run":
                description = "Runs " + pluralName;
                break;
            case "Buy":
                description = "+" + effect + " " + pluralName;
                break;
            case "Cost":
                if (pluralName == "Everything")
                {
                    description = pluralName + " is " + ((float)effect * 100) + "% cheaper";
                }
                else
                {
                    description = pluralName + " are " + ((float)effect * 100) + "% cheaper";
                }
                break;
            case "Profit":
            case "Multiplier":
                description = "Profit of " + pluralName + " x" + (double)effect;
                break;
            case "Speed":
                description = "Speed of " + pluralName + " x" + (1 / (float)effect);
                break;
            case "Gold":
                description = effect + " Gold";
                break;
            case "TimeJump":
                description = effect + " Hour time jump";
                break;
            case "Luck":
                description = "Try your luck";
                break;
            case "iEffect":
                description = "Investor effectiveness +" + effect * 100 + "%";
                break;
        }

        return description;
    }
}