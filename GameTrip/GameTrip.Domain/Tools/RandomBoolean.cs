namespace GameTrip.Domain.Tools;
public static class RandomBoolean
{
    public static bool RandomBool()
    {
        Random random = new();

        int randomInt = random.Next(2);
        return Convert.ToBoolean(randomInt);
    }
}
