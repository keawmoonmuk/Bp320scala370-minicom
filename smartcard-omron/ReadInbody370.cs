using smartcard_omron;
using smartcard_omron.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNationalIDCard.NET;
using ThaiNationalIDCard.NET.Models;

namespace SmartcardApp
{
    public partial class ReadInbody370 : Form
    {

        //private static ThaiNationalIDCardReader cardReader;
        //private static PersonalPhoto personalPhoto;

       public  Patients patient = new Patients();  //import patient
       static bool _continue;
       static SerialPort _serialport;
       public Data data = new Data();

        Timer t = new Timer();


        //===================log================      
        private string msg = "";
        private string error = "";
        string directory_root = @"C:\LOG";

        public ReadInbody370()
        {
            InitializeComponent();

                timer_readdatainbody370.Start();

        }
        //timer read data 370
        private void timer_readdatainbody370_Tick(object sender, EventArgs e)
        {
           
            if (Data.Width == null)
            {
                Console.WriteLine(Patients.IDCard);
                Console.WriteLine(Data.Sys);
                Console.WriteLine(Data.Dia);

                ReaddataInbody370();

            }
            else
            {
                Metro formresult = new Metro();

                this.Hide();       //hide form ReadInbody 370
                formresult.Show(); // show form resulat metro
                timer_readdatainbody370.Stop();
            }
        }

        //read serialport for inbody 370
        public void ReaddataInbody370()
        {
            try
            {
                //connect serial port  
                _serialport = new SerialPort();
                _serialport = new SerialPort("COM5", 19200, Parity.None, 8, StopBits.One);

                // Set the read/write timeouts  
                _serialport.ReadTimeout = 500;
                _serialport.WriteTimeout = 500;
                if (_serialport != null && !_serialport.IsOpen)
                {
                    Console.WriteLine("Comport is open....");
                    _serialport.Open();
                    _continue = true;

                    Console.WriteLine("Wait data....");

                    List<byte> bytes = new List<byte>();

                    while (_continue)
                    {
                        bytes.Clear();

                        while (true)
                        {

                            if (_serialport.BytesToRead == 0) continue;

                            int b = _serialport.ReadByte();  //dec

                            if (b < 0) continue;
                            bytes.Add((byte)b);

                            if (bytes.Count == 21)
                            {
                                //height
                                char height_1 = Convert.ToChar(bytes[6]);
                                char height_2 = Convert.ToChar(bytes[7]);
                                char height_3 = Convert.ToChar(bytes[8]);
                                char height_4 = Convert.ToChar(bytes[9]);

                                //width
                                char width_1 = Convert.ToChar(bytes[11]);
                                char width_2 = Convert.ToChar(bytes[12]);
                                char width_3 = Convert.ToChar(bytes[13]);
                                //bmi
                                char bmi_1 = Convert.ToChar(bytes[15]);
                                char bmi_2 = Convert.ToChar(bytes[16]);
                                char bmi_3 = Convert.ToChar(bytes[17]);

                                string datetimenow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                string height = height_1.ToString() + height_2.ToString() + height_3.ToString() + "." + height_4.ToString();
                                string width = width_1.ToString() + width_2.ToString() + "." + width_3.ToString();
                                string bmi = bmi_1.ToString() + bmi_2.ToString() + "." + bmi_3.ToString();

                                //data.Map = map;
                                Data.Height = height;
                                Data.Width = width;
                                Data.Bmi = bmi;
                                Data.datetime = datetimenow;

                                msg = "Read data Device Inbody 370  form serialport suecess...";
                                LogMessage();

                                Console.WriteLine(Data.Height);
                                Console.WriteLine(Data.Width);
                                Console.WriteLine(Data.Bmi);

                            }
                            if (bytes.Count == 21) break;
                        }


                        if (bytes.Count == 21) break;
                    }
                }
         
            }
            catch (Exception ex)
            {
                error = "Not connect Serialport.... ";
                LogMessageError();

                Console.WriteLine("Not connection serial port", ex);
            }

        }

        //=====================log complete===================
        public void LogMessage()
        {

            if (!Directory.Exists(directory_root))
            {
                Directory.CreateDirectory(directory_root);

            }

            StreamWriter stw = new StreamWriter(@"C:\Log\log.txt", true);
            stw.WriteLine($"TIME COMPLETE : {DateTime.Now} MESSAGE : {msg} -- Data Patient : {Patients.Th_firstname} - {Patients.Th_lastname}, {Patients.IDCard} {Patients.Gender} {Patients.DateOfbrith} DATA BP-9020 {Data.Sys}-{Data.Dia}- {Data.Map}- {Data.Pr} ");
            stw.Close();

        }

        //=======================log error=======================
        public void LogMessageError()
        {
            if (!Directory.Exists(directory_root))
            {
                Directory.CreateDirectory(directory_root);
            }
            StreamWriter stw = new StreamWriter(@"C:\Log\log.txt", true);
            stw.WriteLine($"TIME ERROR : {DateTime.Now}  Error Message : {error} ");
            stw.Close();
        }
    }
}
