using System;

namespace Polymics_Portable
{
    public abstract class AudioProcessor
    {
        public event EventHandler<byte[]> DataAvailable;

        public bool initializeAudio();

        public bool stop();
    }
}

