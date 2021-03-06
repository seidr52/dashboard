﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dashboard
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string option = args[0];
                args = args.Where((e, i) => i != 0).ToArray();

                if (args.Length > 0)
                {
                    switch (option)
                    {
                        case "-p":
                            DoProfile(args);
                            break;
                        case "-e":
                            DoEnvironment(args);
                            break;
                        case "-a":
                            DoAdd(args);
                            break;
                    }
                }
            }
        }

        private static Boolean DoProfile(string[] args)
        {
            Boolean result = false;

            try
            {
                foreach (string arg in args)
                {
                    Profile profile = new Profile();
                    profile.GetProfile(arg);

                    if (!String.IsNullOrEmpty(profile.Exe))
                    {
                        //Console.WriteLine(profile["exe"]);
                        Profile.RunProfile(profile);

                        result = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Resource.WriteLog(exception);
            }

            return result;
        }

        private static Boolean DoEnvironment(string[] args)
        {
            Boolean result = false;

            try
            {
                dynamic environment = Workspace.GetEnvironment(args[0]);

                foreach (JObject arg in environment)
                {
                    Profile profile = new Profile();
                    profile.GetProfile(arg.ToObject<Dictionary<string, string>>());

                    if (!String.IsNullOrEmpty(profile.Exe))
                    {
                        //Console.WriteLine(profile["exe"]);
                        Profile.RunProfile(profile);

                        result = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Resource.WriteLog(exception);
            }

            return result;
        }

        private static Boolean DoAdd(string[] args)
        {
            Boolean result = false;

            try
            {
                string environment = args[0];
                string source = args[1];

                if (environment.Length > 0 && source.Length > 0)
                {
                    IntPtr hwnd;

                    if (source == "active")
                    {
                        hwnd = Profile.GetHWND();
                    }
                    else
                    {
                        hwnd = Profile.GetHWND(source);
                    }


                    if (hwnd != IntPtr.Zero)
                    {
                        Profile profile = new Profile();
                        profile.GetProfile(hwnd);

                        Workspace.AddProfileToEnvironment(profile, environment);

                        result = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Resource.WriteLog(exception);
            }

            return result;
        }
    }
}
