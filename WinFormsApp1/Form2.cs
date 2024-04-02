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
    public partial class Form2 : Form
    {
        // Inside Form2
        private Student studentToEdit;
        private Form1 form1Reference;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            PopulateClassDropdown();
            form1Reference = form1;
           
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string className = comboBox1.Text; // Get the selected class name directly from the ComboBox
            DateTime dob = dateTimePicker1.Value;
            int gender = radioButton1.Checked ? 1 : 2; // 1 for male, 2 for female
           

            // Assuming you have proper validation for name, class selection, and date of birth

            MessageBox.Show($"Name: {name}\nClass: {className}\nDate of Birth: {dob}\nGender: {(gender == 1 ? "Male" : "Female")}");

            // You need to retrieve the ClassId corresponding to the selected ClassName from the database.
            // This requires a database call, assumed here as a method GetClassIdByName(className).
            int classId = GetClassIdByName(className);

            // Now create the student entity
            Student newStudent = new Student
            {
                Name = name,
                ClassId = classId, // Assuming ClassId is a foreign key to the Class table.
                Dob = dob,
                Gender = gender,
                CreatedDate= DateTime.UtcNow,
                // Set other properties like Id, CreatedDate, ModificationDate as necessary
            };
            this.Hide();
            // Somewhere in Form2, when you need to call GetAllStudentList





            // Now insert the new student into the database using the context
            using (var _dbContext = new StudentManagementContext())
            {
                _dbContext.Students.Add(newStudent);
                _dbContext.SaveChanges();
            form1Reference.GetAllStudentList();
            }
        }


        // Method to get ClassId by ClassName (you'll need to implement this according to your context and database setup)
        private int GetClassIdByName(string className)
        {
            using (var _dbContext = new StudentManagementContext())
            {
                var classEntity = _dbContext.Classes.FirstOrDefault(c => c.Name == className);
                return classEntity?.Id ?? -1; // Return -1 or handle the case where the class is not found.
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
