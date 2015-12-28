using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Collections;

namespace BetterPerspective
{
    public struct Mod
    {
        public Component Component;
        public  MethodInfo GUIMethod;
        public String ModName;
    }

    public class ModdingUI : MonoBehaviour
    {
        public Vector2 scrollPosition;
        public Rect windowRect = new Rect(5, 70, 250, 400);
        public List<Mod> Mods = new List<Mod> { };
        
        public int Selected = - 1;
        public string Path;
        public GUISkin skin;
        bool isEnabled;
        GUIStyle HeadStyle;
        void Start()
        {
            windowRect = new Rect(Screen.width - 350 , Screen.height - 580, 320, 460);
            char dsc = System.IO.Path.DirectorySeparatorChar;

            using (WWW www = new WWW("file://" + Path + dsc + "assetbundle" + dsc + "guiskin"))
            {
                if (www.error != null)
                    Debug.LogError("Loading had an error:" + www.error);

                AssetBundle bundle = www.assetBundle;
                skin = bundle.LoadAsset("Test") as GUISkin;
                bundle.Unload(false);
            }
            HeadStyle = new GUIStyle(skin.box);
            HeadStyle.fontStyle = FontStyle.Bold;
            HeadStyle.normal.textColor = Color.white;
            
            
        }
        void OnGUI()
        {
            GUI.skin = skin;
            if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 40, 50, 55), "Mods\n", HeadStyle))
            {
                isEnabled = !isEnabled;
            }
            
            
            if (isEnabled)
            {
                windowRect = GUI.Window(6, windowRect, DoMyWindow, "Mods Settings");
            }
            
            
        }


        void DoMyWindow(int windowID)
        {
            if (GUI.Button(new Rect(289, 5, 21, 20), "X"))
            {
                isEnabled = false;
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 30));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical();
            for (int i = 0; i < Mods.Count; i++)
            {
                GUILayout.BeginVertical("box");
                string ModName = Mods[i].ModName;
                if (ModName == null)
                {
                    ModName = Mods[i].Component.GetType().Name;
                }
                if (GUILayout.Button(ModName, HeadStyle, GUILayout.ExpandWidth(true)))
                {
                    if (Selected == i)
                    {
                        Selected = -1;
                        for (int j = 0; j < Mods.Count; j++)
                        {
                            Mods[j].Component.SendMessage("SettingsClose");

                        }
                    }
                    else
                    {
                        Selected = i;
                        for (int j = 0; j < Mods.Count; j++)
                        {
                            if (Mods[j].Component != Mods[i].Component)
                            {
                                Mods[j].Component.SendMessage("SettingsClose");
                            }
                            else
                            {
                                Mods[j].Component.SendMessage("SettingsOpen");
                            }
                        }
                    }

                }

                if (i == Selected)
                {
                    
                    Mods[i].GUIMethod.Invoke(Mods[i].Component, new object[] { });
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            GUILayout.EndScrollView();


        }
        public void AddMonoBehavoirWithGUI(Component ScriptToCall)
        {
            Mod CurrentMod = new Mod();
            CurrentMod.Component = ScriptToCall;
            CurrentMod.ModName = ScriptToCall.GetType().GetField("ModName").GetValue(ScriptToCall) as string;
            CurrentMod.GUIMethod = ScriptToCall.GetType().GetMethod("DrawGUI");
            Mods.Add(CurrentMod);
        }
       
    }
}
