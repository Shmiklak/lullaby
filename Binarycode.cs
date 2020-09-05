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
    public class Binarycode : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 Color = Color4.White;
        public override void Generate()
        {

            var beat = 134293 - 133946;

            for (int i = 0; i < 29; i++) {
                for (int j = 0; j<16; j++) {

                    var position = new Vector2(-117 + 30 * i, 0 + j * 30);

                    var number = GetLayer("binary_background").CreateAnimation("sb/etc/z.png", 2, Random(100, 1000), OsbLoopType.LoopForever, OsbOrigin.Centre, position);
                    number.Fade(133946, 1);
                    number.Fade(178339, 0);
                    number.Scale(133946, 0.3);
                    number.Color(133946, Color);
                }
            }

            var flare = GetLayer("cube_flare").CreateSprite("sb/etc/flare.jpg", OsbOrigin.Centre);
            flare.Additive(133946, 178339);
            flare.Rotate(133946, 178339, 0, 10);
            flare.Scale(133946, 0.4);

            var flare2 = GetLayer("cube_flare").CreateSprite("sb/etc/flare.jpg", OsbOrigin.Centre);
            flare2.Additive(133946, 178339);
            flare2.Rotate(133946, 178339, 0, -10);
            flare2.Scale(133946, 0.3);
        }
    }
}
