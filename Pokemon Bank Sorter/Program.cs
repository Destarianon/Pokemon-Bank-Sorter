using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon_Bank_Sorter {
	class Program {
		//Globals
		public static int ROW_SIZE = 6;
		public static int ROW_COUNT = 5;
		public static int PAGE_SIZE = ROW_SIZE * ROW_COUNT;
		public static int MAX = 810;

		//Program
		static void Main(string[] args) {

			//load pokedex file
			List<string> pokedex = new List<string>();
			pokedex.Add("null");

			bool pokedex_isavailable = true;
			try {
				string[] lines = System.IO.File.ReadAllLines("dex.txt");

				foreach (string line in lines) {
					var result = line.Split(',');
					try {
						int dex_num = int.Parse(result[0]);
						pokedex.Add(result[1].ToLower());
					} catch (FormatException) {
						throw new Exception("Invalid entry in pokedex file");
					}
				}

			} catch (FileNotFoundException) {
				pokedex_isavailable = false;
			}

			bool run = true;

			Console.Out.Write("Enter a pokedex number");
			if (pokedex_isavailable) {
				Console.Out.Write(" [Name lookup is also available]");
			}
			Console.Out.WriteLine("\nor type \"quit\" to exit");

			while (run) {

				//print prompt
				Console.Out.Write("#");

				//get input
				string input = Console.ReadLine().ToLower();
				if (input.Equals("quit")) {
					break;
				}

				//transform to int
				int dex = 0;
				try {
					dex = int.Parse(input);
				} catch (FormatException) {

					//test if name exists
					if (pokedex_isavailable) {
						dex = pokedex.IndexOf(input);
						if (dex == -1) {
							Console.Out.WriteLine("Unknown pokemon name");
						}
					} else dex = -1;
				}

				//test in valid range
				if (dex < 1 || dex > MAX) {
					Console.Out.WriteLine("Invalid pokedex number");
					continue;
				}

				//calculate page
				int page_offset = (int)Math.Ceiling((double)dex / PAGE_SIZE) - 1;
				int page_start = page_offset * PAGE_SIZE;
				int page_end = page_start + PAGE_SIZE;

				//calculate position
				int page_index = (dex - page_start) - 1;
				int row = page_index / ROW_SIZE;
				int col = (page_index % ROW_SIZE);

				//output textual
				if (pokedex_isavailable) {
					Console.Out.WriteLine("name:     " + pokedex.ElementAt(dex) + ": " + dex);
				}
				Console.Out.WriteLine("page:     " + page_start + "-" + page_end);
				Console.Out.WriteLine("position: [" + (row + 1) + "," + (col + 1) + "]\n");

				//output graphical
				char[,] box = new char[ROW_COUNT, ROW_SIZE];
				for (int box_r = 0; box_r < ROW_COUNT; box_r++) {
					for (int box_c = 0; box_c < ROW_SIZE; box_c++) {
						box[box_r, box_c] = '-';
					}
				}
				box[row, col] = 'X';

				for (int box_r = 0; box_r < ROW_COUNT; box_r++) {
					for (int box_c = 0; box_c < ROW_SIZE; box_c++) {
						Console.Out.Write(box[box_r, box_c]);
					}
					Console.Out.Write("\n");
				}
				Console.Out.Write("\n");
			}

		}
	}
}
