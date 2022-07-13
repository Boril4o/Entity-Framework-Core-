using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.Constraints
{
    public static class GlobalConstraints
    {
        //Theatre
        public const int THEATRE_NAME_MAX_LENGTH = 30;

        public const int THEATRE_NAME_MIN_LENGTH = 4;

        public const int THEATRE_DIRECTOR_MAX_LENGTH = 30;

        public const int THEATRE_DIRECTOR_MIN_LENGTH = 4;

        public const string THEATRE_NUMBER_OF_HALLS_MIN = "1";

        public const string THEATRE_NUMBER_OF_HALLS_MAX = "10";

        //Play
        public const int PLAY_TITLE_MAX_LENGTH = 50;

        public const int PLAY_TITLE_MIN_LENGTH = 4;

        public const int PLAY_DESCRIPTION_MAX_LENGTH = 700;

        public const int PLAY_SCREEN_WRITER_MAX_LENGTH = 30;

        public const int PLAY_SCREEN_WRITER_MIN_LENGTH = 4;

        public const float PLAY_RATING_MIN = 0.00f;

        public const float PLAY_RATING_MAX = 10.00f;

        public const int PLAY_DURATION_MIN_HOURS = 1;

        //Cast
        public const int CAST_FULL_NAME_MAX_LENGTH = 30;

        public const int CAST_FULL_NAME_MIN_LENGTH = 4;

        public const int CAST_PHONE_NUMBER_MAX_LENGTH = 15;

        public const int CAST_PHONE_NUMBER_MIN_LENGTH = 15;

        //Ticket
        public const string TICKET_PRICE_MIN = "1.00";

        public const string TICKET_PRICE_MAX = "100.00";

        public const string TICKET_ROW_NUMBER_MAX = "10";

        public const string TICKET_ROW_NUMBER_MIN = "1";
    }
}
