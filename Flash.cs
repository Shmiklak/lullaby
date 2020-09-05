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
    public class Flash : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var beat = (134293 - 133946) * 2;

		    var EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            int[] timeStamps = {133946, 136720, 139495, 145044, 147819, 150593, 156142, 158917, 161691, 167240, 170015, 172790, 178339, 183888, 189437, 194986, 200535, 206084, 211634, 217183, 222732};
            foreach (var stamp in timeStamps)
            {
                var flash = GetLayer("Flashes").CreateSprite("sb/etc/p.png", OsbOrigin.Centre);
                flash.Scale(stamp, 1000);
                flash.Additive(stamp, stamp+beat);
                flash.Fade(stamp, stamp+beat, 0.7, 0);
            }
        }
    }
}
