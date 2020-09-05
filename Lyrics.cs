using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;
using System.Drawing;
using System.IO;

namespace StorybrewScripts
{
    public class Lyrics : StoryboardObjectGenerator
    {
        [Configurable]
        public string SubtitlesPath = "lyrics.srt";

        [Configurable]
        public string FontName = "Verdana";

        [Configurable]
        public string SpritesPath = "sb/f";

        [Configurable]
        public int FontSize = 26;

        [Configurable]
        public float FontScale = 0.5f;

        [Configurable]
        public Color4 FontColor = Color4.White;

        [Configurable]
        public FontStyle FontStyle = FontStyle.Regular;

        [Configurable]
        public int GlowRadius = 0;

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 100);

        [Configurable]
        public bool AdditiveGlow = true;

        [Configurable]
        public int OutlineThickness = 3;

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public int ShadowThickness = 0;

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 100);

        [Configurable]
        public Vector2 Padding = Vector2.Zero;

        [Configurable]
        public float SubtitleY = 400;

        [Configurable]
        public bool PerCharacter = true;

        [Configurable]
        public bool TrimTransparency = true;

        [Configurable]
        public bool EffectsOnly = false;

        [Configurable]
        public bool Debug = false;

        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;

        [Configurable]
        public Color4 Color = Color4.White;

        [Configurable]
        public float ColorVariance = 0.6f;

        [Configurable]

        public bool squares = true;

        public override void Generate()
        {
            var font = LoadFont(SpritesPath, new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
                EffectsOnly = EffectsOnly,
                Debug = Debug,
            },
            new FontGlow()
            {
                Radius = AdditiveGlow ? 0 : GlowRadius,
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            var subtitles = LoadSubtitles(SubtitlesPath);

            if (GlowRadius > 0 && AdditiveGlow)
            {
                var glowFont = LoadFont(Path.Combine(SpritesPath, "glow"), new FontDescription()
                {
                    FontPath = FontName,
                    FontSize = FontSize,
                    Color = FontColor,
                    Padding = Padding,
                    FontStyle = FontStyle,
                    TrimTransparency = TrimTransparency,
                    EffectsOnly = true,
                    Debug = Debug,
                },
                new FontGlow()
                {
                    Radius = GlowRadius,
                    Color = GlowColor,
                });
                generateLyrics(glowFont, subtitles, "glow", true);
            }
            generateLyrics(font, subtitles, "", false);
        }

        public void generateLyrics(FontGenerator font, SubtitleSet subtitles, string layerName, bool additive)
        {
            var layer = GetLayer(layerName);
            if (PerCharacter) generatePerCharacter(font, subtitles, layer, additive);
            else generatePerLine(font, subtitles, layer, additive);
        }

        public void generatePerLine(FontGenerator font, SubtitleSet subtitles, StoryboardLayer layer, bool additive)
        {
            foreach (var line in subtitles.Lines)
            {
                var texture = font.GetTexture(line.Text);
                var position = new Vector2(320 - texture.BaseWidth * FontScale * 0.5f, SubtitleY)
                    + texture.OffsetFor(Origin) * FontScale;

                var sprite = layer.CreateSprite(texture.Path, Origin, position);
                sprite.Scale(line.StartTime, FontScale);
                sprite.Fade(line.StartTime - 200, line.StartTime, 0, 1);
                sprite.Fade(line.EndTime - 200, line.EndTime, 1, 0);
                if (additive) sprite.Additive(line.StartTime - 200, line.EndTime);
            }
        }

        public void generatePerCharacter(FontGenerator font, SubtitleSet subtitles, StoryboardLayer layer, bool additive)
        {
            var squaresLayer = GetLayer("lyricsSquares");
            foreach (var subtitleLine in subtitles.Lines)
            {
                var letterY = SubtitleY;
                var i = 0;
                var lineOffset = 0;
                foreach (var line in subtitleLine.Text.Split('\n'))
                {
                    var lineWidth = 0f;
                    var lineHeight = 0f;
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        lineWidth += texture.BaseWidth * FontScale;
                        lineHeight = Math.Max(lineHeight, texture.BaseHeight * FontScale);
                    }

                    var letterX = 320 - lineWidth * 0.5f + lineOffset;
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        if (!texture.IsEmpty)
                        {
                            
                            var initialPosition = new Vector2(letterX + 40, letterY) + texture.OffsetFor(Origin)  * FontScale;

                            var position = new Vector2(letterX, letterY)
                                + texture.OffsetFor(Origin) * FontScale;

                            var finalPosition = new Vector2(letterX + Random(-15, 15), letterY + Random(-15, 15)) + texture.OffsetFor(Origin) * FontScale;

                            if (squares) {
                                for (int f = 0; f < 4; f++) {

                                    var color = Color;
                                    if (ColorVariance > 0)
                                    {
                                        ColorVariance = MathHelper.Clamp(ColorVariance, 0, 1);

                                        var hsba = Color4.ToHsl(color);
                                        var sMin = Math.Max(0, hsba.Y - ColorVariance * 0.5f);
                                        var sMax = Math.Min(sMin + ColorVariance, 1);
                                        var vMin = Math.Max(0, hsba.Z - ColorVariance * 0.5f);
                                        var vMax = Math.Min(vMin + ColorVariance, 1);

                                        color = Color4.FromHsl(new Vector4(
                                            hsba.X,
                                            (float)Random(sMin, sMax),
                                            (float)Random(vMin, vMax),
                                            hsba.W));
                                    }


                                    var initiialRotation = Random(0, MathHelper.DegreesToRadians(360));
                                    var endRotation = Random(0, MathHelper.DegreesToRadians(360));

                                    var sqaureIinitPos = new Vector2(letterX + Random(-50, 50), letterY + Random(-50, 50)) + texture.OffsetFor(Origin) * FontScale;

                                    var squarePosition = new Vector2(letterX + Random(-25, 25), letterY + Random(-25, 25)) + texture.OffsetFor(Origin) * FontScale;

                                    var square = squaresLayer.CreateSprite("sb/etc/p.png", Origin, squarePosition);
                                    square.Move(subtitleLine.StartTime - 600 + i, subtitleLine.StartTime + i, sqaureIinitPos, squarePosition);
                                    square.Scale(subtitleLine.StartTime - 600 + i, subtitleLine.EndTime, 20, 20);
                                    square.Additive(subtitleLine.StartTime - 600 + i,  subtitleLine.EndTime);
                                    square.Fade(subtitleLine.StartTime - 600 + i, subtitleLine.StartTime + i, 0, 0.1);
                                    square.Rotate(subtitleLine.StartTime - 600 + i, subtitleLine.EndTime, initiialRotation, endRotation);
                                    square.Color(subtitleLine.StartTime - 600 + i, color);
                                    square.Fade(subtitleLine.EndTime - 600, subtitleLine.EndTime, 0.1, 0);
                                }
                            }

                            var sprite = layer.CreateSprite(texture.Path, Origin, position);
                            sprite.Move(OsbEasing.Out, subtitleLine.StartTime - 600 + i, subtitleLine.StartTime + i, initialPosition, position);
                            // sprite.Scale(subtitleLine.StartTime - 200 + i, subtitleLine.StartTime + i, 0, FontScale);
                            // sprite.Rotate(OsbEasing.Out, subtitleLine.StartTime - 400 + i, subtitleLine.StartTime + i, MathHelper.DegreesToRadians(Random(0, 360)), MathHelper.DegreesToRadians(0));
                            sprite.Fade(OsbEasing.Out, subtitleLine.StartTime - 600 + i, subtitleLine.StartTime + i, 0, 1);
                            // sprite.MoveY(subtitleLine.StartTime - 200 + i, subtitleLine.StartTime + i, letterY - 15, letterY);
                            sprite.Fade(OsbEasing.In, subtitleLine.EndTime - 600, subtitleLine.EndTime, 1, 0);
                            sprite.Scale(OsbEasing.In, subtitleLine.EndTime - 600, subtitleLine.EndTime, FontScale, 0);
                            sprite.Move(OsbEasing.In, subtitleLine.EndTime - 600, subtitleLine.EndTime, position, finalPosition); 
                            
                            if (additive) sprite.Additive(subtitleLine.StartTime - 600, subtitleLine.EndTime);
                            i += 65; 
                        }
                        letterX += texture.BaseWidth * FontScale;
                    }
                    // lineOffset += 120;
                    letterY += lineHeight;
                }
            }
        }
    }
}
