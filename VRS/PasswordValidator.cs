using System;
using System.Windows.Forms;

namespace VRS
{
    public partial class PasswordValidator: Form
    {

        String rental_time_limit = "";
        String dirPath = @"C:\Velocity Rental Time";
        String filePath = @"C:\Velocity Rental Time\Velocity Rental.VRS";
        public PasswordValidator()
        {
            InitializeComponent();

        }
        private void button1_Click( object sender , EventArgs e )
        {

            PasswordValidator f3 = ( PasswordValidator )Application.OpenForms[ "PasswordValidator" ];
            f3.Close();
        }

        private void button2_Click( object sender , EventArgs e )
        {
            if ( textBox1.Text == "!SERVICE!98" )
            {
                TimeExtenderWindow timeExtenderWindow = new TimeExtenderWindow();

                DialogResult dialogResult = timeExtenderWindow.ShowDialog();

                if ( dialogResult == DialogResult.OK )
                {
                    //Console.WriteLine("You clicked Extend Time");
                }
                else if ( dialogResult == DialogResult.Cancel )
                {
                    ActiveForm.Close();
                    //Console.WriteLine("You clicked either Cancel or X button in the top right corner");
                }
                timeExtenderWindow.Dispose();
            }
            else
            {
                label2.Visible = true;
                label2.Text = "Incorrect Password";
            }
        }

        private void PasswordValidator_Load( object sender , EventArgs e )
        {

        }
    }
}
