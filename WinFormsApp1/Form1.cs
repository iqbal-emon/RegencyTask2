using System;
using System.Linq; // For LINQ operations
using System.Windows.Forms;
using WinFormsApp1.databaase;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetAllStudentList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Hide();
            Form2 form2 = new Form2(this); // 'this' refers to the current instance of Form1
            form2.Show();
            // -OR-

            // Show Form2 as a modal dialog
            // form2.ShowDialog(); // Use this if you want to prevent the user from interacting with Form1 while Form2 is open.
        }

        public void GetAllStudentList()
        {

            using (var _dbContext = new StudentManagementContext())
            {
                // Retrieve selected columns (ID, Name, DOB, ClassName, Gender) from the database
                var selectedStudents = _dbContext.Students
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Dob,
                ClassName = s.Class.Name, // Assuming there's a navigation property 'Class' in 'Student'
                Gender = s.Gender == 1 ? "Male" : "Female" // Correctly projecting a new value
            }).ToList();


                dataGridView1.DataSource = selectedStudents;
                dataGridView1.Columns["Id"].Visible = false; // This hides the 'Id' column
                AddActionButtons();
            }
            // Call the method to add action buttons after setting the data source

        }
        private void AddActionButtons()
        {
            // Check if the 'Edit' button column already exists
            if (!dataGridView1.Columns.Contains("editButton"))
            {
                // Add a button column for 'Edit'
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.UseColumnTextForButtonValue = true; // This property will make the button display the text
                editButton.HeaderText = "Edit";
                editButton.Name = "editButton"; // Name property is used for Contains check
                editButton.Text = "Edit";
                dataGridView1.Columns.Add(editButton);
            }

            // Repeat the same check for 'Details' and 'Delete' button columns
            if (!dataGridView1.Columns.Contains("detailsButton"))
            {
                // Add a button column for 'Details'
                DataGridViewButtonColumn detailsButton = new DataGridViewButtonColumn();
                detailsButton.UseColumnTextForButtonValue = true;
                detailsButton.HeaderText = "Details";
                detailsButton.Name = "detailsButton";
                detailsButton.Text = "Details";
                dataGridView1.Columns.Add(detailsButton);
            }

            if (!dataGridView1.Columns.Contains("deleteButton"))
            {
                // Add a button column for 'Delete'
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.UseColumnTextForButtonValue = true;
                deleteButton.HeaderText = "Delete";
                deleteButton.Name = "deleteButton";
                deleteButton.Text = "Delete";
                dataGridView1.Columns.Add(deleteButton);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            Guid studentId;

            // Check if the click is on a button column
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                string buttonText = senderGrid.Columns[e.ColumnIndex].HeaderText;

                switch (buttonText)
                {
                    case "Edit":
                        // Fetch the selected student's details
                        studentId = Guid.Parse(senderGrid["Id", e.RowIndex].Value.ToString());
                        Student studentToEdit = FetchStudentDetails(studentId);

                        // Open Form2 with the student details
                        using (Form3 form3 = new Form3(this, studentToEdit))
                        {
                            form3.ShowDialog();
                        }
                        break;
                    case "Details":
                        // Fetch the selected student's details
                        studentId = Guid.Parse(senderGrid["Id", e.RowIndex].Value.ToString());
                        Student studentDetails = FetchStudentDetails(studentId);

                        // Open Form4 with the student details
                        using (Form4 form4 = new Form4(studentDetails))
                        {
                            form4.ShowDialog();
                        }
                        break;
                    case "Delete":
                        // Confirm user wants to delete
                        if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            try
                            {
                                // Parse the GUID from the DataGridView assuming the 'Id' column is the GUID of the student
                                studentId = Guid.Parse(senderGrid["Id", e.RowIndex].Value.ToString());

                                // Perform the delete operation
                                using (var _dbContext = new StudentManagementContext())
                                {
                                    // Use the GUID to find the student in the database
                                    var studentToDelete = _dbContext.Students.Find(studentId);
                                    if (studentToDelete != null)
                                    {
                                        _dbContext.Students.Remove(studentToDelete);
                                        _dbContext.SaveChanges();

                                        // Refresh the DataGridView
                                        GetAllStudentList();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Record not found or already deleted.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle any errors that occur during the delete
                                MessageBox.Show($"Error occurred: {ex.Message}");
                            }
                        }
                        break;
                    default:
                        MessageBox.Show("Unknown action.");
                        break;
                }
            }
        }

        private Student FetchStudentDetails(Guid id)
        {
            using (var _dbContext = new StudentManagementContext())
            {
                return _dbContext.Students.Find(id);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
