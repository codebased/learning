using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Mixin
{
    public interface MixinInterfacePlayerService
    {
        string FilePath { get; set; }
    }

    public static class MixinExtensions
    {
        public static void PlayAudio(this MixinInterfacePlayerService service)
        {

        }

        public static void PlayVideo(this MixinInterfacePlayerService service)
        {

        }
    }

    public class DvdPlayer : MixinInterfacePlayerService
    {
        public string FilePath { get; set; }
    }
    public class ProjectorPlayer : MixinInterfacePlayerService
    {
        public string FilePath { get; set; }
    }
}
