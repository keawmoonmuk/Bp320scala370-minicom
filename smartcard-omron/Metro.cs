using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNationalIDCard.NET.Models;  //import library smartcard
using ThaiNationalIDCard.NET;
using smartcard_omron.Models;
using System.IO.Ports;
using System.Globalization;
using PCSC;
using SmartcardApp;
using System.Net;
using System.IO;

namespace smartcard_omron
{
    public partial class Metro : MetroFramework.Forms.MetroForm
    {
        private static ThaiNationalIDCardReader cardReader;
        private static PersonalPhoto personalPhoto;

        Patients patient = new Patients();  //import patient
        static bool _continue;
        static SerialPort _serialport;
        Data data = new Data();
    
        Timer t = new Timer();

        public Metro()
        {
            InitializeComponent();
              
              timer_checksmartcard.Start();                
           
        }


        //---------------timer form smartcard-----------------------
        public  void timer_checksmartcard_Tick(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    cardReader = null;
                    personalPhoto = null;
                    cardReader = new ThaiNationalIDCardReader();  // new method
                    personalPhoto = cardReader.GetPersonalPhoto();

                    if (personalPhoto.CitizenID == Patients.IDCard) 
                    {
                        Readdata_Result();

                        //send to server 

                        //Callwebservice();
                        return;
                    }
    
                    //ReadSmartcard(personalPhoto);  // read smartcard
           
                    timer_readdata320.Stop();
                }
                catch
                {
                    //Cleardata();
                    InsertSmartcard forminsertsmart = new InsertSmartcard();
                    forminsertsmart.Show();
                    timer_checksmartcard.Stop();

                    metroTxt_status.CustomBackground = true;
                    metroTxt_status.CustomForeColor = true;
                
                    metroTxt_status.Text = "กรุณาเสียบบัตรประชาชน...";
                    metroTxt_status.BackColor = Color.Red;
                    metroTxt_status.ForeColor = Color.Black;
                    metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);

