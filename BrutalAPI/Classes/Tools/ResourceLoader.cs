using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class ResourceLoader
    {
        public static Texture2D LoadTexture(string name, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            var textureBytes = ResourceBinary(name, assembly);

            if (textureBytes == null)
            {
                Debug.LogError("Missing Texture! Check for typos when using ResourceLoader.LoadSprite() and that all of your textures have their build action as Embedded Resource.");

                return null;
            }

            var tex = new Texture2D(0, 0, TextureFormat.ARGB32, false)
            {
                anisoLevel = 1,
                filterMode = 0
            };

            tex.LoadImage(textureBytes);

            return tex;
        }

        public static Sprite LoadSprite(string name, Vector2? pivot = null, int ppu = 32, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            var tex = LoadTexture(name, assembly);

            if (tex == null)
                return null;

            return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), pivot ?? new Vector2(0.5f, 0.5f), ppu);
        }

        public static byte[] ResourceBinary(string name, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            var resname = assembly.GetManifestResourceNames().FirstOrDefault(x => x == name || x.EndsWith($".{name}") || x.Contains($".{name}."));

            if (string.IsNullOrEmpty(resname))
                return null;

            using var stream = assembly.GetManifestResourceStream(resname);

            var ba = new byte[stream.Length];
            stream.Read(ba, 0, ba.Length);

            return ba;
        }

        public static AssetBundle LoadAssetBundle(string bundlePath)
        {
            //AssetBundle bundle = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary(bundlePath));
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null)
            {
                Debug.LogWarning($"Failed to load AssetBundle: {bundle}!");
                return null;
            }
            return bundle;
        }

        public static AnimationClip LoadAnimationFromAssetBundle(string clipBundlePath, AssetBundle fileBundle)
        {
            return fileBundle.LoadAsset<AnimationClip>(clipBundlePath);
        }

        public static YarnProgram LoadYarnProgramFromAssetBundle(string yarnBundlePath, AssetBundle fileBundle)
        {
            return fileBundle.LoadAsset<YarnProgram>(yarnBundlePath);
        }

        public static ParticleSystem LoadParticleSystemFromAssetBundle(string particleBundlePath, AssetBundle fileBundle)
        {
            return fileBundle.LoadAsset<GameObject>(particleBundlePath).GetComponent<ParticleSystem>();
        }

        #region Music and Sound stuff
        /*
        public static void CreateSoundBankFile(string resourceName, bool onlyIfNotExist = false)
        {
            CreateResourceFile(resourceName, Application.dataPath + "/StreamingAssets", resourceName + ".bank", onlyIfNotExist);
            //call this like, CreateSoundBankFile("FunnyGuyHitNoise");
        }

        public static void CreateResourceFile(string resourceName, string path, string outputName, bool onlyIfNotExist = false)
        {
            byte[] resource = new byte[0] { };
            try
            {
                resource = ResourceLoader.ResourceBinary(resourceName);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            if (resource.Length > 0 && !(onlyIfNotExist && File.Exists(path + "/" + outputName)))
            {
                File.WriteAllBytes(path + "/" + outputName, resource);
            }
        }
        */
        #endregion
    }
}
