using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Credits : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    var logo = GetLayer("credits").CreateSprite("sb/credits/receptor.png", OsbOrigin.Centre);
            logo.Fade(OsbEasing.In, 22963, 23310, 0, 1);
            logo.Fade(OsbEasing.Out, 34061 - 600, 34755 - 600, 1, 0);
            logo.Scale(22963, 0.4);
            logo.MoveY(OsbEasing.In, 25738, 26084, 240, 200);
        }
    }
}