                    timer_readdata320.Stop();
                    timer_readdata370.Stop();
                 
                }
                break;
            }
        }
        //--------------cal api-----------------
        public void Callwebservice()
        {
            string urlapi = "https://localhost:44398/api/DataPatient";                        //url
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(urlapi);
            httpWebRequest.ContentType = "application/json";                                 //herder
            httpWebRequest.Method = "POST";                                                  //method post

            using(var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"IDCard\" :" + Patients.IDCard+"," +
                               "{\"Th_prefix\" :" + Patients.Th_prefix + "," +
                               "{\"Th_firstname\" :" + Patients.Th_firstname + "," +
                               "{\"Th_lastname\" :" + Patients.Th_lastname + "," +
                               "{\"En_prefix\" :" + Patients.En_prefix + "," +
                               "{\"En_firstname\" :" + Patients.En_firstname + "," +
                               "{\"En_lastname\" :" + Patients.En_lastname + "," +
                               "{\"DateOfbrith\" :" + Patients.DateOfbrith + "," +
                               "{\"Gender\" :" + Patients.Gender + "," +
                               "{\"Houseno\" :" + Patients.Houseno + "," +
                               "{\"Valaigeno\" :" + Patients.Valaigeno + "," +
                               "{\"Lane\" :" + Patients.Lane + "," +
                               "{\"Road\" :" + Patients.Road + "," +
                               "{\"Subdistrict\" :" + Patients.Subdistrict + "," +
                               "{\"District\" :" + Patients.District + "," +
                               "{\"Province\" :" + Patients.Province + "," +
                               "{\"Issuedate\" :" + Patients.Issuedate + "," +
                               "{\"Issure\" :" + Patients.Issure + "," +
                               "{\"Expire\" :" + Patients.Expire + "," +
                               "{\"Sys\" :" + Data.Sys + "," +
                               "{\"Dia\" :" + Data.Dia + "," +
                               "{\"Pr\" :" + Data.Pr + "," +
                               "{\"Width\" :" + Data.Width + "," +
                               "{\"Height\" :" + Data.Height + "," +
                               "{\"Bmi\" :" + Data.Bmi + "," +
                               "{\"datetime\" :" + Data.datetime + "," +

                                "}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        //-------------form result all-----------------------
        public void Readdata_Result()
        {
            Refresh();
            string datetimenow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            //data smartcard all
            metroTxt_idcard.Text = Patients.IDCard;
            metroTxt_prefix_th.Text = Patients.Th_prefix;
            metroTxt_firstname_th.Text = Patients.Th_firstname;
            metroTxt_lastname_th.Text = Patients.Th_lastname;
            metroTxt_prefix_en.Text = Patients.En_prefix;
            metroTxt_firstname_en.Text = Patients.En_firstname;
            metroTxt_lastname_en.Text = Patients.En_lastname;
            metroTxt_dateOfbirth.Text = Patients.DateOfbrith;
            metroTxt_gender.Text = Patients.Gender;
            metroTxt_houseno.Text = Patients.Houseno;
            metroTxt_moo.Text = Patients.Valaigeno;
            metroTxt_lane.Text = Patients.Lane;
            metroTxt_road.Text = Patients.Road;
            metroTxt_subdistrat.Text = Patients.Subdistrict;
            metroTxt_distrat.Text = Patients.District;
            metroTxt_province.Text = Patients.Province;
            metroTxt_issuedate.Text = Patients.Issuedate;
            metroTxt_issure.Text = Patients.Issure;
            metroTxt_expire.Text = Patients.Expire;

            //data device 
            metroTxt_sys.Text = Data.Sys;
            metroTxt_dia.Text = Data.Dia;
            metroTxt_pr.Text = Data.Pr;
            metroTxt_width.Text = Data.Width;
            metroTxt_height.Text = Data.Height;
            metroTxt_bmi.Text = Data.Bmi;
            metroTxt_datetime.Text = datetimenow;

            metroTxt_status.Text = "การวัดเสร็จสิ้น กรุณาดึงบัตรประชาชนออก";
            metroTxt_status.BackColor = Color.GreenYellow;
            metroTxt_status.ForeColor = Color.Black;
            metroTxt_status.Font = new Font("Arial", 40, FontStyle.Bold);

        }


        //timer readdata form inbody 320
        public void timer_readdata_Tick(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    cardReader = null;
                    personalPhoto = null;
                    cardReader = new ThaiNationalIDCardReader();  // new method
                    personalPhoto = cardReader.GetPersonalPhoto();

                    if (Data.Sys == null)
                    {                   

                        ReaddataInbody320();                // read data for omron

                        metroTxt_status.Text = "กรุณา ทำการชั่งน้ำหนักและวัดส่วนสูง";
                        metroTxt_status.BackColor = Color.GreenYellow;
                        metroTxt_status.ForeColor = Color.Black;
                        metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);


                        timer_readdata370.Start();

                    return;

                    }
                }
                catch
                {
                    Cleardata();

                    metroTxt_status.CustomBackground = true;        
                    metroTxt_status.CustomForeColor = true;

                    metroTxt_status.Text = "กรุณาเสียบบัตรประชาชน...";
                    metroTxt_status.BackColor = Color.Red;
                    metroTxt_status.ForeColor = Color.Black;
                    metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);

                   
                }
                break;
            }
        }

        //time read data 370
        private void timer_readdata370_Tick(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    cardReader = null;
                    personalPhoto = null;
                    cardReader = new ThaiNationalIDCardReader();  // new method
                    personalPhoto = cardReader.GetPersonalPhoto();

                    if (Data.Width == null)
                    {
                      
                        ReaddataInbody370();                // read data for omron

                        metroTxt_status.Text = "การวัดเสร็จสิ้น กรุณาดึงบัตรประชาชนออก";
                        metroTxt_status.BackColor = Color.GreenYellow;
                        metroTxt_status.ForeColor = Color.Black;
                        metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);


                        return;

                    }
                }
                catch
                {
                    Cleardata();

                    metroTxt_status.CustomBackground = true;
                    metroTxt_status.CustomForeColor = true;

                    metroTxt_status.Text = "กรุณาเสียบบัตรประชาชน...";
                    metroTxt_status.BackColor = Color.Red;
                    metroTxt_status.ForeColor = Color.Black;
                    metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);

                }

                break;
            }

        }

        //read serialport for inbody 320
        public  void ReaddataInbody320()
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

                                string datetimenow =  DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                string sys = sys_s.ToString() + sys_y.ToString() + sys_ss.ToString();                           
                                string dia = dia_d.ToString() + dia_i.ToString() + dia_a.ToString();
                                string pr = pr_p.ToString() + pr_r.ToString() + pr_pr.ToString();
                                //string map = map_m.ToString() + map_a.ToString() + map_p.ToString();
                                //DateTime datenow = DateTime.ParseExact(dateformat, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                //data.Map = map;
                                Data.Sys = sys;                           
                                Data.Dia = dia;
                                Data.Pr = pr;                             
                                Data.datetime = datetimenow;
                             
                                metroTxt_sys.Text = sys;
                                metroTxt_dia.Text = dia;
                                metroTxt_pr.Text = pr;
                                //metroTxt_map.Text = map;

                                metroTxt_datetime.Text = datetimenow;

                                timer_readdata370.Start();
                                //ReaddataInbody370();  // read inbody 370

                            }
                            if (bytes.Count == 192) break;
                        }


                        if (bytes.Count == 192) break;
                    }
                }
           
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not connection serial port", ex);
             
                metroTxt_status.Text = "Can not Connect serialport 320";
                metroTxt_status.BackColor = Color.Red;
                metroTxt_status.ForeColor = Color.Black;
                metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);
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

                            if (bytes.Count == 64)
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
                                string height = height_1.ToString() + height_2.ToString() + height_3.ToString() + height_4.ToString();
                                string width = width_1.ToString() + width_2.ToString() + width_3.ToString();
                                string bmi = bmi_1.ToString() + bmi_2.ToString() + bmi_3.ToString();
                             
                                //data.Map = map;
                                Data.Sys = height;
                                Data.Dia = width;
                                Data.Pr = bmi;
                                Data.datetime = datetimenow;

                                metroTxt_width.Text = height;
                                metroTxt_height.Text = width;
                                metroTxt_bmi.Text = bmi;                           
                                metroTxt_datetime.Text = datetimenow;


                            }
                            if (bytes.Count == 64) break;
                        }


                        if (bytes.Count == 64) break;
                    }
                }
          
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not connection serial port", ex);

                metroTxt_status.Text = "Can not Connect serialport 370";
                metroTxt_status.BackColor = Color.Red;
                metroTxt_status.ForeColor = Color.Black;
                metroTxt_status.Font = new Font("Arial", 35, FontStyle.Bold);

            }

        }

        //------------read smartcard reader---------------------
        public  void  ReadSmartcard( PersonalPhoto personalPhoto)
        {      
            Refresh();         
            string idcard = personalPhoto.CitizenID;   //Idcard
            var thai_fullname = personalPhoto.ThaiPersonalInfo;  //----------th_fullname
            string th_firstname = thai_fullname.FirstName;  //firstname
            string th_lastname = thai_fullname.LastName;  //lastname
            string th_prefix = thai_fullname.Prefix;  //prefix
            string th_midlename = thai_fullname.MiddleName;  //middle name
            var en_fullname = personalPhoto.EnglishPersonalInfo;    //--------en_fullname
            string en_firstname = en_fullname.FirstName;  //firstname
            string en_lastname = en_fullname.LastName;  //lastname
            string en_prefix = en_fullname.Prefix;  //lastname
            string en_midlename = en_fullname.MiddleName;  //middle name en

            DateTime dateofbirth = personalPhoto.DateOfBirth;

            var gender = personalPhoto.Sex;           // sex when 1 = ชาย , 2 = หญิง
            if (gender == "1")
            {
                gender = "ชาย";
            }
            else
            {
                gender = "หญิง";
            }

            var address = personalPhoto.AddressInfo;
            string houseno = address.HouseNo;         //บ้านเลขที่
            string valiageno = address.VillageNo;     //หมู่ที่
            string lane = address.Lane;              // ตรอก ซอย
            if (lane == null) { lane = "-"; }
            string road = address.Road;              //ถนน
            if (road == null) { lane = "-"; }
            string subdistrat = address.SubDistrict; //ตำบล
            string district = address.District;      //อำเภอ
            string province = address.Province;      //จังหวัด

            DateTime issuedate = personalPhoto.IssueDate;     //วันออกบัตร
            DateTime expire = personalPhoto.ExpireDate;       //วันหมดอายุ
            string issure = personalPhoto.Issuer;    //สถานที่ออกบัตร

            var photo = personalPhoto.Photo;   //photo

            string convert_dateofbrith = formatdate(dateofbirth);
            string convert_issuedate = formatdate(issuedate);
            string convert_expire = formatdate(expire);

            Patients.IDCard = idcard;
            Patients.Th_prefix = th_prefix;
            Patients.Th_firstname = th_firstname;
            Patients.Th_lastname = th_lastname;
            Patients.Th_midlename = th_midlename;

            Patients.En_prefix = en_prefix;
            Patients.En_firstname = en_firstname;
            Patients.En_lastname = en_lastname;
            Patients.En_midlename = en_midlename;

            Patients.Gender = gender;
            //address
            Patients.Houseno = houseno;
            Patients.Valaigeno = valiageno;
            Patients.Lane = lane;
            Patients.Road = road;
            Patients.Subdistrict = subdistrat;
            Patients.District = district;
            Patients.Province = province;
            //  patient.DateOfbrith = dateofbirth;
            Patients.Issure = issure;  //สถานที่ออกบัตร

            Patients.DateOfbrith = convert_dateofbrith;
            Patients.Issuedate = convert_issuedate;
            Patients.Expire = convert_expire;

         

            metroTxt_idcard.Text = Patients.IDCard;
            metroTxt_prefix_th.Text = Patients.Th_prefix;
            metroTxt_firstname_th.Text = Patients.Th_firstname;
            metroTxt_lastname_th.Text = Patients.Th_lastname;

            metroTxt_prefix_en.Text = Patients.En_prefix;
            metroTxt_firstname_en.Text = Patients.En_firstname;
            metroTxt_lastname_en.Text = Patients.En_lastname;

            metroTxt_dateOfbirth.Text = convert_dateofbrith;
            metroTxt_gender.Text = Patients.Gender;
            metroTxt_houseno.Text = Patients.Houseno;
            metroTxt_moo.Text = Patients.Valaigeno;
            metroTxt_lane.Text = Patients.Lane;
            metroTxt_road.Text = Patients.Road;
            metroTxt_subdistrat.Text = Patients.Subdistrict;
            metroTxt_distrat.Text = Patients.District;
            metroTxt_province.Text = Patients.Province;

            metroTxt_issuedate.Text = convert_issuedate;
            metroTxt_issure.Text = Patients.Issure;
            metroTxt_expire.Text = convert_expire;

            Console.WriteLine(Data.Sys);
            Console.WriteLine(Data.Dia);
            Console.WriteLine(Data.Map);
            Console.WriteLine(Data.Pr);

          
        }

       
        //format datetime
        public string formatdate(DateTime date)
        {
            string date_format = date.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            return date_format;
        }

        //clear data
        public  void Cleardata()
        {         
            metroTxt_idcard.Text = "";
            metroTxt_prefix_th.Text = "";
            metroTxt_firstname_th.Text = "";
            metroTxt_lastname_th.Text = "";
            metroTxt_prefix_en.Text = "";
            metroTxt_firstname_en.Text = "";
            metroTxt_lastname_en.Text = "";
            metroTxt_dateOfbirth.Text = "";
            metroTxt_gender.Text = "";
            metroTxt_houseno.Text = "";
            metroTxt_moo.Text = "";
            metroTxt_lane.Text = "";
            metroTxt_road.Text = "";
            metroTxt_subdistrat.Text = "";
            metroTxt_distrat.Text = "";
            metroTxt_province.Text = "";
            metroTxt_issuedate.Text = "";
            metroTxt_issure.Text = "";
            metroTxt_expire.Text = "";
            metroTxt_width.Text = "";
            metroTxt_height.Text = "";
            metroTxt_bmi.Text = "";
            metroTxt_pr.Text = "";
            metroTxt_datetime.Text = "";
            metroTxt_sys.Text = "";
            metroTxt_dia.Text = "";
       
        }
    }
}
