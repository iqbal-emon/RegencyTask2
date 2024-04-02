using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.databaase;

namespace WinFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4(Student student)
        {
            InitializeComponent();

            Console.WriteLine(student);

            label4.Text = student.Class?.Name ?? "";


            label2.Text = student.Name ?? ""; // Use the null-coalescing operator to handle null values
            label7.Text = student.Dob?.ToString("d") ?? ""; // Use the null-conditional operator and null-coalescing operator
            label3.Text = student.Gender == 1 ? "Male" : "Female";

        }

       
    }
}
