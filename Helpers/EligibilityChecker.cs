using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectorOfScheme.Helpers
{
    using System;
    using System.Linq;

    namespace StudentAccidentClaim.Helpers
    {
        public class EligibilityChecker
        {
            /// <summary>
            /// Checks eligibility of student as per Government Resolution.
            /// </summary>
            /// <param name="standard">Class of student (1–12)</param>
            /// <param name="accidentType">Type of accident</param>
            /// <returns>Tuple (IsEligible, Message)</returns>
            public static (bool, string) CheckEligibility(int standard, string accidentType)
            {
                // Rule 1: Only 1st to 12th standard students
                if (standard < 1 || standard > 12)
                {
                    return (false, "❌ Not Eligible: Student must be between Class 1 and 12.");
                }

                // Rule 2: Excluded Accident Types
                string[] excluded = {
                "Suicide",
                "Suicide Attempt",
                "Self Harm",
                "Crime Accident",
                "Drug Influence",
                "Natural Death",
                "Motor Accident"
            };

                if (excluded.Contains(accidentType))
                {
                    return (false, "❌ Not Eligible: Accident type not covered under scheme.");
                }

                // ✅ Eligible
                return (true, "✅ Eligible under scheme.");
            }
        }
    }
}