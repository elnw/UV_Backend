using System;

namespace UV_Backend.Helpers.Logic
{
    public static class LogicHelpers
    {
        public static string GetDescriptionForScore(int score)
        {
            if (score >= 0 && score <= 6)
                return "I";
            else if (score >= 7 && score <= 13)
                return "II";
            else if (score >= 14 && score <= 20)
                return "III";
            else if (score >= 21 && score <= 27)
                return "IV";
            else if (score >= 28 && score <= 34)
                return "V";
            else 
                return "VI";
        }

        public static DateTime ConvertUTCToDatetime(long UTC)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(UTC).UtcDateTime, easternZone);
        }

        public static int GetMED(string scoreDescription)
        {
            switch (scoreDescription) {
                case "I":
                    return 25;
                case "II":
                    return 33;
                case "III":
                    return 40;
                case "IV":
                    return 50;
                case "V":
                    return 75;
                case "VI":
                    return 120;
                default:
                    return 0;
            }
        }

      
    }
}
