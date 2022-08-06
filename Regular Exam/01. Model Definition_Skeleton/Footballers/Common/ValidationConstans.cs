using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.Common
{
    public class ValidationConstans
    {
        public const int FOTBALLER_NAME_MAX_LENGTH = 40;

        public const int FOTBALLER_NAME_MIN_LENGTH = 2;

        //TEAM

        public const int TEAM_NAME_MAX_LENGTH = 40;

        public const int TEAM_NAME_MIN_LENGTH = 3;

        public const string TEAM_NAME_REGEX = @"[A-Za-z\d .-]+";

        public const int TEAM_NATIOANALITY_MAX_LENGTH = 40;

        public const int TEAM_NATIOANALITY_MIN_LENGTH = 2;

        public const string TEAM_TROPHIES_MIN = "0";

        public const string TEAM_TROPHIES_MAX = "2147483647";

        //COACH
        public const int COACH_NAME_MAX_LENGTH = 40;

        public const int COACH_NAME_MIN_LENGTH = 2;
    }
}
