using ComponentHandlerLibrary.Utils;
using ScintillaNET;

namespace LexerStyleLibrary.Styles
{
    public class LoggingStyle
    {
        public const int StyleDefault = 0,
                     StyleKeyword = 1,
                  StyleIdentifier = 2,
                      StyleNumber = 3,
                      StyleString = 4,
                     StyleDebug = 5,
                     StyleMessage = 6,
                        StyleInfo = 7,
                     StyleWarning = 8,
                       StyleError = 9,
                        StyleTime = 10;


        private const int STATE_UNKNOWN = 0,
                            STATE_DEBUG = 5,
                          STATE_MESSAGE = 6,
                             STATE_INFO = 7,
                          STATE_WARNING = 8,
                            STATE_ERROR = 9,
                            STATE_TIME = 10;

        //For Number Margin.
        private const int BACK_COLOR = 0x2A211C;
        private const int FORE_COLOR = 0xB7B7B7;
        private const int NUMBER_MARGIN = 1;


        //public LoggingStyle(Scintilla inScintilla) => keywords = inScintilla.DescribeKeywordSets();

        public void Style(Scintilla scintilla, int startPos, int endPos)
        {

            var length = 0;
            var state = STATE_UNKNOWN;

            scintilla.StartStyling(startPos);
            {
                while (startPos < endPos)
                {

                    char c = (char)scintilla.GetCharAt(startPos),
                         d = startPos > 1 ? (char)scintilla.GetCharAt(startPos + 1) : default;
                    //REPROCESS:
                    switch (state)
                    {
                        case STATE_UNKNOWN:
                            //STATES: LOGTYPES
                            if (c == '[' && char.IsDigit(d) == false)
                            {
                                //STATE_DEBUG
                                if (d == 'd' || d == 'D')
                                {
                                    scintilla.SetStyling(1, StyleDebug);
                                    state = STATE_DEBUG;
                                }
                                //STATE_INFO
                                if (d == 'i' || d == 'I')
                                {
                                    scintilla.SetStyling(1, StyleInfo);
                                    state = STATE_INFO;
                                }

                                //STATE_MESSAGE
                                if (d == 'm' || d == 'M')
                                {
                                    scintilla.SetStyling(1, StyleMessage);
                                    state = STATE_MESSAGE;
                                }

                                //STATE_WARNING
                                if (d == 'w' || d == 'W')
                                {
                                    scintilla.SetStyling(1, StyleWarning);
                                    state = STATE_WARNING;
                                }

                                //STATE_ERROR
                                if (d == 'e' || d == 'E')
                                {
                                    scintilla.SetStyling(1, StyleError);
                                    state = STATE_ERROR;
                                }
                            }

                            //STATES: LOGTIME
                            //STATE_TIME
                            else if (c == '[' && char.IsDigit(d) == true)
                            {
                                scintilla.SetStyling(1, StyleTime);
                                state = STATE_TIME;

                            }

                            // STATES: EVERYTHING ELSE, DEFAULT.
                            else
                            {
                                scintilla.SetStyling(1, StyleDefault);
                            }
                            break;

                        case STATE_DEBUG:
                            if (c == ']')
                            {
                                length++;
                                scintilla.SetStyling(length, StyleDebug);
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else { length++; }
                            break;

                        case STATE_INFO:
                            if (c == ']')
                            {
                                length++;
                                scintilla.SetStyling(length, StyleInfo);
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else { length++; }
                            break;

                        case STATE_MESSAGE:
                            if (c == ']')
                            {
                                length++;
                                scintilla.SetStyling(length, StyleMessage);
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else { length++; }
                            break;


                        case STATE_WARNING:
                            if (c == ']')
                            {
                                length++;
                                scintilla.SetStyling(length, StyleWarning);
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else { length++; }
                            break;

                        case STATE_ERROR:
                            if (c == ']')
                            {
                                length++;
                                scintilla.SetStyling(length, StyleError);
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else { length++; }
                            break;

                        case STATE_TIME:
                            if (c == ']')
                            {
                                length++;
                                scintilla.SetStyling(length, StyleTime);
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else { length++; }
                            break;
                    }

                    startPos++;
                }

            }
        }

        public void InitNumberMargin(Scintilla scintilla)
        {

            scintilla.Styles[ScintillaNET.Style.LineNumber].BackColor = Util.IntToColor(BACK_COLOR);
            scintilla.Styles[ScintillaNET.Style.LineNumber].ForeColor = Util.IntToColor(FORE_COLOR);
            scintilla.Styles[ScintillaNET.Style.IndentGuide].ForeColor = Util.IntToColor(FORE_COLOR);
            scintilla.Styles[ScintillaNET.Style.IndentGuide].BackColor = Util.IntToColor(BACK_COLOR);

            Margin nums = scintilla.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            //scintilla.MarginClick += TextArea_MarginClick;
        }
    }
}
