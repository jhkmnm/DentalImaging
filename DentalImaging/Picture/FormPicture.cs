using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using DentalImaging.Help;
using DentalImaging.Model;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using DentalImaging.Picture;
using System.Net.NetworkInformation;
using AForge.Video;
using System.Drawing.Imaging;
using DentalImaging.NewForm;

namespace DentalImaging
{
    public partial class FormPicture : Form
    {
        public List<ImgInfo> imgList { get; set; }
        private ImgInfo currentImg;
        private Image oldImg;
        private int currentImgIndex;
        int showIndex = 0;
        int VideoCount = 1;
        List<PictureBox> picList = new List<PictureBox>();
        private VideoFileWriter VideoOutPut;
        MJPEGStream _wifiCamera = null;
        string dirName = $"{User.DBPath}\\{User.CurrentPatient.FName + User.CurrentPatient.BirthDay}";
        bool isMirror;
        IMessageBase messageHelp;
        Thread thd;
        UsbRegDeviceList allDevices = UsbDevice.AllDevices;
        private List<List<ImgInfo>> delList = new List<List<ImgInfo>>();
        bool IsWifi = false;
        ILog logger;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice _usbCamera;
        public int selectedDeviceIndex = 0;
        bool isLoad = false;
        private int cameraWidth, cameraHeight;
        private Bitmap curFrame = new Bitmap(1, 1, PixelFormat.Format24bppRgb);
        private static object obj = new object();
        private VideoFileWriter writer = new VideoFileWriter();     //写入到视频
        private Thread recordThread = null;//录像线程
        private bool isNeedRecord = false;//是否需要录制flag
        bool isOpenUsb = true;
        private AppHotKey hotKey = new AppHotKey();
        private List<Bitmap> pic对比List = new List<Bitmap>();
        private int currentTab = 0;
        private DateTime _lastTime;
        bool _needAddImg = false;

        /// <summary>
        /// 设置Alt+S的显示/隐藏窗体全局热键
        /// </summary>
        private void SetHotKey()
        {
            AppHotKey.RegisterHotKey(this.Handle, Space, 0, Keys.Delete);            
        }        

        void hotKey2_HotKey()
        {
            DeleteCurrentImg();
        }

        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int Space = 10003; //热键ID
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    switch (m.WParam.ToInt32())
                    {
                        case Space: //热键ID
                            DeleteCurrentImg();
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        #region 摄像头

        #region Wifi
        private bool ConnectionWifiVideo()
        {
            if (User.LinkType == LinkType.USB) return false;

            Ping pingSender = new Ping();
            try
            {
                PingReply reply = pingSender.Send("10.10.10.254", 10);//第一个参数为ip地址，第二个参数为ping的时间
                if (reply.Status == IPStatus.Success)
                {
                    _wifiCamera = new MJPEGStream();
                    _wifiCamera.NewFrame += new NewFrameEventHandler(mjpegSource_NewFrame);

                    string url = "http://10.10.10.254:8080";
                    _wifiCamera.Source = url;
                    _wifiCamera.Start();

                    IsWifi = true;
                    videoSourcePlayer1.Visible = false;
                    pictureBox1.Visible = true;
                    btnVideotape.Enabled = true;
                    btnMirror.Enabled = false;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void mjpegSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap FrameData = new Bitmap(eventArgs.Frame);
            this.pictureBox1.Image = FrameData;
            curFrame = (Bitmap)FrameData.Clone();
            if (isNeedRecord)
            {
                this.BeginInvoke(new Action(() => { lblTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }));
            }
        }
        #endregion

        #region USB
        private bool ConnectionUSBVideo(int width)
        {
            if (_usbCamera != null && _usbCamera.IsRunning)
                _usbCamera.Stop();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                CommHelp.ShowTips("未发现视频设备");
                this.Close();
                return false;
            }
            selectedDeviceIndex = 0;
            string monikerString = "";
            foreach(FilterInfo filter in videoDevices)
            {
                logger.Info("找到摄像头:" + filter.Name);
                if (filter.Name == "SKT-OL400C-13A")
                {
                    monikerString = filter.MonikerString;
                    break;
                }
            }
            if (string.IsNullOrEmpty(monikerString))
            {
                CommHelp.ShowTips("设备未连接");
                this.Close();
                return false;
            }
            _usbCamera = new VideoCaptureDevice(monikerString);//连接摄像头。
            foreach (var v in _usbCamera.VideoCapabilities)
            {
                if (v.FrameSize.Width == width)
                {
                    _usbCamera.VideoResolution = v;
                    break;
                }
            }
            IsWifi = false;
            videoSourcePlayer1.VideoSource = _usbCamera;
            videoSourcePlayer1.Visible = true;
            _usbCamera.Start();
            lblTime.Visible = true;
            pictureBox1.Visible = false;

            return true;
        }

        private void Camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isNeedRecord)
            {
                var drawDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.BeginInvoke(new Action(() => { lblTime.Text = drawDate; }));
                VideoOutPut.WriteVideoFrame(eventArgs.Frame);
                if (_needAddImg)
                {
                    _needAddImg = false;
                    Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
                    var id = imgList.Count > 0 ? imgList.Max(m => m.Index) + 1 : 1;
                    imgList.Add(new ImgInfo { Index = id, Date = DateTime.Now, InfoType = 1, VideoPath = videoPath, Img = MergeImg(bitmap) });
                }
            }
            else
            {
                if (VideoOutPut != null && VideoOutPut.IsOpen)
                    VideoOutPut.Close();
            }
            cameraWidth = eventArgs.Frame.Width;
            cameraHeight = eventArgs.Frame.Height;
        }        
        #endregion

        #endregion

        private void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
            logger = LogManager.GetLogger(typeof(FormPicture));
        }        

