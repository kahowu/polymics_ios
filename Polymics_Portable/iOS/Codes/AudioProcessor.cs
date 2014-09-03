using System;

namespace Polymics_Portable
{
    public abstract class AudioProcessor
    {
        public event EventHandler<byte[]> DataAvailable;

        public abstract bool initializeAudio();

        public abstract bool stop();
    }
}

