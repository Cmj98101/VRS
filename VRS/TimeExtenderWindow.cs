using System;
using System.IO;
using System.Windows.Forms;

namespace VRS
{
    public partial class TimeExtenderWindow: Form
    {
        String rental_time_limit = "6/3/2019 8:00:00 PM";
        String dirPath = @"C:\Velocity Rental Time";
        String filePath = @"C:\Velocity Rental Time\Velocity Rental.VRS";

        public TimeExtenderWindow()
        {
            InitializeComponent();
            textBox1.Text = rental_time_limit;


        }
        private void CreatePath( String dirPath , String filePath )
        {
            if ( Directory.Exists( dirPath ) && File.Exists( filePath ) )
            {
                Console.WriteLine( $"Directory Exists: {dirPath} & File Exists: {filePath}" );
                return;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory( dirPath );
                    File.Create( filePath );
                }
                catch ( Exception error )
                {
                    Console.WriteLine( $"Create Path Error:{error}" );
                    MessageBox.Show( $"Create Path Error:{error}" );
                    return;
                }
            }

        }
        private void set_time()
        {
            CreatePath( dirPath , filePath );
            using ( StreamWriter sw = new StreamWriter( filePath ) )
            {
                DateTime time = DateTime.Now;
                sw.WriteLine( $"{time.ToString()},{rental_time_limit}" );
            }
        }
        private void button1_Click( object sender , EventArgs e )
        {
            rental_time_limit = textBox1.Text;
            Console.WriteLine( $"{rental_time_limit}" );
            set_time();
        }

        private void button2_Click( object sender , EventArgs e )
        {
            set_time();
            MessageBox.Show( "Clean Slate: Restart Application!" );
        }


    }
}
