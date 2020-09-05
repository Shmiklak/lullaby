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
    public class Error : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var error = GetLayer("error").CreateSprite("sb/credits/error.png", OsbOrigin.Centre);
            error.Fade(OsbEasing.In, 111749, 112443, 0, 1);
            error.Fade(122154, 122847, 1, 0);
            error.Scale(111749, 0.45);

            var loader = GetLayer("error").CreateSprite("sb/etc/load.png", OsbOrigin.Centre, new Vector2(320, 345));
            loader.Fade(OsbEasing.In, 111749, 112443, 0, 1);
            loader.Fade(122154, 122847, 1, 0);
            loader.Scale(111749, 0.1);
            loader.Rotate(111749, 122847, 0, 30);


            var recoverMessage = GetLayer("error").CreateSprite("sb/credits/recovery.png", OsbOrigin.Centre);
            recoverMessage.Fade(OsbEasing.In, 122847, 123194, 0, 1);
            recoverMessage.Fade(129784, 130477, 1, 0);
            recoverMessage.Scale(122847, 0.45);

            var red = GetLayer("error").CreateSprite("sb/etc/p.png", OsbOrigin.Centre);
            red.Fade(OsbEasing.InOutBounce, 122847, 131171, 0, 0.8);
            red.Fade(132905, 133946, 0.8, 0);
            red.ColorHsb(122847, 0, 1, 0.5);
            red.Scale(122847, 1000);

            var fine = GetLayer("error").CreateSprite("sb/credits/fine.png", OsbOrigin.Centre);
            fine.Fade(OsbEasing.In, 222732, 223425, 0, 1);
            fine.Fade(232443, 233830, 1, 0);
            fine.Scale(122847, 0.45);
        }
    }
}
