using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.databaase;

namespace WinFormsApp1
{

    public partial class Form3 : Form
    {
        private Student studentToEdit ;
        private Form1 form1Reference;
        public Form3(Form1 form1, Student student)
        {
            InitializeComponent();
            PopulateClassDropdown();
            form1Reference = form1;
            studentToEdit = student;

            // Populate the fields with student data
            PopulateFields(student);
        }
        private void PopulateFields(Student student)
        {

            if (student != null)
            {
                textBox1.Text = student.Name;
               
                comboBox1.SelectedValue = student.ClassId;
                dateTimePicker1.Value=(DateTime)student.Dob;
                MessageBox.Show(student.Gender.ToString());
                string genderString = student.Gender.ToString();
                if (genderString == "1")
                {
                  
                    radioButton1.Checked = true; // Female
                    radioButton2.Checked = false;
                }

                if (genderString == "2")
                {
                 
                    radioButton2.Checked = true; // Female
                    radioButton1.Checked = false;
                }
               
            }
        }

        private void PopulateClassDropdown()
        {
            using (var _dbContext = new StudentManagementContext())
            {
                // Query the database for classes
                var classList = _dbContext.Classes
                    .Select(c => new
                    {
                        ClassId = c.Id, // Using ClassId to avoid confusion with the built-in 'Id' property of ComboBox items
                        ClassName = c.Name
                    })
                    .ToList();

                // Set the class list as the datasource for the ComboBox
                comboBox1.DataSource = classList; // Make sure your ComboBox is named classDropdown or change this to match your ComboBox's name
                comboBox1.DisplayMember = "ClassName"; // Display the class name in the ComboBox
                comboBox1.ValueMember = "ClassId"; // Use the class Id as the underlying value
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Guid studentId = this.studentToEdit.Id;
            using (var context=new StudentManagementContext())
            {
                var student = context.Students.Find(studentId);
                if (student != null)
                {
                    student.Name=textBox1.Text;
                    student.Dob = dateTimePicker1.Value;
                    string className = comboBox1.Text;
                    int classId = GetClassIdByName(className);
                    student.ClassId = classId;
                    student.Gender = radioButton1.Checked ? 1 : 2;
                    student.ModificationDate = DateTime.UtcNow;
                    context.SaveChanges();
                    MessageBox.Show("Successfully Updated");
                    this.Hide();
                    form1Reference.GetAllStudentList();

                }
            }

        }
        private int GetClassIdByName(string className)
        {
            using (var _dbContext = new StudentManagementContext())
            {
                var classEntity = _dbContext.Classes.FirstOrDefault(c => c.Name == className);
                return classEntity?.Id ?? -1; // Return -1 or handle the case where the class is not found.
            }
        }
    }
}
