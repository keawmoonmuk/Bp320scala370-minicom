using smartcard_omron.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNationalIDCard.NET;
using ThaiNationalIDCard.NET.Models;

namespace SmartcardApp
{
    public partial class InsertSmartcard : Form
    {
        public static ThaiNationalIDCardReader cardReader;
        public static PersonalPhoto personalPhoto;

 
        public InsertSmartcard()
        {
            InitializeComponent();
 
            time_checkreadsmartcard.Start();
        }

        //check and readsmartcard
        private void time_checkreadsmartcard_Tick(object sender, EventArgs e)
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
                        ReadInbody320 formreaddata_inbody320 = new ReadInbody320();

                        //formreaddata_inbody320.Getdata(patient);
                        this.Hide();                    //hide form Insertsmartcard
                        formreaddata_inbody320.Show();  // show form ReadInbody320
                        time_checkreadsmartcard.Stop();

                        return;
                    }

                    ReadSmartcard(personalPhoto);  // read smartcard

                }
                catch(Exception ex)
                {
                    Console.WriteLine("error read smartcard  =", ex);

                }
               
                break;
            }

        }

        //------------read smartcard reader---------------------
        public void ReadSmartcard(PersonalPhoto personalPhoto)
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

            string convert_dateofbrith = formatdate(dateofbirth);
            string convert_issuedate = formatdate(issuedate);
            string convert_expire = formatdate(expire);

            Patients.DateOfbrith = convert_dateofbrith;
            Patients.Issuedate = convert_issuedate;
            Patients.Expire = convert_expire;

        }

        //format datetime
        public string formatdate(DateTime date)
        {
            string date_format = date.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            return date_format;
        }


    }
}
