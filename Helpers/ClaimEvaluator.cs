using System;

namespace DirectorOfScheme.Helpers
{
    public static class ClaimEvaluator
    {
        public static (decimal, string) EvaluateClaim(string accidentType)
        {
            if (string.IsNullOrWhiteSpace(accidentType))
                return (0, "❌ Invalid Accident Type");

            switch (accidentType.Trim().ToLower())
            {
                case "death":
                    return (150000, "Accidental Death – Eligible for ₹1,50,000");

                case "disabilityfull":
                    return (100000, "Major Disability – Eligible for ₹1,00,000");

                case "disabilitypartial":
                    return (75000, "Minor Disability – Eligible for ₹75,000");

                case "surgery":
                    return (100000, "Surgery – Eligible up to ₹1,00,000");

                case "injury":
                    return (100000, "Injury – Eligible up to ₹1,00,000");

                default:
                    return (0, "❌ Not Eligible under scheme");
            }
        }
    }
}
