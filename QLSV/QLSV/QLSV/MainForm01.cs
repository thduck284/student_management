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
    public partial class MainForm01 : Form
    {
        public MainForm01()
        {
            InitializeComponent();
        }

        private void addNewStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStudentForm addStdF = new AddStudentForm();
            addStdF.Show(this);
        }

        private void rToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentListForm studentlist = new StudentListForm();
            //studentlist.LoadStudentDetails();
            studentlist.ShowDialog(this);
            /*using (StudentListForm detailsForm = new StudentListForm())
            {
                detailsForm.LoadStudentDetails();
                detailsForm.ShowDialog();
            }*/
        }

        private void editRemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (UpdateDeleteStudentForm updelstudent = new UpdateDeleteStudentForm())
            {
                updelstudent.ShowDialog();             
            }
        }

        private void staticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticsForm statics = new StaticsForm();
            statics.ShowDialog(this);
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageStudentForm managestudent = new ManageStudentForm();
            managestudent.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintStudentForm printStudentForm = new PrintStudentForm();
            printStudentForm.ShowDialog();
        }

        private void addCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCourse addcourse = new AddCourse();
            addcourse.ShowDialog();
        }

        private void removeCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCourseForm removecourse = new RemoveCourseForm();
            removecourse.ShowDialog();
        }

        private void editCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCourseForm editcourse = new EditCourseForm();
            editcourse.ShowDialog();
        }

        private void manageCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageCouresesForm manageCouresesForm = new ManageCouresesForm();
            manageCouresesForm.ShowDialog();
        }

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PrintCourseForm printCourseForm = new PrintCourseForm();
            printCourseForm.ShowDialog();
        }

        private void addScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddScoreForm addScoreForm = new AddScoreForm();
            addScoreForm.ShowDialog();
        }

        private void removeScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveScoreForm removeScoreForm = new RemoveScoreForm();
            removeScoreForm.ShowDialog();
        }

        private void manageScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageScoreForm manageScoreForm = new ManageScoreForm();
            manageScoreForm.ShowDialog();
        }

        private void avgScoreByCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AvgScoreByCourse avgScoreByCourse = new AvgScoreByCourse();
            avgScoreByCourse.ShowDialog();
        }

        private void printToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PrintScoreForm printScoreForm = new PrintScoreForm();
            printScoreForm.ShowDialog();
        }

        private void aVGByScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AvgResultByScore avgResultByScore = new AvgResultByScore();
            avgResultByScore.ShowDialog();
        }

        private void finalResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticResultForm staticResultForm = new StaticResultForm();
            staticResultForm.ShowDialog();
        }
    }
}
