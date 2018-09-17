using System.Collections.Generic;
using QuestionBot.CommandSystem;

namespace project.CommandSystem
{
    public static class CommandOverview
    {
        public static List<(string, PermissionLevel)> CommandsStreamer = new List<(string, PermissionLevel)>()
        {
            ("config", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel-print", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel-print-streamer", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel-print-moderator", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel-print-everyone", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel-set", PermissionLevel.Streamer ),
            ("config-commandpermissionlevel-set-command-platform-permissionlevel", PermissionLevel.Streamer ),
            ("config-discordmoderator", PermissionLevel.Streamer ),
            ("config-discordmoderator-add-id", PermissionLevel.Streamer ),
            ("config-discordmoderator-print", PermissionLevel.Streamer ),
            ("config-discordmoderator-remove-id", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-print", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-both", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-bycommand", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-bykeywords", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-default", PermissionLevel.Streamer ),
            ("config-twitchmoderator", PermissionLevel.Streamer ),
            ("config-twitchmoderator-disable", PermissionLevel.Streamer ),
            ("config-twitchmoderator-enable", PermissionLevel.Streamer ),
            ("config-twitchmoderator-print", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-print", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-set", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-set-char", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-set-default", PermissionLevel.Streamer ),
            ("question", PermissionLevel.everyone ),
            ("question-answered", PermissionLevel.Moderator ),
            ("question-answered-all", PermissionLevel.Moderator ),
            ("question-answered-id", PermissionLevel.Moderator ),
            ("question-print", PermissionLevel.Moderator ),
            ("question-print-all", PermissionLevel.Moderator ),
            ("question-print-answered", PermissionLevel.Moderator ),
            ("question-print-id", PermissionLevel.Moderator ),
            ("question-print-unanswered", PermissionLevel.Moderator ),
            ("question-remove", PermissionLevel.Streamer ),
            ("question-remove-all", PermissionLevel.Streamer ),
            ("question-remove-answered", PermissionLevel.Streamer ),
            ("question-remove-id", PermissionLevel.Streamer ),
            ("question-remove-unanswered", PermissionLevel.Streamer ),
            ("question-unanswered", PermissionLevel.Moderator ),
            ("question-unanswered-all", PermissionLevel.Moderator ),
            ("question-unanswered-id", PermissionLevel.Moderator ),
        };

        public static List<(string, PermissionLevel)> CommandsModerator = new List<(string, PermissionLevel)>()
        {
            ("config", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-print", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-both", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-bycommand", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-bykeywords", PermissionLevel.Streamer ),
            ("config-questionrecognitionmode-set-default", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-print", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-set", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-set-char", PermissionLevel.Streamer ),
            ("config-twitchquestioncommandprefix-set-default", PermissionLevel.Streamer ),
            ("question", PermissionLevel.everyone ),
            ("question-answered", PermissionLevel.Moderator ),
            ("question-answered-all", PermissionLevel.Moderator ),
            ("question-answered-id", PermissionLevel.Moderator ),
            ("question-print", PermissionLevel.Moderator ),
            ("question-print-all", PermissionLevel.Moderator ),
            ("question-print-answered", PermissionLevel.Moderator ),
            ("question-print-id", PermissionLevel.Moderator ),
            ("question-print-unanswered", PermissionLevel.Moderator ),
            ("question-remove", PermissionLevel.Streamer ),
            ("question-remove-all", PermissionLevel.Streamer ),
            ("question-remove-answered", PermissionLevel.Streamer ),
            ("question-remove-id", PermissionLevel.Streamer ),
            ("question-remove-unanswered", PermissionLevel.Streamer ),
            ("question-unanswered", PermissionLevel.Moderator ),
            ("question-unanswered-all", PermissionLevel.Moderator ),
            ("question-unanswered-id", PermissionLevel.Moderator ),
        };

        public static List<(string, PermissionLevel)> CommandsEveryone = new List<(string, PermissionLevel)>()
        {
            ("question", PermissionLevel.everyone ),
            ("question-print", PermissionLevel.Moderator ),
            ("question-print-all", PermissionLevel.Moderator ),
            ("question-print-answered", PermissionLevel.Moderator ),
            ("question-print-id", PermissionLevel.Moderator ),
            ("question-print-unanswered", PermissionLevel.Moderator ),
        };
    }
}
