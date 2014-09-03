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

        public event EventHandler<byte[]> DataAvailable;

        public override bool stop() {
            try
            {
                recorder.Stop();
                return true;
            }
            catch
            {
            }
            return false;
        }

        public override bool initializeAudio() {
            try {
                recorder.Initialize();
                recorder.Start();
                return true;
            } catch {
            }
            return false;
        }

        public IOSAudioProcessor()
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
                    SampleRate = StudentDemo.Globals.SAMPLERATE,
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

        AudioUnitStatus AudioInputCallBack (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioUnit audioUnit)
        {

            var buffer = new AudioBuffer()
                {
                    NumberChannels = 1,
                    DataByteSize = (int)numberFrames * 2,
                    Data = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)numberFrames * 2)
                };

            var bufferList = new AudioBuffers(1);
            bufferList[0] = buffer;

            var status = audioUnit.Render(ref actionFlags, timeStamp, busNumber, numberFrames, bufferList);

            var send = new byte[buffer.DataByteSize];
            System.Runtime.InteropServices.Marshal.Copy(buffer.Data, send, 0, send.Length);

            var handler = DataAvailable;
            if (handler != null)
                handler(this, send);

            Console.Write("\n Buffer: ");
            foreach (byte b in send)
                Console.Write("\\x" + b);
            Console.Write("\n");

            System.Runtime.InteropServices.Marshal.FreeHGlobal(buffer.Data);



            return AudioUnitStatus.OK;
        }
    }
}

