using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace StorybrewScripts
{
    public class Tpazol : StoryboardObjectGenerator
    {
        private FontGenerator Font;
        public override void Generate()
        {
            var bg = GetLayer("0").CreateSprite("bg.jpg", OsbOrigin.Centre);
            bg.Scale(0, 0, 0, 0);
            SetFont();
            GenerateParts();
        }

        public void GenerateParts()
        {
            int id = 0;
            var lines = File.ReadAllLines(ProjectPath + "/sections.txt");
            var lines2 = File.ReadAllLines(ProjectPath + "/sections2.txt");
            int oldBookmark = 0;
            int titleStartTime = 0;

            foreach(var bookmark in Beatmap.Bookmarks)
            {
                Log(id.ToString());
                
                if(id != 0)
                {
                    string songName = lines[id - 1];
                    string songName2 = lines2[id - 1];
                    
                    GenerateBackground(oldBookmark, bookmark, id);
                    GenerateText(oldBookmark, bookmark, songName, 0.2f, new Vector2(90, 350), false);
                    GenerateText(oldBookmark, bookmark, songName2, 0.2f, new Vector2(90, 390), false);
                }

                if(id == 0)
                    titleStartTime = bookmark;

                oldBookmark = bookmark;
                id++;
            }
            var sVin = GetLayer("1").CreateSprite("sb/v.png", OsbOrigin.Centre);
            GenerateText(1393854, 1407854, "Map by Twiggykun", 0.3f, new Vector2(140, 170), true);
            GenerateText(1395854, 1407854, "Storyboard by PantyDev", 0.3f, new Vector2(90, 220), true);
            var bitmap = GetMapsetBitmap("sb/v.png");
            sVin.Scale(0, 1455464, 480.0 / bitmap.Height, 480.0 / bitmap.Height);
        }

        public void GenerateBackground(int startTime, int endTime, int backgroundID)
        {
            int duration = endTime - startTime;
            var background = GetLayer("0").CreateSprite("sb/bg/" + backgroundID + ".jpg", OsbOrigin.BottomRight, new Vector2(80, 473));
            var bitmap = GetMapsetBitmap("sb/bg/" + backgroundID + ".jpg");
            background.ScaleVec(OsbEasing.OutExpo, startTime, startTime + 1000, 180.0 / (bitmap.Height), 0, 180.0 / (bitmap.Height), 180.0 / bitmap.Height);
            background.ScaleVec(OsbEasing.InExpo, endTime - 1000, endTime, 180.0 / (bitmap.Height), 180.0 / bitmap.Height, 180.0 / (bitmap.Height), 0);
            //background.ScaleV(OsbEasing.InExpo, startTime, endTime, 180.0 / bitmap.Height, 180.0 / (bitmap.Height));
            //background.Fade(OsbEasing.OutExpo, startTime, startTime + 2000, 0, 1);
            //background.Fade(OsbEasing.OutExpo, endTime - 7000, endTime, 1, 0);
        }

        private void SetFont()
        {
            var font = LoadFont("sb/f", new FontDescription{
                FontPath = "Arial",
                FontSize = 100,
                FontStyle = FontStyle.Bold,
                Color = Color4.White
            });
            this.Font = font;
        }

        public void GenerateText(int startTime, int endTime, string text, float scale, Vector2 pos, bool shake)
        {
                float letterX = pos.X;
                float letterY = pos.Y;
                int letterID = 0;
                int lastLetterID = 0;
                foreach(var letter in text)
                {
                    if (shake)
                        lastLetterID ++;
                }

                foreach(var letter in text)
                {
                    letterID ++;
                    var texture = Font.GetTexture(letter.ToString());

                    if(!texture.IsEmpty)
                    {
                        var position = new Vector2(letterX, letterY)
                            + texture.OffsetFor(OsbOrigin.Centre) * scale;
                        
                        var sprite = GetLayer("2").CreateSprite(texture.Path, OsbOrigin.Centre, position);
                        
                        sprite.Fade(OsbEasing.OutExpo, startTime + (letterID * 50), startTime + 1500 + (letterID * 50), 0, 1);
                        sprite.Fade(OsbEasing.OutExpo, startTime + 1500 + (letterID * 50), endTime - 1500 - (letterID * 150), 1, 1);
                        sprite.Fade(endTime - 1500 - (letterID * 50), endTime - (letterID * 50), 1, 0);
                        sprite.Move(OsbEasing.OutExpo, startTime + (letterID * 50), startTime + 1500 + (letterID * 50), position.X + 50, position.Y, position.X, position.Y);
                        
                        var tick = (endTime - 1395854)/10;
                        
                        if(shake)
                        {
                            for(int i = 0; i < 10; i++)
                            {
                                if(i == 0)
                                    sprite.Rotate(OsbEasing.InOutExpo, 1395854+(tick*i), 1395854+(tick* (i+1)), 0, Math.PI/20);
                                else
                                {
                                    if(i % 2 == 0)
                                        sprite.Rotate(OsbEasing.InOutExpo, 1395854+(tick*i), 1395854+(tick* (i+1)), -Math.PI/20, Math.PI/20);
                                    else sprite.Rotate(OsbEasing.InOutExpo, 1395854+(tick*i), 1395854+(tick* (i+1)), Math.PI/20, -Math.PI/20);
                                }
                            }
                        }

                        sprite.ScaleVec(OsbEasing.OutExpo, startTime + (letterID * 50), startTime + 500 + (letterID * 50), -scale * 0.5, scale, scale, scale);
                    }
                    letterX += texture.BaseWidth * scale;
                }
        }
    }
}
