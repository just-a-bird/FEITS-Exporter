using FEITS.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FEITS.Model
{
    public static class AssetGeneration
    {
        private static bool isInitialized = false;

        //Font
        private static bool[] validCharacters;

        public static bool[] ValidCharacters
        {
            get { return validCharacters; }
        }

        private static FontCharacter[] characters;
        private static Image[] Images = {Resources.Awakening_0, Resources.Awakening_1};

        //Resources
        private static Dictionary<string, byte[]> faceData;
        private static List<string> resourceList = new List<string>();

        //Lexicon
        private static readonly string tempPath = Path.Combine(Path.GetTempPath(), "FEITS\\");
        private static Uri lexUri;

        //Kamui
        //BUG: EyeStyles not being used except for EyeStyles[0]
        private static string[] EyeStyles = {"a", "b", "c", "d", "e", "f", "g"};

        static AssetGeneration()
        {
        }

        public static void Initialize(BackgroundWorker worker, DoWorkEventArgs e, IList dictList)
        {
            if (isInitialized)
            {
                Console.WriteLine("Assets already initialized!");
            }
            else
            {
                Console.WriteLine("Initializing assets...");

                //Set up font, generate list of valid chars
                validCharacters = new bool[0x10000];
                characters = new FontCharacter[0x10000];
                for (var i = 0; i < Resources.chars.Length/0x10; i++)
                {
                    var fc = new FontCharacter(Resources.chars, i*0x10);
                    validCharacters[fc.Data.Value] = true;
                    fc.SetGlyph(Images[fc.Data.ImageIndex]);
                    characters[fc.Data.Value] = fc;
                }

                worker.ReportProgress(25);

                //Grab face data and assign to dictionary
                faceData = new Dictionary<string, byte[]>();
                var fids = Resources.FID.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < fids.Length; i++)
                {
                    var dat = new byte[0x48];
                    Array.Copy(Resources.faces, i*0x48, dat, 0, 0x48);
                    faceData[fids[i]] = dat;
                }

                worker.ReportProgress(50);

                dictList.Add(new Uri(@"pack://application:,,,/FEITS Exporter;component/Resources/txt/FE_Dictionary.lex"));
                worker.ReportProgress(75);

                var set = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
                foreach (DictionaryEntry o in set)
                {
                    resourceList.Add(o.Key as string);
                }

                worker.ReportProgress(100);
                Resources.ResourceManager.ReleaseAllResources();

                isInitialized = true;
            }
        }

        public static Image DrawString(Image baseImage, string message, int startX, int startY, Color? textColor = null)
        {
            var textColorOrDefault = textColor ?? Color.Black;
            var curX = startX;
            var curY = startY;
            var newImage = (Bitmap) baseImage.Clone();
            using (var g = Graphics.FromImage(newImage))
            {
                foreach (var c in message)
                {
                    if (c == '\n')
                    {
                        curY += 20;
                        curX = startX;
                    }
                    else
                    {
                        var cur = characters[GetValue(c)];
                        g.DrawImage(cur.GetGlyph(textColorOrDefault), new Point(curX, curY - cur.Data.CropHeight));
                        curX += cur.Data.CropWidth;
                    }
                }
            }
            return newImage;
        }

        public static Image GetCharacterStageImage(string name, string emotions, Color hairColor, bool Slot1,
            PlayerGender playerGender)
        {
            var isUser = name == "username";
            var hairName = "_st_髪";
            var dat_id = "FSID_ST_" + name;
            if (isUser)
            {
                dat_id = "FSID_ST_" + playerGender.ToIfString(true) + "_顔" + EyeStyles[0].ToUpper();
                name = EyeStyles[0] + playerGender.ToIfString();
                hairName = name.Substring(1) + hairName + 0;
            }
            else
                hairName = name + hairName + "0";
            var splitEmotions = emotions.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var resourceName = name + "_st_" + splitEmotions[0];
            Image characterImage;
            if (resourceList.Contains(resourceName))
            {
                characterImage = (Image) Resources.ResourceManager.GetObject(resourceName);
                Debug.Assert(characterImage != null, $"{nameof(characterImage)} != null");
            }
            else
                characterImage = new Bitmap(1, 1);
            using (var g = Graphics.FromImage(characterImage))
            {
#if ACCESSORY
                if (isUser)
                {
                    g.DrawImage(
                        Resources.ResourceManager.GetObject(playerGender.ToIfString() + "_st_アクセサリ1_" + 0)
                            as Image, new Point(0, 0));
                }
#endif
                for (var i = 1; i < splitEmotions.Length; i++)
                {
                    var emotion = splitEmotions[i];
                    var exresname = name + "_st_" + emotion;
                    if (!resourceList.Contains(exresname)) continue;

                    var image = Resources.ResourceManager.GetObject(exresname) as Image;
                    Debug.Assert(image != null, $"{nameof(image)} != null");
                    byte xOffset, yOffset;
                    switch (emotion)
                    {
                        case "汗":
                            xOffset = 0x40;
                            yOffset = 0x42;
                            break;
                        case "照":
                            xOffset = 0x38;
                            yOffset = 0x3A;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(emotions), emotion, "Unexpected emotion.");
                    }
                    g.DrawImage(image,
                        new Point(BitConverter.ToUInt16(faceData[dat_id], xOffset),
                            BitConverter.ToUInt16(faceData[dat_id], yOffset)));
                }
                if (resourceList.Contains(hairName))
                {
                    var hair = Resources.ResourceManager.GetObject(hairName) as Bitmap;
                    g.DrawImage(ColorHair(hair, hairColor), new Point(0, 0));
                }
#if ACCESSORY
                if (isUser)
                {
                    g.DrawImage(
                        Resources.ResourceManager.GetObject(playerGender.ToIfString() + "_st_アクセサリ2_" + 0)
                            as Image, new Point(133, 28));
                }
#endif
            }
            if (Slot1)
                characterImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return characterImage;
        }

        public static Image GetCharacterBuImage(string name, string emotion, Color hairColor, bool crop,
            PlayerGender playerGender)
        {
            var hairName = "_bu_髪";
            var dat_id = "FSID_BU_" + name;
            var isUser = name == "username";
            var playerGenderString = playerGender.ToIfString();
            if (isUser)
            {
                dat_id = $"FSID_BU_{playerGender.ToIfString(true)}_顔{EyeStyles[0].ToUpper()}";
                name = $"{EyeStyles[0]}{playerGenderString}";
                hairName = $"{playerGenderString}{hairName}0";
            }
            else
                hairName = name + hairName + "0";
            var splitEmotions = emotion.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var resname = name + "_bu_" + splitEmotions[0];
            Image characterImage;
            if (resourceList.Contains(resname))
            {
                characterImage = (Image) Resources.ResourceManager.GetObject(resname);
                Debug.Assert(characterImage != null, $"{nameof(characterImage)} != null");
            }
            else
                characterImage = new Bitmap(1, 1);

            using (var g = Graphics.FromImage(characterImage))
            {
#if ACCESSORY
                if (isUser)
                {
                    g.DrawImage(
                        Resources.ResourceManager.GetObject(playerGenderString +
                                                            "_bu_アクセサリ1_" + 0)
                            as Image, new Point(0, 0));
                }
#endif

                for (var i = 1; i < splitEmotions.Length; i++)
                {
                    var exresname = name + "_bu_" + splitEmotions[i];
                    if (splitEmotions[i] == "汗" && resourceList.Contains(exresname))
                    {
                        g.DrawImage(Resources.ResourceManager.GetObject(exresname) as Image,
                            new Point(BitConverter.ToUInt16(faceData[dat_id], 0x40),
                                BitConverter.ToUInt16(faceData[dat_id], 0x42)));
                    }
                    else if (splitEmotions[i] == "照" && resourceList.Contains(exresname))
                    {
                        g.DrawImage(Resources.ResourceManager.GetObject(exresname) as Image,
                            new Point(BitConverter.ToUInt16(faceData[dat_id], 0x38),
                                BitConverter.ToUInt16(faceData[dat_id], 0x3A)));
                    }
                }
                if (resourceList.Contains(hairName))
                {
                    var hair = Resources.ResourceManager.GetObject(hairName) as Bitmap;
                    g.DrawImage(ColorHair(hair, hairColor), new Point(0, 0));
                }
#if ACCESSORY
                if (isUser)
                {
                    var Acc = new[] {new Point(66, 5), new Point(65, 21)}[0 - 2];
                    g.DrawImage(
                        Resources.ResourceManager.GetObject((new[] {"マイユニ男1", "マイユニ女2"})[playerGender] +
                                                            "_bu_アクセサリ2_" + 0)
                            as Image, Acc);
                }
#endif
            }
            if (crop)
            {
                var Cropped = new Bitmap(BitConverter.ToUInt16(faceData[dat_id], 0x34),
                    BitConverter.ToUInt16(faceData[dat_id], 0x36));
                using (var g = Graphics.FromImage(Cropped))
                {
                    g.DrawImage(characterImage,
                        new Point(-BitConverter.ToUInt16(faceData[dat_id], 0x30),
                            -BitConverter.ToUInt16(faceData[dat_id], 0x32)));
                }
                characterImage = Cropped;
            }
            characterImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return characterImage;
        }

        public static Image ColorHair(Image hair, Color color)
        {
            var bmp = (Bitmap) hair;
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            var ptr = bmpData.Scan0;

            var bytes = Math.Abs(bmpData.Stride)*bmp.Height;
            var rgbaValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbaValues, 0, bytes);

            for (var i = 0; i < rgbaValues.Length; i += 4)
            {
                if (rgbaValues[i + 3] <= 0) continue;
                rgbaValues[i + 2] = BlendOverlay(color.R, rgbaValues[i + 2]);
                rgbaValues[i + 1] = BlendOverlay(color.G, rgbaValues[i + 1]);
                rgbaValues[i + 0] = BlendOverlay(color.B, rgbaValues[i + 0]);
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbaValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public static byte BlendOverlay(byte Src, byte Dst)
        {
            return ((Dst < 128)
                ? (byte) Math.Max(Math.Min((Src/255.0f*Dst/255.0f)*255.0f*2, 255), 0)
                : (byte) Math.Max(Math.Min(255 - ((255 - Src)/255.0f*(255 - Dst)/255.0f)*255.0f*2, 255), 0));
        }

        public static Image Fade(Image BaseImage)
        {
            var bmp = BaseImage as Bitmap;
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            var ptr = bmpData.Scan0;

            var bytes = Math.Abs(bmpData.Stride)*bmp.Height;
            var rgbaValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbaValues, 0, bytes);

            const double BLACK_A = 113.0/255.0;

            for (var i = 0; i < rgbaValues.Length; i += 4)
            {
                if (rgbaValues[i + 3] <= 0) continue;
                var DST_A = rgbaValues[i + 3]/255.0;
                // double FINAL_A = BLACK_A + (DST_A) * (1.0 - BLACK_A);
                // rgbaValues[i + 3] = (byte)Math.Round((FINAL_A) * 255.0);
                rgbaValues[i + 2] = (byte) Math.Round((((rgbaValues[i + 2]/255.0))*(DST_A)*(1.0 - BLACK_A))*255.0);
                rgbaValues[i + 1] = (byte) Math.Round((((rgbaValues[i + 1]/255.0))*(DST_A)*(1.0 - BLACK_A))*255.0);
                rgbaValues[i + 0] = (byte) Math.Round((((rgbaValues[i + 0]/255.0))*(DST_A)*(1.0 - BLACK_A))*255.0);
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbaValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public static ushort GetValue(char c)
        {
            return BitConverter.ToUInt16(Encoding.Unicode.GetBytes(string.Empty + c), 0);
        }

        public static int GetLength(string s)
        {
            return
                s.Select(GetValue)
                    .Select(val => Math.Max(characters[val].Data.Width, characters[val].Data.CropWidth))
                    .Sum();
        }
    }
}