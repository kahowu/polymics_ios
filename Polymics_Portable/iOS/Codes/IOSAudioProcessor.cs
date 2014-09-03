using System;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using MonoTouch.AudioToolbox;
using MonoTouch.AudioUnit;

namespace Polymics_Portable.iOS
{
    public class IOSAudioProcessor : AudioProcessor
    {
        AudioUnit recorder;
        const int inputBus = 1;
        const int outputBus = 0;
        AudioStreamBasicDescription playerFormat;

        AudioBuffer aBuffer;

        const int AUDIOBUFFERSIZE = 2000;
        const int BUFFERCOUNT = 3;
        int byteSize;

        public bool initializeAudio() {
        }

        public AudioProcessor()
        {


            var inputComponent = AudioComponent.FindNextComponent(
                null,
                new AudioComponentDescription
                {
                    ComponentFlags = 0,
                    ComponentFlagsMask = 0,
                    ComponentManufacturer = AudioComponentManufacturerType.Apple,
                    ComponentSubType = (int)AudioTypeOutput.Remote,
                    ComponentType = AudioComponentType.Output
                });

            recorder = inputComponent.CreateAudioUnit();
            recorder.SetEnableIO(true, AudioUnitScopeType.Input, inputBus);
            recorder.SetEnableIO(false, AudioUnitScopeType.Output, outputBus);

            var audioFormat = new AudioStreamBasicDescription
                {
                    SampleRate = Globals.SAMPLERATE,
                    Format = AudioFormatType.LinearPCM,
                    FormatFlags = AudioFormatFlags.IsSignedInteger | AudioFormatFlags.IsPacked,
                    FramesPerPacket = 1,
                    ChannelsPerFrame = 1,
                    BitsPerChannel = 16,
                    BytesPerPacket = 2,
                    BytesPerFrame = 2
                };

            recorder.SetAudioFormat(audioFormat, AudioUnitScopeType.Output, inputBus);
            recorder.SetAudioFormat(audioFormat, AudioUnitScopeType.Input, outputBus);

            recorder.SetInputCallback(AudioInputCallBack, AudioUnitScopeType.Global, inputBus);

            // TODO: Disable buffers (requires interop)
            aBuffer = new AudioBuffer
                {
                    NumberChannels = 1,
                    DataByteSize = 512 * 2,
                    Data = System.Runtime.InteropServices.Marshal.AllocHGlobal(512 * 2)
                };

        }
    }
}

