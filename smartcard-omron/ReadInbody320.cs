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
    public partial class ReadInbody320 : Form
    {

       public Patients patient = new Patients();  //import patient
       static bool _continue;
       static SerialPort _serialport;
       public Data data = new Data();


        //=================log==============  
        private string msg = "";
        private string error = "";
        string directory_root = @"C:\LOG";
        public ReadInbody320()
        {
            InitializeComponent();
                        
                timer_readinbody320.Start();
       
        }

        //user timer read inbody 320
        private void timer_readinbody320_Tick(object sender, EventArgs e)
        {

            Console.WriteLine(Patients.IDCard);
            if (Data.Sys == null)
            {
               string idc =  Patients.IDCard;
                Console.WriteLine(idc);
                ReaddataInbody320();
            }
            else
            {
                ReadInbody370 form370 = new ReadInbody370();

                this.Hide();     // hide form Readinbody 320
                form370.Show(); // show form ReadInbody470
                timer_readinbody320.Stop();
            }
        }


        //read serialport for inbody 320
        public void ReaddataInbody320()
        {

            try
            {
                //connect serial port  
                _serialport = new SerialPort();
                _serialport = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

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

                            if (bytes.Count == 192)
                            {
                                //sys
                                char sys_s = Convert.ToChar(bytes[162]);
                                char sys_y = Convert.ToChar(bytes[163]);
                                char sys_ss = Convert.ToChar(bytes[164]);

                                //dia
                                char dia_d = Convert.ToChar(bytes[172]);
                                char dia_i = Convert.ToChar(bytes[173]);
                                char dia_a = Convert.ToChar(bytes[174]);
                                //pr
                                char pr_p = Convert.ToChar(bytes[177]);
                                char pr_r = Convert.ToChar(bytes[178]);
                                char pr_pr = Convert.ToChar(bytes[179]);

                                string datetimenow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                string sys = sys_s.ToString() + sys_y.ToString() + sys_ss.ToString();
                                string dia = dia_d.ToString() + dia_i.ToString() + dia_a.ToString();
                                string pr = pr_p.ToString() + pr_r.ToString() + pr_pr.ToString();
                              
                                //data.Map = map;
                                Data.Sys = sys;
                                Data.Dia = dia;
                                Data.Pr = pr;
                                Data.datetime = datetimenow;

                                msg = "Read data Device 320 form serialport suecess...";
                                LogMessage();

                                Console.WriteLine(Data.Sys);
                                Console.WriteLine(Data.Dia);
                                Console.WriteLine(Data.Pr);
                              
                            }
                      
                            if (bytes.Count == 192) break;
                        }

                        //if (bytes.Count == 64) break;
                        if (bytes.Count == 192) break;
                    }
                }

            }
            catch (Exception ex)
            {
                error = "Not connect Serialport.... ";
                LogMessageError();
                Console.WriteLine("Not connection serial port", ex);

                Console.WriteLine("Not connection serial port", ex);

            }
        }


        //==============log complete===================
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

        //===================log error====================
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
