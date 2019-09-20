using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DentalImaging.Help
{
    public class WebCamera
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public bool DeviceExist { get; set; }

        public WebCamera()
        {
            DeviceExist = false;
        }

        public List<DeviceInfo> GetCameras()
        {
            List<DeviceInfo> cameraList = new List<DeviceInfo>();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            int idx = 0;
            foreach (FilterInfo device in videoDevices)
            {
                cameraList.Add(new DeviceInfo(device.Name, device.MonikerString, idx, FilterCategory.VideoInputDevice));
                idx++;
            }
            return cameraList;
        }

        public VideoCaptureDevice StartVideo(DeviceInfo device)
        {
            try
            {
                if(videoSource == null)
                    videoSource = new VideoCaptureDevice(device.MonikerString);

                return videoSource;
            }
            catch
            {
                return null;
            }
        }

        public bool CloseVideo()
        {
            if ((videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    DeviceExist = false;
                }
            videoSource = null;
            return true;
        }
    }

    public class DeviceInfo
    {
        public string Name;
        public string MonikerString;
        public int Index;
        Guid Category;

        public DeviceInfo(string name, string monikerString, int index) :
            this(name, monikerString, index, Guid.Empty)
        {
        }

        public DeviceInfo(string name, string monikerString, int index, Guid category)
        {
            Name = name;
            MonikerString = monikerString;
            Index = index;
            Category = category;
        }

        public override string ToString()
        {
            return Name;
        }        
    }

    public class DeviceCapabilityInfo
    {
        public Size FrameSize;
        public int MaxFrameRate;

        public DeviceCapabilityInfo(Size frameSize, int maxFrameRate)
        {
            FrameSize = frameSize;
            MaxFrameRate = maxFrameRate;
        }

        public override string ToString()
        {
            return string.Format("{0}x{1}  {2}fps", FrameSize.Width, FrameSize.Height, MaxFrameRate);
        }
    }
}
