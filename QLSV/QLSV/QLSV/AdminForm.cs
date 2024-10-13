using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class AdminForm : Form
    {
        private bool x = true;
        private int select;

        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Anchor = AnchorStyles.None;
            }
        }

        private void buttonMenu_Click(object sender, EventArgs e)
        {
            if(x)
            {
                buttonMenu.Location = new Point(0, 45);
                flowLayoutPanelMenu.Visible = false;
                buttonMenu.Visible = true;
                panelShow.Location = new Point(0, 99);
                panelShow.Size = new Size(1455, 700);
                x = false;
            }
            else
            {
                buttonMenu.Location = new Point(125,45);
                flowLayoutPanelMenu.Visible = true;
                panelShow.Location = new Point(200, 99);
                panelShow.Size = new Size(1255, 700);
                x = true;
            }
        }

        private void ShowFormInPanel(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelShow.Controls.Clear();
            panelShow.Controls.Add(form);
            form.Show();
        }


        private void buttonStudent_Click(object sender, EventArgs e)
        {
            select = 0;
            buttonAdd.Enabled = true;
            buttonRemove.Enabled = false;
            buttonEdit.Enabled = true;
            buttonGroup.Visible = false;
            StudentListForm studentListForm = new StudentListForm();
            ShowFormInPanel(studentListForm);
        }

        private void buttonCourse_Click(object sender, EventArgs e)
        {
            select = 1;
            buttonAdd.Enabled = true;
            buttonRemove.Enabled = true;
            buttonEdit.Enabled = true;
            buttonGroup.Visible = false;
            CourseAdminForm courseAdminForm = new CourseAdminForm();
            ShowFormInPanel(courseAdminForm);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(select == 0)
            {
                AddStudentForm studentForm = new AddStudentForm();
                studentForm.ShowDialog();
            }
            else if(select == 1)
            {
                AddCourse addCourse = new AddCourse();
                addCourse.ShowDialog();
            }
            else if(select == 2)
            {
                AddScoreForm addScoreForm = new AddScoreForm();
                addScoreForm.ShowDialog();
            }
            else if (select == 3)
            {
                HRAccountForm hRAccountForm = new HRAccountForm();
                hRAccountForm.ShowDialog();
            }
            else if(select == 4)
            {
                AddContactForm addContactForm = new AddContactForm();
                addContactForm.ShowDialog();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if(select == 1)
            {
                RemoveCourseForm removeCourseForm = new RemoveCourseForm();
                removeCourseForm.ShowDialog();
            }
            else if(select == 2)
            {
                RemoveScoreForm removeScoreForm = new RemoveScoreForm();
                removeScoreForm.ShowDialog();
            }
            else if(select == 3)
            {
                RemoveHRForm removeHRForm = new RemoveHRForm();
                removeHRForm.ShowDialog();
            }
            else if(select == 4)
            {
                RemoveContactForm removeContactForm = new RemoveContactForm();
                removeContactForm.ShowDialog();
            }
            
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if(select == 0)
            {
                ManageStudentForm manageStudentForm = new ManageStudentForm();
                manageStudentForm.ShowDialog();
            }
            else if(select == 1)
            {
                EditCourseForm editCourseForm = new EditCourseForm();
                editCourseForm.ShowDialog();
            }
            else if(select == 3)
            {
                EditHRForm editHRForm = new EditHRForm();
                editHRForm.ShowDialog();
            }
            else if(select == 4)
            {
                EditContactForm editContactForm = new EditContactForm();
                editContactForm.ShowDialog();
            }

        }

        private void buttonScore_Click(object sender, EventArgs e)
        {
            select = 2;
            buttonAdd.Enabled = true;
            buttonRemove.Enabled = true;
            buttonEdit.Enabled = false;
            buttonGroup.Visible = false;
            ScoreAdminForm scoreAdminForm = new ScoreAdminForm();
            ShowFormInPanel(scoreAdminForm);
        }

        private void buttonMember_Click(object sender, EventArgs e)
        {
            select = 3;
            buttonAdd.Enabled = true;
            buttonRemove.Enabled = true;
            buttonEdit.Enabled = true;
            buttonGroup.Visible = false;
            HRAdminForm hRAdminForm = new HRAdminForm();
            ShowFormInPanel(hRAdminForm);
        }

        private void buttonContact_Click(object sender, EventArgs e)
        {
            select = 4;
            buttonAdd.Enabled = true;
            buttonRemove.Enabled = true;
            buttonEdit.Enabled = true;
            buttonGroup.Visible = true;
            ContactAdminForm contactAdminForm = new ContactAdminForm();
            ShowFormInPanel(contactAdminForm);
        }

        private void buttonGroup_Click(object sender, EventArgs e)
        {
            if(select == 4)
            {
                ManageGroupForm manageGroupForm = new ManageGroupForm();

            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login_Form login = new Login_Form();
            login.ShowDialog();
            this.Close();
        }

        private void buttonStatic_Click(object sender, EventArgs e)
        {
            buttonAdd.Enabled = false;
            buttonRemove.Enabled = false;
            buttonEdit.Enabled = false;
            buttonGroup.Visible = false;
            StaticAdminForm staticAdminForm = new StaticAdminForm();
            ShowFormInPanel(staticAdminForm);
        }
    }
}
