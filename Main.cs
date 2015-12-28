using UnityEngine;
using System.Collections;

namespace BetterPerspective
{
    public class Main : IMod
    {
        GameObject go;
        public void onEnabled()
        {

            go = new GameObject("ModMenu");
            go.AddComponent<ModdingUI>();
            go.GetComponent<ModdingUI>().Path = Path;
        }
          

        public void onDisabled()
        {
            Object.DestroyImmediate(go);
        }

        public string Name { get { return "ModSettings"; } }
        public string Description { get { return "The UI component to hand the mod settings"; } }

        public string Path { get; set; }
    }
    
}
