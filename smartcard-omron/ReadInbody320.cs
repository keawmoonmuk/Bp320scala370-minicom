using smartcard_omron.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        //private static ThaiNationalIDCardReader cardReader;
        //private static PersonalPhoto personalPhoto;
        
      
       public Patients patient = new Patients();  //import patient
       static bool _continue;
       static SerialPort _serialport;
       public Data data = new Data();

        public ReadInbody320()
        {
            InitializeComponent();
            
            //try
            //{
            //    cardReader = null;
            //    personalPhoto = null;
            //    cardReader = new ThaiNationalIDCardReader();  // new method
            //    personalPhoto = cardReader.GetPersonalPhoto();

            //    if (personalPhoto.CitizenID == patient.IDCard) return;

                //ReaddataInbody320();  //read inbody 320
                timer_readinbody320.Start();

            //    if (data.Sys != null)
            //    {
            //        ReadInbody370 forminbody370 = new ReadInbody370();
            //        forminbody370.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    //Cleardata();

            //}
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
                form370.Show();
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

                                Console.WriteLine(Data.Sys);
                                Console.WriteLine(Data.Dia);
                                Console.WriteLine(Data.Pr);
                              
                            }
                            //lenght = 64
                            //if(bytes.Count == 64)
                            //{
                            //    //sys
                            //    char sys_s = Convert.ToChar(bytes[34]);
                            //    char sys_y = Convert.ToChar(bytes[35]);
                            //    char sys_ss = Convert.ToChar(bytes[36]);

                            //    //dia
                            //    char dia_d = Convert.ToChar(bytes[44]);
                            //    char dia_i = Convert.ToChar(bytes[45]);
                            //    char dia_a = Convert.ToChar(bytes[46]);
                            //    //pr
                            //    char pr_p = Convert.ToChar(bytes[49]);
                            //    char pr_r = Convert.ToChar(bytes[50]);
                            //    char pr_pr = Convert.ToChar(bytes[51]);

                            //    string datetimenow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            //    string sys = sys_s.ToString() + sys_y.ToString() + sys_ss.ToString();
                            //    string dia = dia_d.ToString() + dia_i.ToString() + dia_a.ToString();
                            //    string pr = pr_p.ToString() + pr_r.ToString() + pr_pr.ToString();

                            //    //data.Map = map;
                            //    Data.Sys = sys;
                            //    Data.Dia = dia;
                            //    Data.Pr = pr;
                            //    Data.datetime = datetimenow;

                            //    Console.WriteLine(Data.Sys);
                            //    Console.WriteLine(Data.Dia);
                            //    Console.WriteLine(Data.Pr);
                            //}
                            //if (bytes.Count == 64) break;
                            if (bytes.Count == 192) break;
                        }

                        //if (bytes.Count == 64) break;
                        if (bytes.Count == 192) break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Not connection serial port", ex);

            }
        }

    }
}