        public FormPicture()
        {
            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
            InitLog4Net();
            try
            {
                logger.Debug("窗体加载");
                isLoad = true;
                if (ConnectionWifiVideo())
                {
                    try
                    {
                        messageHelp = new TcpHelp("10.10.10.254", 3333);                        
                        messageHelp.SendMessage("{\"Camenable\":\"1\"}");
                        Thread.Sleep(50);
                    }
                    catch (Exception ex)
                    {
                        CommHelp.ShowTips("与设备通信失败，请确认设备没有被其它用户占用！");
                        this.DialogResult = DialogResult.No;
                        this.Close();
                        return;
                    }
                }
                else
                {
                    if (ConnectionUSBVideo(1920))
                    {
                        if (allDevices.Count > 0)
                        {
                            try
                            {
                                messageHelp = new UsbHelp(allDevices[0].Vid, allDevices[0].Pid);
                            }
                            catch (Exception ex)
                            {
                                CommHelp.ShowTips("与设备通信失败，请确认设备没有被其它用户占用！");
                                this.DialogResult = DialogResult.No;
                                this.Close();
                                return;
                            }
                        }
                        else
                        {
                            CommHelp.ShowTips("未发现视频设备");
                            this.DialogResult = DialogResult.No;
                            this.Close();
                            return;
                        }
                    }
                    else
                        return;
                }
                messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.HeartBeat]);
                Thread.Sleep(100);
                thd = new Thread(new ThreadStart(OpenUsb));
                thd.SetApartmentState(ApartmentState.STA);
                thd.IsBackground = true;
                thd.Start();
                currentPage = 0;
                SetHotKey();
                _lastTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                CommHelp.ShowError(ex.Message + ex.StackTrace);
                return;
            }
            try
            {                
                ShowVideo();
            }
            catch(Exception ex)
            {
                CommHelp.ShowError(ex.Message + ex.StackTrace);
                return;
            }
            imgList = new List<ImgInfo>();
        }

        private void FormPicture_Load(object sender, EventArgs e)
        {
            imgSource = new BindImgListSource();
            for (int i = 1; i <= 10; i++)
            {
                imgSource.GetType().GetProperty("Image" + i).SetValue(imgSource, global::DentalImaging.Properties.Resources._1543660125_1_, null);
            }
            dataGridView1.AutoGenerateColumns = false;
            bindingSource1.DataSource = imgSource;
            SetCameraProperty(tabControl1.SelectedIndex == 0 ? 0 : 1);
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            ShowVideo();
            OrderType type = OrderType.PSend_Rule_View;
            if (tabControl1.SelectedIndex == 1)
                type = OrderType.PSend_UV_View;
            else if (tabControl1.SelectedIndex == 2)
                type = OrderType.PSend_ICON_View;
            try
            {
                messageHelp.SendMessage(UsbMessage.OrdersB[type]);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
                CommHelp.ShowError(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ShowEdit();
            OrderType type = OrderType.PSend_Rule_View;
            if (tabControl1.SelectedIndex == 1)
                type = OrderType.PSend_UV_View;
            else if (tabControl1.SelectedIndex == 2)
                type = OrderType.PSend_ICON_View;
            messageHelp.SendMessage(UsbMessage.OrdersB[type]);            
        }

        private void ShowVideo()
        {
            if(!panelVideo.Visible)
            {
                panel2.Visible = true;
                panelVideo.Visible = true;
                panelImgView.Visible = false;
                panelTool.Location = new Point(panelTool.Location.X, panelTool.Location.Y + panelImgList.Height);
                panel2.Height += panelImgList.Height;
                panPai.Visible = true;
                panEdit.Visible = false;
                btnPai.Visible = true;
                btnVideotape.Visible = true;
                pan对比.Visible = false;
                button11.Visible = false;
                panel3.Visible = false;
            }
            if (IsWifi)
            {
                if(!_wifiCamera.IsRunning)
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_Open_Camenable]);
                    _wifiCamera.Start();
                }
            }
            else
            {
                if(!_usbCamera.IsRunning)
                {
                    _usbCamera.Start();
                }
            }            
        }

        private void ShowEdit()
        {
            if(pan对比.Visible)
            {
                panel2.Visible = true;
                pan对比.Visible = false;
                panelImgView.Visible = true;
            }
            else if(!panelImgView.Visible)
            {
                button11.Visible = true;                
                panelVideo.Visible = false;
                panelImgView.Visible = true;
                panelTool.Location = new Point(panelTool.Location.X, panelTool.Location.Y - panelImgList.Height);
                panel2.Height -= panelImgList.Height;
                if(imgList.Count > 0)
                {
                    ShowImgOrVideo(imgList[imgList.Count - 1]);
                }
                    
                pan对比.Visible = false;
                panel3.Visible = true;
            }
        }

        private void btnPai_Click(object sender, EventArgs e)
        {
            OrderType type = OrderType.PSend_Rule_See;
            //指令修改
            //if (tabControl1.SelectedIndex == 1)
            //    type = OrderType.PSend_UV_See;
            //else if (tabControl1.SelectedIndex == 2)
            //    type = OrderType.PSend_ICON_See;
            messageHelp.SendMessage(UsbMessage.OrdersB[type]);
        }

        private void Pai()
        {
            try
            {
                if (_usbCamera != null || IsWifi)
                {
                    Bitmap bitmap = null;
                    if (!IsWifi)
                    {
                        bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
                    }
                    else
                        bitmap = pictureBox1.Image.Clone() as Bitmap;
                    
                    pictureBox4.Image = bitmap;
                    var id = imgList.Count > 0 ? imgList.Max(m => m.Index) + 1 : 1;
                    var info = new ImgInfo { Index = id, Date = DateTime.Now, Img = bitmap };
                    imgList.Add(info);
                    ShowEdit();
                }
            }
            catch(Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        #region 图片列表
        private void InitImgList()
        {
            for (int i = 0; i < 10; i++)
            {
                var picBox = new PictureBox();
                picBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                picBox.BorderStyle = BorderStyle.FixedSingle;
                picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                picBox.Name = $"picBox{i}";
                picBox.Location = new Point(3 + (93 * i), 1);
                picBox.Size = new Size(94, 104);
                picBox.TabIndex = i;
                picBox.TabStop = false;
                picBox.Click += PicBox_Click;
                picList.Add(picBox);
                panelImgList.Controls.Add(picBox);
            }
        }

        int oldIndex = 0;
        private void ReadLoadImgList()
        {
            var index = 0;
            if(picList.Count == 10)
            {
                index = 0;
            }
            else if (showIndex == 0 && currentImgIndex != 0)
            {
                index = currentImgIndex - 1;
            }
            else if (showIndex == 9)
            {
                if (picList.Count > currentImgIndex + 1)
                    index = currentImgIndex - 8;
                else
                    index = currentImgIndex - 9;
            }
            else
            {
                index = currentImgIndex - showIndex;
            }
            if (index < 0) index = 0;

            if(index != oldIndex)
            {
                oldIndex = index;
                panelImgList.Controls.Clear();
                for (var i = 0; i < 10; i++)
                {
                    var picBox1 = picList[index];                    
                    picBox1.TabIndex = i;
                    picBox1.Location = new Point(3 + (93 * i), 1);
                    panelImgList.Controls.Add(picBox1);
                    index++;
                }
            }            
        }

        private void PicBox_Click(object sender, EventArgs e)
        {
            var picBox = ((PictureBox)sender);
            showIndex = picBox.TabIndex;
            var info = (ImgInfo)picBox.Tag;
            if (!pan对比.Visible)
            {
                ShowImgOrVideo(info);
            }
            else
            {
                ((PictureBox)pan对比.Controls[picIndex].Controls[0]).Image = info.Img;
                pic对比List[picIndex] = info.Img;
                picIndex++;
                if (picIndex >= pan对比.Controls.Count)
                    picIndex--;
            }
            Graphics pictureborder = picBox.CreateGraphics();
            Pen pen = new Pen(Color.Yellow, 4);
            pictureborder.DrawRectangle(pen, picBox.ClientRectangle.X, picBox.ClientRectangle.Y, picBox.ClientRectangle.X + picBox.ClientRectangle.Width, picBox.ClientRectangle.Y + picBox.ClientRectangle.Height);
        }

        private void ShowImgOrVideo(int imgID)
        {
            var info = imgList.FirstOrDefault(f => f.Index == imgID);
            if (!pan对比.Visible)
            {
                ShowImgOrVideo(info);
            }
            else
            {
                ((PictureBox)pan对比.Controls[picIndex].Controls[0]).Image = info.Img;
                pic对比List[picIndex] = info.Img;
                picIndex++;
                if (picIndex >= pan对比.Controls.Count)
                    picIndex--;
            }
        }

        BindImgListSource imgSource = null;
        private void BindImgList(int index)
        {
            int begin =0, end =0;
            var maxPage = imgList.Count;

            if(maxPage > 0)
            {
                begin = index - 8;
                if (begin < 1)
                    begin = 1;

                end = begin + 9;
                if (end > maxPage)
                    end = maxPage;

                begin = end - 9;
                if (begin < 1)
                    begin = 1;

                var i = 1;
                for(;begin <= end;)
                {
                    imgSource.GetType().GetProperty("Image" + i).SetValue(imgSource, imgList[begin - 1].Img, null);
                    dataGridView1.Columns[i - 1].ToolTipText = imgList[begin - 1].Index.ToString();
                    i++;
                    begin++;
                }
                end = i - 2;
                for(;i<=10;i++)
                {
                    imgSource.GetType().GetProperty("Image" + i).SetValue(imgSource, global::DentalImaging.Properties.Resources._1543660125_1_, null);
                }

                dataGridView1.Refresh();
                var a = dataGridView1.SelectedCells;
                a[0].Selected = false;
                if (index == imgList.Count && end > 0)
                {
                    dataGridView1.Rows[0].Cells[end].Selected = true;
                }
                else
                {
                    dataGridView1.Rows[0].Cells[index-1].Selected = true;
                }
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(dataGridView1.Columns[e.ColumnIndex].ToolTipText))
            {
                var page = Convert.ToInt32(dataGridView1.Columns[e.ColumnIndex].ToolTipText);
                var img = imgList.FirstOrDefault(f => f.Index == page);
                ShowImgOrVideo(img);
            }
        }
        #endregion        

        private void ShowImgOrVideo(ImgInfo info)
        {
            if (info == null) return;
            if (!pan对比.Visible)
            {
                currentImg = info;
                currentImgIndex = imgList.IndexOf(info);
                oldImg = (Image)currentImg.Img.Clone();
                if (info.InfoType == 0)
                {
                    pictureBox4.Visible = true;
                    pictureBox4.Image = info.Img;
                    axWindowsMediaPlayer1.Visible = false;
                    panEdit.Visible = false;
                    btnPai.Visible = false;
                    btnVideotape.Visible = false;
                }
                else
                {
                    axWindowsMediaPlayer1.Visible = true;
                    pictureBox4.Visible = false;
                    axWindowsMediaPlayer1.URL = info.VideoPath;
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    panPai.Visible = false;
                    panEdit.Visible = true;
                }
                BindImgList(info.Index);
            }
            else
            {
                ((PictureBox)pan对比.Controls[picIndex].Controls[0]).Image = info.Img;
                pic对比List[picIndex] = info.Img;
                picIndex++;
                if (picIndex >= pan对比.Controls.Count)
                    picIndex--;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _usbCamera.DisplayPropertyPage(pictureBox1.Handle);
        }

        private void FormPicture_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_usbCamera != null && _usbCamera.IsRunning)
            {
                _usbCamera.SignalToStop();
                _usbCamera.WaitForStop();
                _usbCamera.Stop();
            }
            isOpenUsb = false;
            Thread.Sleep(15);
            if (messageHelp != null)
            {
                messageHelp.Close();
                Thread.Sleep(10);
            }            
            if (recordThread != null && recordThread.ThreadState == ThreadState.Running) recordThread.Abort();
            AppHotKey.UnregisterHotKey(this.Handle, Space);
            this.DialogResult = DialogResult.OK;
        }

        private void btnVideotape_Click(object sender, EventArgs e)
        {
            if(btnVideotape.Text == LanguageHelp.GetTextLanguage("录像"))
            {
                isNeedRecord = _needAddImg = true;
                lblTime.Visible = true;
                btnPai.Visible = false;
                btnVideo.Visible = false;
                btnEdit.Visible = false;
                btnVideotape.Text = LanguageHelp.GetTextLanguage("停止");
                if (IsWifi)
                {
                    recordThread = new Thread(StartWifiVideo);
                    recordThread.IsBackground = true;
                    lblTime.Parent = pictureBox1;
                    recordThread.Start();
                }
                else
                {
                    if(ConnectionUSBVideo(1920))
                    {
                        _usbCamera.NewFrame += Camera_NewFrame;
                        lblTime.Parent = videoSourcePlayer1;
                        StartUsbVideo();
                    }
                }
            }
            else
            {
                isNeedRecord = false;
                if (!IsWifi)
                {
                    Thread.Sleep(10);
                    ConnectionUSBVideo(1920);
                }
                lblTime.Visible = false;
                btnVideotape.Text = LanguageHelp.GetTextLanguage("录像");
                btnPai.Visible = true;
                btnVideo.Visible = true;
                btnEdit.Visible = true;
                Thread.Sleep(10);
                ShowEdit();
            }
        }

        private Bitmap MergeImg(Image imgSrc)
        {
            System.Drawing.Image imgWarter = global::DentalImaging.Properties.Resources.timg__1_;
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgWarter, new Rectangle(imgSrc.Width / 2 - 100,
                                                 imgSrc.Height / 2 - 100,
                                                 imgWarter.Width * 2,
                                                 imgWarter.Height * 2),
                        0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
            }

            return (Bitmap)imgSrc;
        }

        private void StartUsbVideo()
        {            
            isNeedRecord = true;
            if (VideoOutPut != null)
            {
                VideoOutPut.Close();
                VideoOutPut.Dispose();
            }
            VideoOutPut = new VideoFileWriter();
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            videoPath = $"{dirName}\\{VideoName}";
            var frameRate = 21 * VideoCount;
            //VideoCount++;
            //打开录像文件(如果没有则创建,如果有也会清空).
            VideoOutPut.Open(videoPath, _usbCamera.VideoResolution.FrameSize.Width, _usbCamera.VideoResolution.FrameSize.Height, frameRate, VideoCodec.MPEG4, 10 * 1000000);
        }

        string videoPath = string.Empty;
        private void StartWifiVideo()
        {
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            videoPath = $"{dirName}\\{VideoName}";
            var frameRate = 25;//16 * VideoCount;
            ////VideoCount++;            
            writer.Open(videoPath, curFrame.Width, curFrame.Height, frameRate, VideoCodec.MPEG4, 10 * 1000000);

            Bitmap bitmap = (Bitmap)curFrame.Clone();
            var id = imgList.Count > 0 ? imgList.Max(m => m.Index) + 1 : 1;
            imgList.Add(new ImgInfo { Index = id, Date = DateTime.Now, InfoType = 1, VideoPath = videoPath, Img = MergeImg(bitmap) });

            while (isNeedRecord)
            {
                DateTime beforDT = System.DateTime.Now;
                writer.WriteVideoFrame(curFrame);
                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                if (ts.TotalMilliseconds < 40)
                    System.Threading.Thread.Sleep(40 - Convert.ToInt32(ts.TotalMilliseconds));
            }
            writer.Close();
        }

        private string VideoName
        {
            get
            {
                var files = Directory.GetFiles(dirName, "*.avi");
                int index = 0;
                string fileName = "General_A";
                foreach (var file in files)
                {
                    var str = file.Substring(file.LastIndexOf("\\")+1);
                    int i = 0;
                    int.TryParse(str.Replace(fileName, "").Replace(".avi", ""), out i);
                    if (i > index)
                        index = i;
                }
                return $"{fileName}{++index}.avi";
            }            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if(btnMax.Text == LanguageHelp.GetTextLanguage("全屏显示"))
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                if (pan对比.Visible)
                {
                    InitPanel(currentFormatIndex);
                }
                btnMax.Text = LanguageHelp.GetTextLanguage("退出全屏");
                this.Refresh();
            }
            else
            {
                ExitMax();
            }
        }

        private void btnYa_Click(object sender, EventArgs e)
        {
            var form = new FormToothMap2(currentImg != null, currentImg);
            form.ShowDialog();
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            isMirror = !isMirror;
            int value = 0;
            if (isMirror)
            {
                value = 1;
            }
            _usbCamera.SetCameraProperty(CameraControlProperty.Roll, value, CameraControlFlags.Manual);
        }

        private void FormPicture_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 27)
            {
                ExitMax();
            }
        }

        private void ExitMax()
        {
            //退出全屏
            btnMax.Text = LanguageHelp.GetTextLanguage("全屏显示");
            panelImgView.Visible = !panelImgView.Visible;
            panelVideo.Visible = !panelVideo.Visible;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            if (pan对比.Visible)
            {
                InitPanel(currentFormatIndex);
                for (int i = 0; i < pan对比.Controls.Count; i++)
                {
                    ((PictureBox)pan对比.Controls[i].Controls[0]).Image = pic对比List[i];
                }
            }
            panelImgView.Visible = !panelImgView.Visible;
            panelVideo.Visible = !panelVideo.Visible;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var form = new FormFormat();
            if (form.ShowDialog() == DialogResult.OK)
            {
                InitPanel(form.SelectedIndex + 1);
            }
        }

        private int picIndex;
        private int currentFormatIndex;

        private void InitPanel(int index)
        {
            picIndex = 0;
            panel2.Visible = false;
            pan对比.Visible = true;            
            pan对比.Controls.Clear();
            currentFormatIndex = index;
            if (index == 1)
            {
                var width = (pan对比.Width / 2) - 8;
                var height = pan对比.Height - 3;

                Panel p1 = new Panel();
                p1.ForeColor = Color.White;
                p1.Location = new Point(3, 3);
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Name = "p1";                
                p1.Size = new Size(width, height);
                PictureBox pic1 = new PictureBox();
                pic1.Dock = DockStyle.Fill;
                pic1.Tag = 1;
                pic1.SizeMode = PictureBoxSizeMode.Zoom;
                p1.Controls.Add(pic1);
                pic1.Click += Pic_Click;

                Panel p2 = new Panel();
                p2.ForeColor = Color.White;
                p2.Location = new Point(3 + width + 2, 3);
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Name = "p2";
                p2.Size = new Size(width, height);
                var pic2 = new PictureBox();
                pic2.Dock = DockStyle.Fill;
                pic2.Tag = 2;
                pic2.SizeMode = PictureBoxSizeMode.Zoom;
                p2.Controls.Add(pic2);
                pic2.Click += Pic_Click;

                pan对比.Controls.Add(p1);
                pan对比.Controls.Add(p2);                
            }
            else if (index == 2)
            {
                var width = (pan对比.Width / 2) - 8;
                var height = (pan对比.Height / 2) - 8;

                Panel p1 = new Panel();
                p1.ForeColor = Color.White;
                p1.Location = new Point(3, 3);
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Name = "p1";
                p1.Size = new Size(width, height);
                var pic1 = new PictureBox();
                pic1.Tag = 1;
                pic1.Dock = DockStyle.Fill;
                pic1.SizeMode = PictureBoxSizeMode.Zoom;
                pic1.Click += Pic_Click;
                p1.Controls.Add(pic1);

                Panel p2 = new Panel();
                p2.ForeColor = Color.White;
                p2.Location = new Point(3 + width + 2, 3);
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Name = "p2";
                p2.Size = new Size(width, height);
                var pic2 = new PictureBox();
                pic2.Tag = 2;
                pic2.Dock = DockStyle.Fill;
                pic2.SizeMode = PictureBoxSizeMode.Zoom;
                pic2.Click += Pic_Click;
                p2.Controls.Add(pic2);

                Panel p3 = new Panel();
                p3.ForeColor = Color.White;
                p3.Location = new Point(3, 3 + height + 2);
                p3.BorderStyle = BorderStyle.FixedSingle;
                p3.Name = "p3";
                p3.Size = new Size(width, height);
                var pic3 = new PictureBox();
                pic3.Tag = 3;
                pic3.Click += Pic_Click;
                pic3.SizeMode = PictureBoxSizeMode.Zoom;
                pic3.Dock = DockStyle.Fill;
                p3.Controls.Add(pic3);

                Panel p4 = new Panel();
                p4.ForeColor = Color.White;
                p4.Location = new Point(3 + width + 2, 3 + height + 2);
                p4.BorderStyle = BorderStyle.FixedSingle;
                p4.Name = "p4";
                p4.Size = new Size(width, height);
                var pic4 = new PictureBox();
                pic4.Tag = 4;
                pic4.Dock = DockStyle.Fill;
                pic4.SizeMode = PictureBoxSizeMode.Zoom;
                pic4.Click += Pic_Click;
                p4.Controls.Add(pic4);

                pan对比.Controls.Add(p1);
                pan对比.Controls.Add(p2);
                pan对比.Controls.Add(p3);
                pan对比.Controls.Add(p4);
            }
            else if (index == 3)
            {
                var width = (pan对比.Width / 4) - 12;
                var height = (pan对比.Height / 2) - 8;

                Panel p1 = new Panel();
                p1.ForeColor = Color.White;
                p1.Location = new Point(3, 3);
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Name = "p1";
                p1.Size = new Size(width, height);
                var pic1 = new PictureBox();
                pic1.Dock = DockStyle.Fill;
                pic1.Tag = 1;
                pic1.Click += Pic_Click;
                pic1.SizeMode = PictureBoxSizeMode.Zoom;
                p1.Controls.Add(pic1);                

                Panel p2 = new Panel();
                p2.ForeColor = Color.White;
                p2.Location = new Point(3 + width + 2, 3);
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Name = "p2";
                p2.Size = new Size(width, height);
                var pic2 = new PictureBox();
                pic2.Dock = DockStyle.Fill;
                pic2.Tag = 2;
                pic2.SizeMode = PictureBoxSizeMode.Zoom;
                pic2.Click += Pic_Click;
                p2.Controls.Add(pic2);                

                Panel p3 = new Panel();
                p3.ForeColor = Color.White;
                p3.Location = new Point(3 + width * 2 + 4, 3);
                p3.BorderStyle = BorderStyle.FixedSingle;
                p3.Name = "p3";
                p3.Size = new Size(width, height);
                var pic3 = new PictureBox();
                pic3.Dock = DockStyle.Fill;
                pic3.Click += Pic_Click;
                pic3.Tag = 3;
                pic3.SizeMode = PictureBoxSizeMode.Zoom;
                p3.Controls.Add(pic3);                

                Panel p4 = new Panel();
                p4.ForeColor = Color.White;
                p4.Location = new Point(3 + width * 3 + 6, 3);
                p4.BorderStyle = BorderStyle.FixedSingle;
                p4.Name = "p4";
                p4.Size = new Size(width, height);
                var pic4 = new PictureBox();
                pic4.Dock = DockStyle.Fill;
                pic4.Click += Pic_Click;
                pic4.Tag = 4;
                pic4.SizeMode = PictureBoxSizeMode.Zoom;
                p4.Controls.Add(pic4);

                Panel p5 = new Panel();
                p5.ForeColor = Color.White;
                p5.Location = new Point(3, 3 + height + 2);
                p5.BorderStyle = BorderStyle.FixedSingle;
                p5.Name = "p5";
                p5.Size = new Size(width, height);
                var pic5 = new PictureBox();
                pic5.Dock = DockStyle.Fill;
                pic5.Click += Pic_Click;
                pic5.Tag = 5;
                pic5.SizeMode = PictureBoxSizeMode.Zoom;
                p5.Controls.Add(pic5);

                Panel p6 = new Panel();
                p6.ForeColor = Color.White;
                p6.Location = new Point(3 + width + 2, 3 + height + 2);
                p6.BorderStyle = BorderStyle.FixedSingle;
                p6.Name = "p6";
                p6.Size = new Size(width, height);
                var pic6 = new PictureBox();
                pic6.Dock = DockStyle.Fill;
                pic6.Click += Pic_Click;
                pic6.Tag = 6;
                pic6.SizeMode = PictureBoxSizeMode.Zoom;
                p6.Controls.Add(pic6);

                Panel p7 = new Panel();
                p7.ForeColor = Color.White;
                p7.Location = new Point(3 + width * 2 + 4, 3 + height + 2);
                p7.BorderStyle = BorderStyle.FixedSingle;
                p7.Name = "p7";
                p7.Size = new Size(width, height);
                var pic7 = new PictureBox();
                pic7.Dock = DockStyle.Fill;
                pic7.Click += Pic_Click;
                pic7.Tag = 7;
                pic7.SizeMode = PictureBoxSizeMode.Zoom;
                p7.Controls.Add(pic7);

                Panel p8 = new Panel();
                p8.ForeColor = Color.White;
                p8.Location = new Point(3 + width * 3 + 6, 3 + height + 2);
                p8.BorderStyle = BorderStyle.FixedSingle;
                p8.Name = "p6";
                p8.Size = new Size(width, height);
                var pic8 = new PictureBox();
                pic8.Dock = DockStyle.Fill;
                pic8.Click += Pic_Click;                
                pic8.Tag = 8;
                pic8.SizeMode = PictureBoxSizeMode.Zoom;
                p8.Controls.Add(pic8);

                pan对比.Controls.Add(p1);
                pan对比.Controls.Add(p2);
                pan对比.Controls.Add(p3);
                pan对比.Controls.Add(p4);
                pan对比.Controls.Add(p5);
                pan对比.Controls.Add(p6);
                pan对比.Controls.Add(p7);
                pan对比.Controls.Add(p8);                
            }

            if(pic对比List.Count == 0)
            {
                pic对比List.Add(null);
                pic对比List.Add(null);
                pic对比List.Add(null);
                pic对比List.Add(null);
                pic对比List.Add(null);
                pic对比List.Add(null);
                pic对比List.Add(null);
                pic对比List.Add(null);
            }
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            int.TryParse(pic.Tag.ToString(), out picIndex);
            picIndex--;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Resetting();
        }

        public void Resetting()
        {
            pictureBox4.Image = (Image)oldImg.Clone();
            currentImg.Img = (Bitmap)oldImg.Clone();
        }

        private void OpenUsb()
        {
            while (isOpenUsb)
            {
                try
                {
                    byte[] msg = messageHelp.UsbMessage();
                    if (msg == null)
                    {
                        if (isOpenUsb)
                            CommHelp.ShowError("与设备通信失败，请关闭窗口，重新连接设备！");
                        break;
                    }
                    _lastTime = DateTime.Now;
                    var message = string.Join(" ", msg);
                    if (oldMessage != message && btnVideotape.Text == LanguageHelp.GetTextLanguage("录像") && panPai.Visible)
                    {
                        logger.Fatal(message);
                        oldMessage = message;
                        if (isLoad)
                        {
                            isLoad = false;
                            message = "0 1 0 0 0 1 0 0";
                        }
                        else
                            isMToP = true;
                        Action action = null;
                        if (UsbMessage.Orders.ContainsKey(message))
                        {
                            var type = UsbMessage.Orders[message];
                            switch (type)
                            {
                                case OrderType.MTP_Rule_View:
                                    action = RuleView;
                                    break;
                                case OrderType.MTP_Rule_See:
                                    action = RuleSee;
                                    break;
                                case OrderType.MTP_UV_View:
                                    action = UVView;
                                    break;
                                case OrderType.MTP_UV_See:
                                    action = UVSee;
                                    break;
                                case OrderType.MTP_ICON_View:
                                    action = ICONView;
                                    break;
                                case OrderType.MTP_ICON_See:
                                    action = ICONSee;
                                    break;
                                case OrderType.Sleep:
                                    action = Sleep;
                                    break;
                            }

                            if (action != null)
                                this.Invoke(new Action(() =>
                                {
                                    action();
                                }));
                        }
                    }
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "正在中止线程。")
                        break;
                    else
                        logger.Debug(ex.Message + ex.StackTrace);
                }
            }           
        }

        /// <summary>
        /// 是否是设备向PC发送的指令
        /// </summary>
        bool isMToP = false;
        string oldMessage;
        int currentPage;

        private void SetCurrentPage(int page)
        {
            if(currentPage != page)
            {
                while(tabControl1.TabPages[currentPage].Controls.Count > 0)
                {
                    tabControl1.TabPages[page].Controls.Add(tabControl1.TabPages[currentPage].Controls[0]);
                }
                currentPage = page;
                SetCameraProperty(tabControl1.SelectedIndex == 0 ? 0 : 1);
            }
        }

        /// <summary>
        /// 切换到微距拍摄模式
        /// </summary>
        private void RuleView()
        {
            try
            {
                if (isMToP)
                {
                    logger.Debug("Send:" + UsbMessage.OrdersB[OrderType.PReturn_Rule_View]);
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PReturn_Rule_View]);
                }
                else
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_Rule_View]);
                }
                tabControl1.SelectedTab = tabPage1;
                isMToP = false;
                SetCurrentPage(0);
                ShowVideo();
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        private void RuleSee()
        {
            try
            {
                //指令修改
                //if (isMToP)
                //{
                messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PReturn_Rule_See]);
                //}
                //else
                //{
                //    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_Rule_See]);
                //}
                //tabControl1.SelectedTab = tabPage1;
                isMToP = false;
                //SetCurrentPage(0);
                Pai();
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        private void UVView()
        {
            try
            {
                if (isMToP)
                {
                    logger.Debug("Send:" + UsbMessage.OrdersB[OrderType.PReturn_UV_View]);
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PReturn_UV_View]);
                }
                else
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_UV_View]);
                }
                tabControl1.SelectedTab = tabPage2;
                isMToP = false;
                SetCurrentPage(1);
                ShowVideo();
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        private void UVSee()
        {
            try
            {
                if (isMToP)
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PReturn_UV_See]);
                }
                else
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_UV_See]);
                }
                tabControl1.SelectedTab = tabPage2;
                isMToP = false;
                SetCurrentPage(1);
                Pai();
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        private void ICONView()
        {
            try
            {
                if (isMToP)
                {
                    logger.Debug("Send:" + UsbMessage.OrdersB[OrderType.PReturn_ICON_View]);
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PReturn_ICON_View]);
                }
                else
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_ICON_View]);
                }
                tabControl1.SelectedTab = tabPage3;
                isMToP = false;
                SetCurrentPage(2);
                ShowVideo();
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        private void ICONSee()
        {
            try
            {
                if (isMToP)
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PReturn_ICON_See]);
                }
                else
                {
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_ICON_See]);
                }
                tabControl1.SelectedTab = tabPage3;
                isMToP = false;
                SetCurrentPage(2);
                Pai();
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message + ex.StackTrace);
            }
        }

        private void Sleep()
        {
            if (IsWifi)
            {
                if(_wifiCamera.IsRunning)
                {
                    _wifiCamera.Stop();
                    pictureBox1.Image = null;
                    messageHelp.SendMessage(UsbMessage.OrdersB[OrderType.PSend_Close_Camenable]);
                }
            }
            else
            {
                if(_usbCamera.IsRunning)
                {
                    _usbCamera.Stop();
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTab = tabControl1.SelectedIndex;
            if (isMToP) return;
            if (tabControl1.SelectedIndex == 0)
                RuleView();
            else if (tabControl1.SelectedIndex == 1)
                UVView();
            else if (tabControl1.SelectedIndex == 2)
                ICONView();
            SetCameraParm();
            //SetCameraProperty(tabControl1.SelectedIndex == 0 ? 0: 1);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (btnVideotape.Text != LanguageHelp.GetTextLanguage("录像"))
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (imgList.Count > 0)
            {
                ShowImgOrVideo(imgList[0]);
            }                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (imgList.Count > 0)
            {
                ShowImgOrVideo(imgList[imgList.Count - 1]);
            }                
        }

        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if(currentImgIndex > 0)
                ShowImgOrVideo(imgList[--currentImgIndex]);
        }

        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (currentImgIndex < imgList.Count - 1)
                ShowImgOrVideo(imgList[++currentImgIndex]);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            DeleteCurrentImg();
        }

        /// <summary>
        /// 右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteCurrentImg();
        }

        private void DeleteCurrentImg()
        { 
            if(currentImgIndex >=0)
            {
                var img = new List<ImgInfo>();
                img.Add(imgList[currentImgIndex]);
                delList.Add(img);
                imgList.RemoveAt(currentImgIndex);

                if (currentImgIndex > 0)
                    currentImgIndex--;

                if(imgList.Count == 0)
                {
                    pictureBox4.Image = null;
                    for (int i = 1; i <= 10; i++)
                    {
                        imgSource.GetType().GetProperty("Image" + i).SetValue(imgSource, global::DentalImaging.Properties.Resources._1543660125_1_, null);
                    }
                    dataGridView1.Refresh();
                }
                else
                {
                    var info = imgList[currentImgIndex];
                    ShowImgOrVideo(info);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //LanguageHelp.Save();
            if (delList.Count == 0) return;
            var imgs = delList.Last();
            foreach(var img in imgs)
                imgList.Insert(img.Index - 1, img);
            delList.RemoveAt(delList.Count - 1);
            if(currentImgIndex >= imgList.Count)
            {
                currentImgIndex = 0;
            }
            var info = imgList[currentImgIndex];
            ShowImgOrVideo(info);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var img = new List<ImgInfo>();
            for (int i =0;i<imgList.Count;i++)
            {                
                img.Add(imgList[i]);
            }
            if(img.Count > 0) delList.Add(img);
            imgList.Clear();
            pictureBox4.Image = null;
            for (int i = 1; i <= 10; i++)
            {
                imgSource.GetType().GetProperty("Image" + i).SetValue(imgSource, global::DentalImaging.Properties.Resources._1543660125_1_, null);
            }
            dataGridView1.Refresh();
        }

        VideoParm ParmA;
        VideoParm ParmB;
        VideoParm ParmC;
        VideoParm CurrentParm;

        private void SetCameraParm()
        {
            if(ParmA == null)
            {
                var path = Application.StartupPath;
                var aPath = Path.Combine(path, "Microspur.set");
                var bPath = Path.Combine(path, "Panorama.set");
                var cPath = Path.Combine(path, "Portrait.set");
                if (File.Exists(aPath))
                {
                    var str = File.ReadAllText(aPath);
                    ParmA = JsonConvert.DeserializeObject<VideoParm>(str);
                }
                if (File.Exists(bPath))
                {
                    var str = File.ReadAllText(bPath);
                    ParmB = JsonConvert.DeserializeObject<VideoParm>(str);
                }
                if (File.Exists(cPath))
                {
                    var str = File.ReadAllText(cPath);
                    ParmC = JsonConvert.DeserializeObject<VideoParm>(str);
                }
            }

            CurrentParm = ParmA;
            if (tabControl1.SelectedIndex == 1)
                CurrentParm = ParmB;
            else if (tabControl1.SelectedIndex == 2)
                CurrentParm = ParmC;

            if (_usbCamera != null)
            {
                if(CurrentParm != null)
                {
                    _usbCamera.SetVideoProperty(VideoProcAmpProperty.Brightness, CurrentParm.Liangdu, VideoProcAmpFlags.Manual);
                    _usbCamera.SetVideoProperty(VideoProcAmpProperty.Contrast, CurrentParm.Duibidu, VideoProcAmpFlags.Manual);
                    _usbCamera.SetVideoProperty(VideoProcAmpProperty.Sharpness, CurrentParm.Qingxidu, VideoProcAmpFlags.Manual);
                    _usbCamera.SetVideoProperty(VideoProcAmpProperty.Saturation, CurrentParm.Baohedu, VideoProcAmpFlags.Manual);
                    _usbCamera.SetVideoProperty(VideoProcAmpProperty.Gamma, CurrentParm.Gama, VideoProcAmpFlags.Manual);
                    _usbCamera.SetVideoProperty(VideoProcAmpProperty.BacklightCompensation, CurrentParm.Niguangduibi, VideoProcAmpFlags.Manual);
                    //_usbCamera.SetVideoProperty(VideoProcAmpProperty.Hue, CurrentParm.Sediao, VideoProcAmpFlags.Manual);
                }
            }
            else if(IsWifi)
            {
                SendCameraParmMessage("{\"Camenable\":\"1\"}");
                if (CurrentParm != null)
                {
                    SendCameraParmMessage("{\"Resolution\":\"" + CurrentParm.Fenbian + "\"}");
                    SendCameraParmMessage("{\"Resolution\":\"" + CurrentParm.Fenbian + "\"}");
                    SendCameraParmMessage("{\"Brightness\":" + CurrentParm.Liangdu + "}");
                    SendCameraParmMessage("{\"Contrast\":" + CurrentParm.Duibidu + "}");
                    SendCameraParmMessage("{\"Saturation\":" + CurrentParm.Baohedu + "}");
                    //SendCameraParmMessage("{\"Hue\":" + CurrentParm.Sediao + "}");
                    SendCameraParmMessage("{\"Sharpness\":" + CurrentParm.Qingxidu + "}");
                    SendCameraParmMessage("{\"Gain\":" + CurrentParm.Zengyi + "}");
                    SendCameraParmMessage("{\"Gamma\":" + CurrentParm.Gama + "}");
                    var baoguang = CurrentParm.BaoguangAuto ? "{\"ExposureAuto\":0}" : "{\"ExposureAuto\":1,\"ExposureAbsolute\":" + CurrentParm.Baoguang + "}";
                    SendCameraParmMessage(baoguang);
                    var baip = CurrentParm.BaipinghengAuto ? "{\"WhiteBalance\":0}" : "{\"WhiteBalance\":1,\"WhiteBalanceTemperature\":" + CurrentParm.Baipingheng + "}";
                    SendCameraParmMessage(baoguang);
                    var duijiao = CurrentParm.DuijiaoAuto ? "{\"FocusingAuto\":0}" : "{\"FocusingAuto\":1,\"FocusingAbsolute\":" + CurrentParm.Duijiao + "}";
                    SendCameraParmMessage(duijiao);
                }
            }
        }

        private void SetCameraProperty(int value)
        {
            if (!IsWifi)
                //_usbCamera.SetCameraProperty(CameraControlProperty.Pan, value, CameraControlFlags.Manual);
                _usbCamera.SetVideoProperty(VideoProcAmpProperty.Hue, value, VideoProcAmpFlags.Manual);
            else
                SendCameraParmMessage("{\"Hue\":" + value + "}");
        }

        private void SendCameraParmMessage(string message)
        {
            this.BeginInvoke(new Action(() => { messageHelp.SendMessage(message); }));
            Thread.Sleep(10);
        }

        private void FormPicture_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if(IsWifi)
            {
                var form = new VideoSettingWifi();
                form.ShowDialog();                
            }
            else
            {
                var form = new VideoSetting(_usbCamera);
                form.ShowDialog();
            }
            ParmA = ParmB = ParmC = null;
            SetCameraParm();
        }
    }

    public class BindImgListSource
    {
        public Bitmap Image1 { get; set; }
        public Bitmap Image2 { get; set; }
        public Bitmap Image3 { get; set; }
        public Bitmap Image4 { get; set; }
        public Bitmap Image5 { get; set; }
        public Bitmap Image6 { get; set; }
        public Bitmap Image7 { get; set; }
        public Bitmap Image8 { get; set; }
        public Bitmap Image9 { get; set; }
        public Bitmap Image10 { get; set; }
    }
}
