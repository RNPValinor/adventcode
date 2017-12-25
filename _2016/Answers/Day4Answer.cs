using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2016.Answers
{
    public class Day4Answer : IAnswer
    {
        private List<Sector> Sectors;

        private void Init()
        {
            string line;
            var file = new StreamReader("Data/day4rooms.list");
            this.Sectors = new List<Sector>();

            while ((line = file.ReadLine()) != null)
            {
                this.Sectors.Add(new Sector(line));
            }
        }

        void IAnswer.PartOne()
        {
            this.Init();

            var validSectors = this.Sectors.Where(s => s.IsValid());

            Console.WriteLine("Valid sector sum is " + validSectors.Select(s => s.SectorID).Sum());
        }

        void IAnswer.PartTwo()
        {
            throw new System.NotImplementedException();
        }
    }

    class Sector
    {
        public string Name { get; set; }
        public int SectorID { get; set; }
        public string Checksum { get; set; }

        public Sector (string data)
        {
            var checksum = data.Substring(data.Length - 7);

            this.Checksum = checksum.Substring(1, 5);

            var nameData = data.Substring(0, data.Length - 7).Split('-');

            this.SectorID = int.Parse(nameData[nameData.Length - 1]);
            this.Name = data.Substring(0, data.Length - 7 - nameData[nameData.Length - 1].Length - 1);
        }

        public bool IsValid()
        {
            var counts = this.Name
                .Where(c => c != '-')
                .GroupBy((c) => c)
                .Select((g) => { return new CountedCharacter { Character = g.Key, Count = g.Count() };})
                .ToList();

            counts.Sort(new CharComparer());
            
            var generatedChecksum = "";

            for (var i = 0; i < 5; i++)
            {
                generatedChecksum = generatedChecksum + counts[i].Character;
            }

            return this.Checksum == generatedChecksum;
        }
    }

    class CountedCharacter
    {
        public char Character { get; set; }
        public int Count { get; set; }
    }

    class CharComparer : IComparer<CountedCharacter>
    {
        int IComparer<CountedCharacter>.Compare(CountedCharacter x, CountedCharacter y)
        {
            if (x.Count != y.Count)
            {
                return y.Count.CompareTo(x.Count);
            }
            else
            {
                return x.Character.CompareTo(y.Character);
            }
        }
    }
}