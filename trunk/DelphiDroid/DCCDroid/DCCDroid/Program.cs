using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DCCDroid.Logic.Exceptions;

namespace DCCDroid
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Delphi For Android " + AppUtils.GetAppVersionNumber() + " Dev Build");
            Console.WriteLine("Copyright © 2010 by Lennie De Villiers"); // TODO: Read from Assembly info
            Console.WriteLine();

            try
            {
                #if DEBUG

                String projectName = "DelphiAndroid";
                String activityName = "DelphiAndroid";
                String packageName = "com.test";
                String inputPath = @"c:\Demo\DelphiDTest\";
                String outputPath = @"c:\Demo\Test\";
                
                #else
                if (args.Length != 5)
                {
                    StringBuilder invalidArguments = new StringBuilder();
                    invalidArguments.Append("Invalid arguments, the following arguments are required: \n\n");
                    invalidArguments.Append("<Project Name> - Name of the project\n");
                    invalidArguments.Append("<Activity Name> - Name of the activity\n");
                    invalidArguments.Append("<Package Name> - Java package name\n");
                    invalidArguments.Append("<Input Source Path> - Delphi source directory path\n");
                    invalidArguments.Append("<Android Output Path> - Android output directory path\n");
                    throw new ArgumentException(invalidArguments.ToString());
                }

                String projectName = args[0].Trim();
                String activityName = args[1].Trim();
                String packageName = args[2].Trim();
                String inputPath = AppUtils.GetDirectory(args, 3, false);
                String outputPath = AppUtils.GetDirectory(args, 4, true);
                
                #endif

                AppConfiguration.Instance.SetValues(projectName, activityName, packageName, inputPath, outputPath);
                AppConfiguration.Instance.Validate();

                AndroidProjectManager.Instance.CreateProject();
                FormLayoutManager.Instance.CompileToText();

                AppConfiguration.Instance.ControlsList = FormLayoutManager.Instance.GetFormLayout();

                AndroidLayoutManager.Instance.CreateMainLayout();

                ActivityManager.Instance.Update();
                SourceCodeManager.Instance.CopyFramework();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CompileErrorException ex)
            {
                Console.WriteLine("\nCompiler halt error, unable to continue because: \n");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Compiler Error: {0}, {1}", ex.Message, ex.StackTrace);
            }

            #if DEBUG
                Console.ReadLine();
            #endif
        }
    }
}
