using Terraria.ModLoader;

namespace BPConstructs.Contents
{
    internal class BPCPlayer : ModPlayer
    {
        private bool _architectMode = false;
        
        public bool architectMode
        {
            get => _architectMode;
            set => _architectMode = value;
        }

    }
}
