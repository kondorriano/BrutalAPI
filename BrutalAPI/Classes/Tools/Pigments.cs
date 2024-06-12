using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class Pigments
    {
        #region In-Game Pigment Gets
        static public ManaColorSO Blue => LoadedDBsHandler.PigmentDB.GetPigment("Blue");
        static public ManaColorSO Purple => LoadedDBsHandler.PigmentDB.GetPigment("Purple");
        static public ManaColorSO Red => LoadedDBsHandler.PigmentDB.GetPigment("Red");
        static public ManaColorSO Yellow => LoadedDBsHandler.PigmentDB.GetPigment("Yellow");
        static public ManaColorSO Grey => LoadedDBsHandler.PigmentDB.GetPigment("Grey");
        static public ManaColorSO Green => LoadedDBsHandler.PigmentDB.GetPigment("Green");
        static public ManaColorSO BluePurple => LoadedDBsHandler.PigmentDB.GetPigment("BP");
        static public ManaColorSO BlueRed => LoadedDBsHandler.PigmentDB.GetPigment("BR");
        static public ManaColorSO BlueYellow => LoadedDBsHandler.PigmentDB.GetPigment("BY");
        static public ManaColorSO PurpleBlue => LoadedDBsHandler.PigmentDB.GetPigment("PB");
        static public ManaColorSO PurpleRed => LoadedDBsHandler.PigmentDB.GetPigment("PR");
        static public ManaColorSO PurpleYellow => LoadedDBsHandler.PigmentDB.GetPigment("PY");
        static public ManaColorSO RedBlue => LoadedDBsHandler.PigmentDB.GetPigment("RB");
        static public ManaColorSO RedPurple => LoadedDBsHandler.PigmentDB.GetPigment("RP");
        static public ManaColorSO RedYellow => LoadedDBsHandler.PigmentDB.GetPigment("RY");
        static public ManaColorSO YellowBlue => LoadedDBsHandler.PigmentDB.GetPigment("YB");
        static public ManaColorSO YellowRed => LoadedDBsHandler.PigmentDB.GetPigment("YR");
        static public ManaColorSO YellowPurple => LoadedDBsHandler.PigmentDB.GetPigment("YP");
        #endregion

        #region Split Pigment and Health Fields
        private static readonly Dictionary<ManaColorSO[], ManaColorSO> AlreadyMadeSplitPigment = new()
        {
            { [Blue, Purple], BluePurple },
            { [Blue, Red], BlueRed },
            { [Blue, Yellow], BlueYellow },

            { [Purple, Blue], PurpleBlue },
            { [Purple, Red], PurpleRed },
            { [Purple, Yellow], PurpleYellow },

            { [Red, Blue], RedBlue },
            { [Red, Purple], RedPurple },
            { [Red, Yellow], RedYellow },

            { [Yellow, Blue], YellowBlue },
            { [Yellow, Red], YellowRed },
            { [Yellow, Purple], YellowPurple }
        };

        private static readonly List<Texture2D> SplitPigmentSprites = new()
        {
            ResourceLoader.LoadTexture("Split2_Pigment"),
            ResourceLoader.LoadTexture("Split3_Pigment"),
            ResourceLoader.LoadTexture("Split4_Pigment"),
            ResourceLoader.LoadTexture("Split5_Pigment"),
            ResourceLoader.LoadTexture("Split6_Pigment"),
            ResourceLoader.LoadTexture("Split7_Pigment"),
            ResourceLoader.LoadTexture("Split8_Pigment"),
            ResourceLoader.LoadTexture("Split9_Pigment")
        };

        private static readonly List<Texture2D> SplitPigmentHealthSprites = new()
        {
            ResourceLoader.LoadTexture("Split2_Health"),
            ResourceLoader.LoadTexture("Split3_Health"),
            ResourceLoader.LoadTexture("Split4_Health"),
            ResourceLoader.LoadTexture("Split5_Health"),
            ResourceLoader.LoadTexture("Split6_Health"),
            ResourceLoader.LoadTexture("Split7_Health"),
            ResourceLoader.LoadTexture("Split8_Health"),
            ResourceLoader.LoadTexture("Split9_Health")
        };

        private static readonly List<Color> StitchColors = new()
        {
            new(1, 0, 0, 1),
            new(0, 1, 0, 1),
            new(0, 0, 1, 1),
            new(1, 1, 0, 1),
            new(0, 1, 1, 1),
            new(1, 0, 1, 1),
            new(1, 1, 1, 1),
            new(0, 0, 0, 1),
            new(0, 0, 0, 0)
        };

        #endregion

        #region Basic Pigment Functions

        static public ManaColorSO GetPigmentWithID(string id)
        {
            return LoadedDBsHandler.PigmentDB.GetPigment(id);
        }

        static public void AddNewPigment(string id, ManaColorSO pigment)
        {
            LoadedDBsHandler.PigmentDB.AddNewPigment(id, pigment);
        }
        #endregion

        #region Split Pigment and Health Functions

        public static ManaColorSO SplitPigment(params ManaColorSO[] components)
        {
            if (AlreadyMadeSplitPigment.TryGetValue(components, out var spligment))
                return spligment;

            return AlreadyMadeSplitPigment[components] = MakeNewSplitPigment(components);
        }

        private static ManaColorSO MakeNewSplitPigment(ManaColorSO[] components)
        {
            if (components.Length <= 0)
                return null;

            if (components.Length == 1)
                return components[1];

            var pigmentTemplate = SplitPigmentSprites[Mathf.Clamp(components.Length, 0, SplitPigmentSprites.Count - 1)];
            var healthTemplate = SplitPigmentHealthSprites[Mathf.Clamp(components.Length, 0, SplitPigmentHealthSprites.Count - 1)];

            var canProduce = false;
            var costDamage = true;
            
            var types = new List<string>();

            string sound = null;

            var sprites_P = new List<Sprite>();
            var sprites_U = new List<Sprite>();
            var sprites_C = new List<Sprite>();
            var sprites_S = new List<Sprite>();
            var sprites_H = new List<Sprite>();

            foreach(var c in components)
            {
                if (c == null)
                    continue;

                var pigments = c.pigmentTypes.Count <= 1 ? [c] : c.pigmentTypes.Select(LoadedDBsHandler.PigmentDB.GetPigment);

                types.AddRange(c.pigmentTypes);

                foreach (var p in pigments)
                {
                    canProduce |= p.canGenerateMana;
                    costDamage &= p.dealsCostDamage;

                    sound ??= p.manaSoundEvent;

                    if(p.manaSprite != null)
                        sprites_P.Add(p.manaSprite);

                    if(p.manaUsedSprite != null)
                        sprites_U.Add(p.manaUsedSprite);

                    if(p.manaCostSprite != null)
                        sprites_C.Add(p.manaCostSprite);

                    if (p.manaCostSelectedSprite != null)
                        sprites_S.Add(p.manaCostSelectedSprite);

                    if(p.healthSprite != null)
                        sprites_H.Add(p.healthSprite);
                }
            }

            var pigment = ScriptableObject.CreateInstance<ManaColorSO>();

            var id = string.Join("_", types.Select(x => string.IsNullOrEmpty(x) ? "" : x.Substring(0, 1)));

            pigment.name = $"Pigment_Cost_{id}";
            pigment.pigmentID = id.Replace("_", "");

            pigment.pigmentTypes = types;

            pigment.canGenerateMana = canProduce;
            pigment.dealsCostDamage = costDamage;

            pigment.manaSoundEvent = sound ?? "";

            var pigmentsTex = new Texture2D(160, 51, TextureFormat.ARGB32, false)
            {
                anisoLevel = 1,
                filterMode = 0,
                name = $"SplitPigmentSprites_{id}"
            };

            pigment.manaSprite =                StitchPigmentSprites(SplitPigmentSprites, sprites_P, pigmentsTex, 0, 0, 1);
            pigment.manaUsedSprite =            StitchPigmentSprites(SplitPigmentSprites, sprites_U, pigmentsTex, 40, 0, 2);
            pigment.manaCostSprite =            StitchPigmentSprites(SplitPigmentSprites, sprites_C, pigmentsTex, 80, 0, 3);
            pigment.manaCostSelectedSprite =    StitchPigmentSprites(SplitPigmentSprites, sprites_S, pigmentsTex, 120, 0, 4);

            pigment.healthSprite =              StitchPigmentSprites(SplitPigmentHealthSprites, sprites_H, pigmentsTex, 0, 40, 5);

            pigmentsTex.Apply();

            return pigment;
        }

        private static Sprite StitchPigmentSprites(IList<Texture2D> templates, IList<Sprite> sprites, Texture2D pigmentsTexture, int xOffs, int yOffs, int index)
        {
            var template = templates[Mathf.Clamp(sprites.Count - 2, 0, templates.Count - 1)];

            if (template == null)
                return null;

            for(int x = 0; x < template.width; x++)
            {
                for(int y = 0; y < template.height; y++)
                {
                    var color = template.GetPixel(x, y);

                    var idx = StitchColors.IndexOf(color);

                    if(idx >= 0 && idx < sprites.Count)
                    {
                        var spriteHere = sprites[idx];

                        if(spriteHere != null && spriteHere.texture.isReadable)
                        {
                            var middleCoord = spriteHere.rect.center;

                            var middleOffsetX = x - (template.width / 2);
                            var middleOffsetY = y - (template.height / 2);

                            var xCoord = (int)middleCoord.x + middleOffsetX;
                            var yCoord = (int)middleCoord.y + middleOffsetY;

                            color = spriteHere.texture.GetPixel(xCoord, yCoord);
                        }
                    }

                    pigmentsTexture.SetPixel(x + xOffs, y + yOffs, color);
                }
            }

            var sprite = Sprite.Create(pigmentsTexture, new(xOffs, yOffs, template.width, template.height), new(0.5f, 0.5f), 32);
            sprite.name = $"{pigmentsTexture.name}_{index}";

            return sprite;
        }
        #endregion
    }
}
