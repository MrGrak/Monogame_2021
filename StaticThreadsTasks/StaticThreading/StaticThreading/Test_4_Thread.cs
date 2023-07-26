﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticThreading
{
    public static class Test_4_Thread
    {
        public static Particle[] Particles;
        public static Stopwatch St = new Stopwatch();

        //create/initialize particle array
        public static void Constructor()
        {
            Particles = new Particle[Globals.Size];
            Reset();
        }

        //reset particle array to known starting state
        public static void Reset()
        {
            for (int i = 0; i < Globals.Size; i++)
            {
                Particles[i].X = 0;
                Particles[i].Y = 0;
                Particles[i].State = 0;
                Particles[i].Id = 0;
            }
        }

        //apply some work to particle array
        public static void Update()
        {
            for (int i = 0; i < Globals.Size; i++)
            {
                Particles[i].X++;
                Particles[i].Y++;
                Particles[i].State++;
                Particles[i].Id++;
            }
        }

        //test system, write log
        public static void RunTest()
        {
            StringBuilder sb = new StringBuilder(4096);
            St.Start();

            Task Task1 = Task.Factory.StartNew(() => SubTest(sb, St));
            Task.WaitAll(Task1);

            St.Stop();
            sb.Append("\n\ncomplete, elapsed: "
                + St.ElapsedMilliseconds + " ms.");

            //write output log
            string Dir = Path.Combine(Globals.GetDir(), "Test_4_Task.txt");
            Debug.WriteLine("writing output to: " + Dir);
            using (StreamWriter w = new StreamWriter(Dir))
            { w.WriteLine(sb.ToString()); }
        }

        public static void SubTest(StringBuilder sb, Stopwatch St)
        {
            for (int i = 1; i < Globals.Iterations; i++)
            {
                Update();
                sb.Append("\nPos: " + Particles[0].X);
                sb.Append(" frame " + i + " elapsed: "
                    + St.ElapsedTicks + " ticks.");
            }
        }
    }
}