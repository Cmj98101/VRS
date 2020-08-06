using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VRS
{
    public partial class Form1: Form
    {
        String rental_time_limit = "10/12/2019 8:00:00 AM";
        String dirPath = @"C:\Velocity Rental Time";
        String filePath = @"C:\Velocity Rental Time\Velocity Rental.VRS";

        String read_file;
        string[] file_info;
        public Form1()
        {
            InitializeComponent();
            setup();
        }
        private void setup()
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            timer4.Stop();
            bool time_altered;

            //File.SetAttributes(dirPath, FileAttributes.Normal | FileAttributes.Normal);

            CreatePath( dirPath , filePath );

            using ( StreamReader sr = new StreamReader( filePath ) )
            {

                try
                {
                    read_file = sr.ReadLine();
                    file_info = read_file.Split( ',' );

                }
                catch ( Exception error )
                {
                    MessageBox.Show( $"No data in the file:  {error}" );
                }

                if ( file_info[ 0 ] == "Time Altered" )
                {
                    time_altered = true;
                }
                else
                {
                    time_altered = false;
                }
            }
            if ( time_altered == true )
            {
                timer3.Start();
            }
            else
            {
                try
                {
                    check_time();
                    set_time();
                }
                catch ( Exception error )
                {
                    timer4.Start();
                    MessageBox.Show( $"Check_time or set_time failed {error}" );

                }

            }
        }
        private void CreatePath( String dirPath , String filePath )
        {
            if ( Directory.Exists( dirPath ) && File.Exists( filePath ) )
            {
                Console.WriteLine( $"Directory Exists: {dirPath} & File Exists: {filePath}" );
                //MessageBox.Show($"Path Exists");

                return;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory( dirPath );
                    using ( StreamWriter sw = new StreamWriter( filePath ) )
                    {
                        DateTime time = DateTime.Now;
                        sw.WriteLine( $"{time.ToString()},{rental_time_limit}" );
                    }
                    File.SetAttributes( @"C:\Velocity Rental Time" , FileAttributes.Hidden | FileAttributes.System );

                    MessageBox.Show( "Path Created & Time Set" );

                }
                catch ( Exception error )
                {
                    Console.WriteLine( $"Create Path Error:{error}" );
                    MessageBox.Show( $"Create Path Error:{error}" );
                    return;
                }

            }

        }
        private bool check_for_reverse_time_change()
        {
            // File.SetAttributes(@"C:\Velocity Rental Time", FileAttributes.Normal | FileAttributes.Normal);

            using ( StreamReader sr = new StreamReader( filePath ) )
            {
                DateTime time_stamp = DateTime.Now;
                String[] time_stamp_seperated = time_stamp.ToString().Split( ' ' );
                string[] date_now = time_stamp_seperated[ 0 ].Split( '/' );
                string[] time_now = time_stamp_seperated[ 1 ].Split( ':' );
                string dayTime_now = time_stamp_seperated[ 2 ];

                String read_file = sr.ReadLine();
                string[] file_info = read_file.Split( ',' );

                // Split file Read into 3 sections
                string[] file_time_seperated = file_info[ 0 ].Split( ' ' );
                // Split each individual section
                string[] date = file_time_seperated[ 0 ].Split( '/' );
                string[] time = file_time_seperated[ 1 ].Split( ':' );
                string dayTime = file_time_seperated[ 2 ];
                Console.WriteLine( $"{file_info[ 0 ]}" );

                if ( file_info[ 0 ] == "Time Altered" )
                {
                    timer1.Stop();
                    timer2.Stop();
                    timer3.Start();
                    return false;
                }
                else if ( int.Parse( date_now[ 0 ] ) < int.Parse( date[ 0 ] ) )
                {
                    Console.WriteLine( "DATE NOW IS LESS THAN THE CURRENT DATE (MONTH)" );

                    return false;
                }
                else if ( int.Parse( date_now[ 0 ] ) == int.Parse( date[ 0 ] ) && int.Parse( date_now[ 1 ] ) < int.Parse( date[ 1 ] ) )
                {
                    Console.WriteLine( "DATE NOW IS LESS THAN THE CURRENT DATE (MONTH/DATE)" );

                    return false;
                }
                else if ( int.Parse( date_now[ 0 ] ) == int.Parse( date[ 0 ] ) && int.Parse( date_now[ 1 ] ) == int.Parse( date[ 1 ] ) )
                {
                    if ( dayTime_now == dayTime )
                    {
                        Console.WriteLine( $"DAYTIME_LIMIT IS {dayTime_now} and DAYTIME IS {dayTime} IT IS EQUAL" );
                        if ( int.Parse( time_now[ 0 ] ) < int.Parse( time[ 0 ] ) )
                        {
                            Console.WriteLine( $"LIMIT: {time_now[ 0 ]}:{time_now[ 1 ]}... TIME: {time[ 0 ]}:{time[ 1 ]} TIME NOW IS LESS THAN TIME " );

                            return false;
                        }
                        else if ( int.Parse( time_now[ 0 ] ) == int.Parse( time[ 0 ] ) && int.Parse( time_now[ 1 ] ) < int.Parse( time[ 1 ] ) )
                        {
                            Console.WriteLine( "HOUR IS EQUAL BUT TIME IS (MIN) IS LESS THAN THE TIME NOW" );

                            return false;
                        }
                    }
                    else if ( dayTime_now == "AM" && dayTime == "PM" )
                    {
                        Console.WriteLine( $"DAYTIME NOW IS (AM) {dayTime_now} and DAYTIME IS (PM) {dayTime} IT DOES NOT MATCH" );

                        return false;
                    }
                    else if ( dayTime_now == "PM" && dayTime == "AM" )
                    {
                        Console.WriteLine( $"DAYTIME NOW IS (PM) {dayTime_now} and DAYTIME IS (AM) {dayTime} IT DOES NOT MATCH" );

                        return false;
                    }

                }
            }
            return true;
        }
        private void set_time()
        {
            CreatePath( dirPath , filePath );
            if ( check_for_reverse_time_change() == true )
            {
                using ( StreamWriter sw = new StreamWriter( filePath ) )
                {
                    DateTime time = DateTime.Now;
                    sw.WriteLine( $"{time.ToString()},{rental_time_limit}" );
                }
                File.SetAttributes( @"C:\Velocity Rental Time" , FileAttributes.Hidden | FileAttributes.System );

            }
            else if ( check_for_reverse_time_change() == false )
            {
                using ( StreamWriter sw = new StreamWriter( filePath ) )
                {
                    sw.WriteLine( $"Time Altered,{rental_time_limit}" );
                }
                File.SetAttributes( @"C:\Velocity Rental Time" , FileAttributes.Hidden | FileAttributes.System );
                timer1.Stop();
                timer2.Stop();
                timer3.Start();
            }


        }
        private void check_time()
        {
            // If check_for_reverse_time_change() is True then
            if ( check_for_reverse_time_change() == true )
            {
                //File.SetAttributes(@"C:\Velocity Rental Time", FileAttributes.Normal | FileAttributes.Normal);

                using ( StreamReader sr = new StreamReader( filePath ) )
                {
                    String read_file = sr.ReadLine();
                    string[] file_info = read_file.Split( ',' );
                    Console.WriteLine( $"{file_info[ 1 ]}" );
                    rental_time_limit = file_info[ 1 ];

                    // Split file Read into 3 sections
                    string[] file_time_seperated = file_info[ 0 ].Split( ' ' );
                    // Split each individual section
                    string[] date = file_time_seperated[ 0 ].Split( '/' );
                    string[] time = file_time_seperated[ 1 ].Split( ':' );
                    string dayTime = file_time_seperated[ 2 ];

                    // Split file Read into 3 sections
                    string[] rental_time_seperated = file_info[ 1 ].Split( ' ' );
                    // Split each individual section
                    string[] date_limit = rental_time_seperated[ 0 ].Split( '/' );
                    string[] time_limit = rental_time_seperated[ 1 ].Split( ':' );
                    string dayTime_limit = rental_time_seperated[ 2 ];
                    if ( file_info[ 0 ] == "Time Altered" )
                    {
                        timer1.Stop();
                        timer2.Stop();
                        timer3.Start();
                        return;
                    }
                    Console.WriteLine( $"LIMIT: {date_limit[ 0 ]}/{date_limit[ 1 ]} ... DATE: {date[ 0 ]}/{date[ 1 ]}   LIMIT: {time_limit[ 0 ]}:{time_limit[ 1 ]}... TIME: {time[ 0 ]}:{time[ 1 ]}     LIMIT: {dayTime_limit}... DAYTIME: {dayTime}" );
                    if ( int.Parse( date_limit[ 0 ] ) > int.Parse( date[ 0 ] ) )
                    {
                        Console.WriteLine( "LIMIT IS GREATER THAN THE CURRENT DATE (MONTH)" );
                        timer3.Stop();
                        timer2.Stop();
                        timer1.Start();
                    }
                    else if ( int.Parse( date_limit[ 0 ] ) < int.Parse( date[ 0 ] ) )
                    {
                        Console.WriteLine( "LIMIT IS LESS THAN THE CURRENT DATE (MONTH)" );
                        timer3.Stop();
                        timer1.Stop();
                        timer2.Start();
                    }
                    else if ( int.Parse( date_limit[ 0 ] ) <= int.Parse( date[ 0 ] ) && int.Parse( date_limit[ 1 ] ) < int.Parse( date[ 1 ] ) )
                    {
                        Console.WriteLine( "LIMIT IS LESS THAN THE CURRENT DATE (MONTH/DATE)" );
                        timer3.Stop();
                        timer1.Stop();
                        timer2.Start();
                    }
                    else if ( int.Parse( date_limit[ 0 ] ) >= int.Parse( date[ 0 ] ) && int.Parse( date_limit[ 1 ] ) > int.Parse( date[ 1 ] ) )
                    {
                        Console.WriteLine( "LIMIT IS GREATER THAN THE CURRENT DATE (MONTH/DATE)" );
                        timer3.Stop();
                        timer2.Stop();
                        timer1.Start();
                    }

                    else if ( int.Parse( date_limit[ 0 ] ) == int.Parse( date[ 0 ] ) && int.Parse( date_limit[ 1 ] ) == int.Parse( date[ 1 ] ) )
                    {
                        if ( dayTime_limit == dayTime )
                        {
                            Console.WriteLine( $"DAYTIME_LIMIT IS {dayTime_limit} and DAYTIME IS {dayTime} IT IS EQUAL" );
                            if ( int.Parse( time_limit[ 0 ] ) > int.Parse( time[ 0 ] ) )
                            {
                                Console.WriteLine( $"LIMIT: {time_limit[ 0 ]}:{time_limit[ 1 ]}... TIME: {time[ 0 ]}:{time[ 1 ]} LIMIT IS GREATER THAN TIME " );
                                timer3.Stop();
                                timer2.Stop();
                                timer1.Start();
                            }
                            else if ( int.Parse( time_limit[ 0 ] ) == int.Parse( time[ 0 ] ) && int.Parse( time_limit[ 1 ] ) >= int.Parse( time[ 1 ] ) )
                            {
                                Console.WriteLine( "HOUR IS EQUAL BUT TIME IS (MIN) IS GREATER" );
                                timer3.Stop();
                                timer2.Stop();
                                timer1.Start();
                            }
                            else
                            {
                                Console.WriteLine( "LIMIT IS LESS THAN TIME" );
                                timer3.Stop();
                                timer1.Stop();
                                timer2.Start();
                            }
                        }
                        else if ( dayTime_limit == "AM" && dayTime == "PM" )
                        {
                            Console.WriteLine( $"DAYTIME_LIMIT IS (AM) {dayTime_limit} and DAYTIME IS (PM) {dayTime}" );
                            timer3.Stop();
                            timer1.Stop();
                            timer2.Start();
                        }
                        else if ( dayTime_limit == "PM" && dayTime == "AM" )
                        {
                            Console.WriteLine( $"DAYTIME_LIMIT IS (PM) {dayTime_limit} and DAYTIME IS (AM) {dayTime}" );
                            timer3.Stop();
                            timer2.Stop();
                            timer1.Start();
                        }
                    }


                }
                File.SetAttributes( @"C:\Velocity Rental Time" , FileAttributes.Hidden | FileAttributes.System );

            }
            else
            {
                //File.SetAttributes(@"C:\Velocity Rental Time", FileAttributes.Normal | FileAttributes.Normal);

                // If check_for_reverse_time_change() is false then
                label1.Text = "YOU HAVE ALTERED THE TIME!";
                using ( StreamWriter sw = new StreamWriter( filePath ) )
                {
                    sw.WriteLine( $"Time Altered,{rental_time_limit}" );
                }
                File.SetAttributes( @"C:\Velocity Rental Time" , FileAttributes.Hidden | FileAttributes.System );

                timer1.Stop();
                timer2.Stop();
                timer3.Start();
            }


        }
        private void Form1_Load( object sender , EventArgs e )
        {

        }
        private void timer1_Tick( object sender , EventArgs e )
        {
            Console.WriteLine( "T1 IS RUNNING" );
            label1.Text = "";
            DateTime time = DateTime.Now;
            label1.Text = $"{time.ToString()}";
            try
            {
                check_time();
                set_time();
            }
            catch ( Exception error )
            {
                timer4.Start();
                MessageBox.Show( $"Check_time or set_time failed {error}" );

            }

        }
        private void timer2_Tick( object sender , EventArgs e )
        {
            Console.WriteLine( "T2 IS RUNNING" );
            try
            {
                check_time();
                set_time();
            }
            catch ( Exception error )
            {
                timer4.Start();
                MessageBox.Show( $"Check_time or set_time failed {error}" );

            }

            label1.Text = $"RENTAL TIME EXPIRED:   {rental_time_limit}!";
            foreach ( var process in Process.GetProcessesByName( "Plot" ) )
            {
                process.Kill();
            }
        }
        private void timer3_Tick( object sender , EventArgs e )
        {
            Console.WriteLine( "T3 IS RUNNING" );
            label1.Text = "YOU ALTERED THE TIME!";
            foreach ( var process in Process.GetProcessesByName( "Plot" ) )
            {
                process.Kill();
            }
        }
        private void timer4_Tick( object sender , EventArgs e )
        {
            Console.WriteLine( "T4 IS RUNNING" );
            label1.Text = "Call for Support +1 (833)-275-6888";
            foreach ( var process in Process.GetProcessesByName( "Plot" ) )
            {
                process.Kill();
            }
        }
        private void extendTimeToolStripMenuItem_Click( object sender , EventArgs e )
        {
            PasswordValidator passwordValidatorDialog = new PasswordValidator();

            DialogResult dialogResult = passwordValidatorDialog.ShowDialog();

            if ( dialogResult == DialogResult.OK )
            {
                Console.WriteLine( "You clicked Extend Time" );
            }
            else if ( dialogResult == DialogResult.Cancel )
            {
                Console.WriteLine( "You clicked either Cancel or X button in the top right corner" );
            }
            passwordValidatorDialog.Dispose();
        }

        private void helpToolStripMenuItem_Click( object sender , EventArgs e )
        {
            MessageBox.Show( $"Rental Expiration: {rental_time_limit}" );
        }

        private void extendTimeToolStripMenuItem1_Click( object sender , EventArgs e )
        {
            PasswordValidator passwordValidatorDialog = new PasswordValidator();

            DialogResult dialogResult = passwordValidatorDialog.ShowDialog();

            if ( dialogResult == DialogResult.OK )
            {
                Console.WriteLine( "You clicked Extend Time" );
            }
            else if ( dialogResult == DialogResult.Cancel )
            {
                Console.WriteLine( "You clicked either Cancel or X button in the top right corner" );
            }
            passwordValidatorDialog.Dispose();
        }

        private void Form1_Load_1( object sender , EventArgs e )
        {

        }
    }
}
